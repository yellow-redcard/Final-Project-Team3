using Goldmetal.UndeadSurvivor;
using UnityEngine;
using UnityEngine.UI;


    public class Item : MonoBehaviour
    {
        public ItemData data; // ������ ������
        public int level; // ���� ������ ����
        public Gear gear; // ����� Gear

        private Image icon;
        private Text textLevel;
        private Text textName;
        private Text textDesc;

        void Awake()
        {
            // UI ��� �ʱ�ȭ
            icon = GetComponentsInChildren<Image>()[1];
            icon.sprite = data.itemIcon;

            Text[] texts = GetComponentsInChildren<Text>();
            textLevel = texts[0];
            textName = texts[1];
            textDesc = texts[2];
            textName.text = data.itemName;
        }

        void OnEnable()
        {
            // ������ ���� ������Ʈ
            textLevel.text = "Lv." + (level + 1);
            textDesc.text = string.Format(data.itemDesc, data.damages[level]);
        }

        public void OnClick()
        {
            // ������ ���
            switch (data.itemType)
            {
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    HandleGearItem();
                    break;

                case ItemData.ItemType.Heal:
                    HealPlayer();
                    break;
            }

            // ���� ���� �� �ִ� ���� Ȯ��
            level++;
            if (level >= data.damages.Length)
            {
                GetComponent<Button>().interactable = false; // ��ư ��Ȱ��ȭ
            }
        }

        private void HandleGearItem()
        {
            if (gear == null) // Gear�� ���� ��� ���� ����
            {
                GameObject newGear = new GameObject(data.itemName);
                gear = newGear.AddComponent<Gear>();
                gear.Init(data); // Gear �ʱ�ȭ
            }
            else
            {
                float nextRate = data.damages[level];
                gear.LevelUp(nextRate); // Gear ������
            }

            Debug.Log($"[Item] {data.itemName} ���. Level: {level + 1}");
        }

        private void HealPlayer()
        {
            // �÷��̾� ü�� ȸ�� ó��
            Debug.Log("�÷��̾� ü�� ȸ��!");
            GameManager.Instance.playerMovement.RestoreHealth();
        }
    }


