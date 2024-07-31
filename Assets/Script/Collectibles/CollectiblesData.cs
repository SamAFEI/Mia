using UnityEngine;

namespace Assets.Script.Collectibles
{
    [CreateAssetMenu(menuName = "Collect Data")]
    public class CollectiblesData : ScriptableObject
    {
        public enum emType
        {
            RedSoul,GreedSoul
        }
        public emType Type;
        public int BaseValue;
    }
}
