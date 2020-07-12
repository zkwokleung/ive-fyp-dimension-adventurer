using UnityEngine;

namespace DimensionAdventurer.Math
{
    public class MathHelper
    {
        public static void PreventNegative(float number)
        {
            if (number < 0)
                Debug.LogError("The number should not be negative!");
        }

        public static void PreventNegative(int number)
        {
            if (number < 0)
                Debug.LogError("The number should not be negative!");
        }

    }
}