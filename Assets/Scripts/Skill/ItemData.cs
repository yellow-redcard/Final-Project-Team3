using UnityEngine;

    [CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
    public class ItemData : ScriptableObject
    {
        public enum ItemType { Melee, Range, Glove, Shoe, Heal }

        [Header("# Main Info")]
        public ItemType itemType; // 아이템 타입
        public int itemId; // 아이템 ID
        public string itemName; // 아이템 이름
        [TextArea]
        public string itemDesc; // 아이템 설명
        public Sprite itemIcon; // 아이템 아이콘

        [Header("# Level Data")]
        public float[] damages; // 레벨별 효과 값
    }

