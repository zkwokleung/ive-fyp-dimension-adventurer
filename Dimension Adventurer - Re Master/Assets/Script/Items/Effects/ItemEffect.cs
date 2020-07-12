using UnityEngine;

namespace DimensionAdventurer.Items.Effects
{
    public class ItemEffect : ScriptableObject
    {
        public new string name;
        [Multiline] public string description;

        public virtual void ExecuteEffect(GameObject source, ItemEffectEventArgs e)
        {

        }
    }
}