using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.UI
{
    public class LoaderAnimation : MonoBehaviour
    {
        private const string LOADING_ANIM_CORO_ID = "LoadingCircleAnimation";

        [Header("ITween Hash Properties")]
        [SerializeField] private float z = -1f;
        [SerializeField] private float time = 1f;
        [SerializeField] private iTween.EaseType easeType = iTween.EaseType.linear;

        #region MonoBehaviour
        private void OnEnable()
        {
            Play();
        }

        private void OnDisable()
        {
            Stop();
        }
        #endregion

        #region Public Method
        public void Play()
        {
            CoroutineHandler.StartCoroutine(LOADING_ANIM_CORO_ID, IELoadingAnimation());
        }

        public void Stop()
        {
            CoroutineHandler.StopCoroutine(LOADING_ANIM_CORO_ID);
        }
        #endregion

        #region Private Methods
        private IEnumerator IELoadingAnimation()
        {
            float waitTime = time + .01f;
            while (true)
            {
                //Debug.Log($"Circle:  {++i}");
                iTween.RotateBy(gameObject, iTween.Hash(
                    "z", z,
                    "time", time,
                    "easetype", easeType));
                yield return new WaitForSeconds(waitTime);
            }
        }
        #endregion
    }
}