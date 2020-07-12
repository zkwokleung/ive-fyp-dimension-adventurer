using System;
using UnityEngine;

namespace DimensionAdventurer.Inputs
{
    public enum Gesture
    {
        Tap,
        DoubleTap,
        Swipe,
        Drag,
        DragEnd,
        LongPressStart,
        LongPressEnd
    }

    public class MobileInputEventArgs : EventArgs
    {
        public Gesture gesture;

        public Touch touch;
        public int touchId;
        public Vector2 position;
        public Vector2 startPos;
        public float distance = 0;
        public float startTime = 0;
        public float elapsedTime = 0;

        public MobileInputEventArgs(MobileInputEventSystem inputSystem)
        {
            this.touchId = inputSystem.TargetTouchIndex;
            this.distance = inputSystem.Distance;
            this.startTime = inputSystem.StartTime;
            this.elapsedTime = inputSystem.touchElapsedTime;
            this.position = inputSystem.CurrentPos;
            this.startPos = inputSystem.StartPos;
            touch = Input.GetTouch(touchId);
        }
    }
}
