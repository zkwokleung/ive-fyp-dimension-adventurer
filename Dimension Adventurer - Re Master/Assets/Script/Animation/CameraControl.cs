using DimensionAdventurer.Players;
using System.Collections;
using UnityEngine;

namespace DimensionAdventurer.Animations
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] [Range(0, 1)] private float RunningShakeMagnitude = 1f;
        [SerializeField] private float HeadUpSpeed;
        [SerializeField] private float HeadDownSpeed;

        public bool EnableRunningShake = false;
        public bool EnableInjureShake;
        public float injureShakeDuration = 1f;
        public float InjureShakeMagnitude = 1f;

        public Vector3 ShakeDirection
        {
            get => _shakeDirection;

            private set
            {
                _shakeDirection = value;
                _shakeTargetPos = value * RunningShakeMagnitude + transform.localPosition;
            }
        }
        private Vector3 _shakeDirection;
        private Vector3 _shakeTargetPos;

        #region MonoBehaviour
        private void Start()
        {
            ShakeDirection = Vector3.up;
        }

        private void Update()
        {
            if (GameManager.Paused)
                return;

            if (EnableRunningShake)
            {
                if (Vector3.Distance(transform.localPosition, _shakeTargetPos) > 0.05f)
                    transform.localPosition = Vector3.LerpUnclamped(transform.localPosition, _shakeTargetPos, ((ShakeDirection == Vector3.up) ? HeadUpSpeed : HeadDownSpeed) * Time.deltaTime);
                else
                    ShakeDirection = -ShakeDirection;
            }
        }

        private void OnEnable()
        {
            player.DamageEvent += OnPlayerDamaged;
            player.DeathEvent += OnPlayerDead;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        #endregion

        #region Public Functions
        public void InjureShake()
        {
            if (EnableInjureShake)
                CameraShake(injureShakeDuration, InjureShakeMagnitude);
        }

        public void CameraShake(float duration, float magnitude)
        {
            CoroutineHandler.StartCoroutine("CameraShake", IECameraShake(duration, magnitude));
        }
        #endregion

        #region Private Enumerators
        /// <summary>
        /// The effect of camera shaking.
        /// </summary>
        /// <param name="duration">Seconds of the shake</param>
        /// <param name="magnitude">The magnitude of the shake</param>
        private IEnumerator IECameraShake(float duration, float magnitude)
        {
            Vector3 originalPos = transform.localPosition;

            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(x, y, originalPos.z);
                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = originalPos;
        }
        #endregion

        #region Event
        public void OnPlayerDamaged()
        {
            InjureShake();
        }

        public void OnPlayerDead()
        {
            StopAllCoroutines();
        }
        #endregion
    }
}
