using Goldmetal.UndeadSurvivor;
using UnityEngine;
using UnityEngine.UI;


    public class Item : MonoBehaviour
    {
        public ItemData data; // 아이템 데이터
        public int level; // 현재 아이템 레벨
        public Gear gear; // 연결된 Gear

        private Image icon;
        private Text textLevel;
        private Text textName;
        private Text textDesc;

        void Awake()
        {
            // UI 요소 초기화
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
            // 아이템 설명 업데이트
            textLevel.text = "Lv." + (level + 1);
            textDesc.text = string.Format(data.itemDesc, data.damages[level]);
        }

        public void OnClick()
        {
            // 아이템 사용
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

            // 레벨 증가 및 최대 레벨 확인
            level++;
            if (level >= data.damages.Length)
            {
                GetComponent<Button>().interactable = false; // 버튼 비활성화
            }
        }

        private void HandleGearItem()
        {
            if (gear == null) // Gear가 없는 경우 새로 생성
            {
                GameObject newGear = new GameObject(data.itemName);
                gear = newGear.AddComponent<Gear>();
                gear.Init(data); // Gear 초기화
            }
            else
            {
                float nextRate = data.damages[level];
                gear.LevelUp(nextRate); // Gear 레벨업
            }

            Debug.Log($"[Item] {data.itemName} 사용. Level: {level + 1}");
        }

        private void HealPlayer()
        {
            // 플레이어 체력 회복 처리
            Debug.Log("플레이어 체력 회복!");
            GameManager.Instance.playerMovement.RestoreHealth();
        }
    }


