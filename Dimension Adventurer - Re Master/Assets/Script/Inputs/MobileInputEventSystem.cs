using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Inputs
{
    public class MobileInputEventSystem : MonoBehaviour
    {
        /// <summary>
        /// The interval between the finger touchs and lift to determint it is a tap
        /// </summary>
        private const float SINGLE_TAP_THRESHOLD = 0.2f;
        /// <summary>
        /// The interval between two taps. 
        /// It will be considered as a double tap if the interval time is lower than this value
        /// </summary>
        private const float DOUBLE_TAP_THRESHOLD = 0.3f;
        /// <summary>
        /// How far does the finger travel before it considered as a swipe
        /// </summary>
        private const float SWIPE_THRESHOLD = 70f;
        /// <summary>
        /// Maximum time allowed to perform a swipe
        /// </summary>
        private const float SWIPE_ALLOWED_TIME = 0.5f;
        /// <summary>
        /// How far does the finger travel before it considered as a drag
        /// </summary>
        private const float DRAG_THRESHOLD = 100f;
        /// <summary>
        /// How long to press before it considered as a long press
        /// </summary>
        private const float LONG_PRESS_THRESHOLD = 0.4f;

        public static Action<MobileInputEventArgs> MobileInputEvent;


        [SerializeField] public int TargetTouchIndex = 0;

        public Vector2 StartPos { get; private set; }
        public Vector2 CurrentPos { get; private set; }
        public float Distance { get; private set; } = 0;
        public float StartTime { get; private set; } = 0;
        public float touchElapsedTime { get; private set; } = 0;

        private float lastTapTime = 0;
        private int tapCount = 0;
        private bool pressing = false;
        private bool dragging = false;

        private void Awake()
        {
            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.WindowsEditor)
                Destroy(gameObject);
        }

        private void Update()
        {
            //No need to update when there is no input
            if (Input.touchCount < TargetTouchIndex + 1)
                return;

            CurrentPos = Input.GetTouch(TargetTouchIndex).position;

            // Touch begin
            if (Input.GetTouch(TargetTouchIndex).phase == TouchPhase.Began)
            {
                StartPos = Input.GetTouch(TargetTouchIndex).position;
                CurrentPos = StartPos;
                StartTime = Time.time;
                Distance = 0;
                touchElapsedTime = 0;
                if (Time.time - lastTapTime > DOUBLE_TAP_THRESHOLD)
                    tapCount = 0;
            }
            else if (Input.GetTouch(TargetTouchIndex).phase == TouchPhase.Moved)
            {
                Distance = Vector2.Distance(Input.GetTouch(TargetTouchIndex).position, StartPos);
                touchElapsedTime = Time.time - StartTime;

                // Drag
                if ((Distance > DRAG_THRESHOLD) || dragging)
                {
                    OnDrag();
                    dragging = true;
                }
            }
            else if (Input.GetTouch(TargetTouchIndex).phase == TouchPhase.Stationary)
            {
                touchElapsedTime = Time.time - StartTime;

                // Long press
                if (touchElapsedTime > LONG_PRESS_THRESHOLD && !dragging)
                {
                    if (!pressing && !dragging) // Double check
                    {
                        OnLongPressStart();
                        pressing = true;
                    }
                }
            }
            else if (Input.GetTouch(TargetTouchIndex).phase == TouchPhase.Ended)
            {
                CurrentPos = Input.GetTouch(TargetTouchIndex).position;
                touchElapsedTime = Time.time - StartTime;

                // Tapped once
                if (touchElapsedTime < SINGLE_TAP_THRESHOLD)
                {
                    lastTapTime = Time.time;
                    tapCount++;
                }
                else
                {
                    // Reset the tap count if the time exceed the threshold.
                    tapCount = 0;
                }
                if (dragging)
                {
                    OnDragEnd();
                    lastTapTime = 0;
                    dragging = false;

                    if (Distance > SWIPE_THRESHOLD && touchElapsedTime <= SWIPE_ALLOWED_TIME)
                    {
                        // Swipe
                        OnSwipe();
                        lastTapTime = 0;
                    }
                }
                else if (pressing)
                {
                    OnLongPressEnd();
                    lastTapTime = 0;
                    pressing = false;
                }
                else if (Time.time - lastTapTime < DOUBLE_TAP_THRESHOLD && tapCount % 2 == 0)
                {
                    // Double Tap
                    OnDoubleTap();
                }
                else if (tapCount > 0)
                {
                    // Single Tap
                    OnSingleTap();
                }
            }
        }

        #region Events
        private void OnSingleTap()
        {
            Debug.Log("Tap");
            if (MobileInputEvent != null)
                MobileInputEvent.Invoke(new MobileInputEventArgs(this)
                {
                    gesture = Gesture.Tap
                });
        }

        private void OnDoubleTap()
        {
            Debug.Log("DoubleTap");
            if (MobileInputEvent != null)
                MobileInputEvent.Invoke(new MobileInputEventArgs(this)
                {
                    gesture = Gesture.DoubleTap
                });
        }

        private void OnSwipe()
        {
            Debug.Log("Swipe");
            if (MobileInputEvent != null)
                MobileInputEvent.Invoke(new MobileInputEventArgs(this)
                {
                    gesture = Gesture.Swipe
                });
        }

        private void OnDrag()
        {
            Debug.Log("Drag");
            if (MobileInputEvent != null)
                MobileInputEvent.Invoke(new MobileInputEventArgs(this)
                {
                    gesture = Gesture.Drag
                });
        }

        private void OnDragEnd()
        {
            Debug.Log("DragEnd");
            if (MobileInputEvent != null)
                MobileInputEvent.Invoke(new MobileInputEventArgs(this)
                {
                    gesture = Gesture.DragEnd
                });
        }

        private void OnLongPressStart()
        {
            Debug.Log("LongPressStart");
            if (MobileInputEvent != null)
                MobileInputEvent.Invoke(new MobileInputEventArgs(this)
                {
                    gesture = Gesture.LongPressStart
                });
        }

        private void OnLongPressEnd()
        {
            Debug.Log("LongPressEnd");
            if (MobileInputEvent != null)
                MobileInputEvent.Invoke(new MobileInputEventArgs(this)
                {
                    gesture = Gesture.LongPressEnd
                });
        }

        #endregion
    }
}