using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace DimensionAdventurer.VR
{
    [RequireComponent(typeof(Camera))]
    public class VRGazeController : MonoBehaviour
    {
        [SerializeField] private RadialReticle _reticle;
        [SerializeField] private bool _showDebugRay;
        [SerializeField] private float _debugRayLength = 5f;
        [SerializeField] private float _debugRayDuration = 1f;
        [SerializeField] private float _rayLength = 500f;

        PointerEventData m_pointerEventData;
        [SerializeField] private EventSystem _eventSystem;

        private RaycastResult _currentTarget;
        private VRTargetItem _target;
        private VRTargetItem _previousTarget;

        private void Awake()
        {
            if (!GamePreference.singleton.isPlayInVR)
                this.enabled = false;
        }

        void Start()
        {
            if (!_eventSystem)
                _eventSystem = GameObject.Find("GvrEventSystem").GetComponent<EventSystem>();
        }

        void Update()
        {

            _showDebugRay = true;
            GazeRaycast();
        }

        private void GazeRaycast()
        {
            if (_showDebugRay)
            {
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * _debugRayLength, Color.blue, _debugRayDuration);
            }

            //Set up PointerEventData
            m_pointerEventData = new PointerEventData(_eventSystem);
            //Set PointEventData to the center of the screen

#if UNITY_EDITOR
            m_pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);
#else
            m_pointerEventData.position = new Vector2(Screen.width/ 4, Screen.height / 2);
#endif

            List<RaycastResult> results = new List<RaycastResult>();
            _eventSystem.RaycastAll(m_pointerEventData, results);

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.GetComponent<VRTargetItem>())
                {
                    _target = results[i].gameObject.GetComponent<VRTargetItem>();
                    _currentTarget = results[i];

                    if (_reticle)
                        _reticle.SetPosition(results[i]);
                    break;
                }

                _target = null;
            }

            if (_target && _target != _previousTarget)
            {
                _reticle.ShowRadialImage(true);
                _target.GazeEnter(m_pointerEventData);
                if (_previousTarget)
                    _previousTarget.GazeExit(m_pointerEventData);
                _reticle.StartProgress();
                _previousTarget = _target;
            }
            else if (_target && _target == _previousTarget)
            {
                if (_reticle.ProgressRadialImage())
                    CompleteSelection();
            }
            else
            {
                if (_previousTarget)
                    _previousTarget.GazeExit(m_pointerEventData);

                _target = null;
                _previousTarget = null;
                _reticle.ShowRadialImage(false);
                _reticle.ResetProgress();
                _reticle.SetPosition();
            }
        }

        private void CompleteSelection()
        {
            _reticle.ShowRadialImage(false);

            _target.GazeComplete(m_pointerEventData);
        }
    }


}