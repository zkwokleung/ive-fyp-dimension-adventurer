using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static iTween;

namespace DimensionAdventurer.Animations
{
    public class TransformAnimation
    {
        public static void PopUp(GameObject gameObject, float time = 1.5f, EaseType easeType = EaseType.easeInOutExpo)
        {
            gameObject.transform.localScale = Vector3.zero;
            ScaleTo(gameObject, Hash(
                "scale", new Vector3(1f, 1f, 1f),
                "time", time,
                "easetype", easeType
                ));
        }

        public static void Shrink(GameObject gameObject, float time = 1f, EaseType easeType = EaseType.linear)
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            ScaleTo(gameObject, Hash(
                "scale", Vector3.zero,
                "time", time,
                "easetype", easeType
                ));
        }
    }

}