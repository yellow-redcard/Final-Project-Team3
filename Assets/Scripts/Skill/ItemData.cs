using UnityEngine;

    [CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
    public class ItemData : ScriptableObject
    {
        public enum ItemType { Melee, Range, Glove, Shoe, Heal }

        [Header("# Main Info")]
        public ItemType itemType; // ������ Ÿ��
        public int itemId; // ������ ID
        public string itemName; // ������ �̸�
        [TextArea]
        public string itemDesc; // ������ ����
        public Sprite itemIcon; // ������ ������

        [Header("# Level Data")]
        public float[] damages; // ������ ȿ�� ��
    }

