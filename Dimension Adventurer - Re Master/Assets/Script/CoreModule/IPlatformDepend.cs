using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer
{
    public interface IPlatformDepend
    {
        void OnStartStandalone();
        void OnStartMobile();
    }
}