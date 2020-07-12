using System.Collections.Generic;
using UnityEngine;
using DimensionAdventurer.Items.Effects;

namespace DimensionAdventurer.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Data/Item")]
    public class ItemData : ScriptableObject
    {
        public static List<string> SpecialItems = new List<string>();

        public new string name;
        public bool isSpecialItem = false;
        [Multiline]
        public string description;
        public List<ItemEffect> effects;

        public void Use(GameObject source, ItemEffectEventArgs e)
        {
            //Apply effects
            if(effects.Count > 0)
                foreach(ItemEffect ie in effects)
                {
                    e.Producer = this;
                    ie.ExecuteEffect(source, e);
                }
        }
    }
}