using DimensionAdventurer.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Enemies
{
    public class AttackIndicator : MonoBehaviour
    {
        private static float Y_OFFSET = .5f;


        /// <summary>
        /// The position of the indicator. 
        /// This should not be adjusted in the runtime.
        /// </summary>
        public WorldPosition wPosition;

        /// <summary>
        /// The material of the inficator when it is not displayed.
        /// </summary>
        public Material normalMat;

        /// <summary>
        /// The material of the indicator when it is displayed.
        /// </summary>
        public Material displayMat;

        private MeshRenderer _meshRenderer;



        #region MonoBehaviour
        private void Start()
        {
            //Register to dictionary
            AttackIndicatorManager.singleton.indicators.Add(wPosition, this);
            //Debug.Log(DicAtkIndicator.Count + ": (p: " + wPosition.ToString());

            //Get renderer for changing materials
            _meshRenderer = GetComponent<MeshRenderer>();

            //Register event
            Portal.PortalEnteredEvent += OnPlayerEnterPortal;

            //Set transform to fit the scene
            gameObject.transform.localEulerAngles = new Vector3(0, 0, WorldPosition.ConverEnumToRotation(wPosition.Plane));
            gameObject.transform.localPosition = AttackIndicatorManager.singleton.EnvironmentData.ConvertEnumToPosition(wPosition, Y_OFFSET);
            gameObject.transform.localScale = new Vector3(AttackIndicatorManager.singleton.EnvironmentData.TileWidth / 3f, 0.1f, 1f);
        }
        #endregion



        #region Methods
        /// <summary>
        /// Display the indicator
        /// </summary>
        /// <param name="time">Seconds of display</param>
        public IEnumerator IEDisplay(float time)
        {
            _meshRenderer.material = displayMat;
            yield return new WaitForSeconds(time);
            _meshRenderer.material = normalMat;
        }

        /// <summary>
        /// Display the indicator. Overload with void
        /// </summary>
        /// <param name="time">Seconds of display</param>
        public void Display(float time)
        {
            StartCoroutine(IEDisplay(time));
        }
        #endregion

        #region Event
        private void OnPlayerEnterPortal()
        {
            StopAllCoroutines();
            _meshRenderer.material = normalMat;
        }
        #endregion
    }
}