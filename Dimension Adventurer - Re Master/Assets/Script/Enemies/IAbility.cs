using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Enemies
{
    /// <summary>
    /// The interface for different methods of enemies' abilities
    /// </summary>
    public interface IAbility
    {
        void Attack();
        void Howl();
        void Roar();
    }

}