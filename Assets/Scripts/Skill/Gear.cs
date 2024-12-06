using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Gear : MonoBehaviour
    {
        public ItemData.ItemType type; // 장비 타입 (Glove, Shoe)
        public float rate; // 효과 값

        public void Init(ItemData data)
        {
            // Gear 초기화
            name = "Gear " + data.itemId;
            type = data.itemType;
            rate = data.damages[0]; // 초기 효과 값 설정
            ApplyGear();
        }

        public void LevelUp(float nextRate)
        {
            rate = nextRate; // 새로운 효과 값 설정
            ApplyGear();
        }

        private void ApplyGear()
        {
            // 장비 종류에 따라 효과 적용
            switch (type)
            {
                case ItemData.ItemType.Glove:
                    ApplyRateUp(); // 발사 속도 증가
                    break;

                case ItemData.ItemType.Shoe:
                    ApplySpeedUp(); // 이동 속도 증가
                    break;
            }
        }

        private void ApplyRateUp()
        {
            // SkillManager를 통해 발사 속도 증가 적용
            if (GameManager.Instance.skillManager != null)
            {
                GameManager.Instance.skillManager.AdjustFireRate(rate);
                Debug.Log($"[Gear] 발사 속도 증가 적용! Rate: {rate}");
            }
        }

        private void ApplySpeedUp()
        {
            // PlayerMovement를 통해 이동 속도 증가 적용
            if (GameManager.Instance.playerMovement != null)
            {
                GameManager.Instance.playerMovement.AdjustSpeed(rate);
                Debug.Log($"[Gear] 이동 속도 증가 적용! Rate: {rate}");
            }
        }
    }
}
