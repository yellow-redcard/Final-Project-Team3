using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Gear : MonoBehaviour
    {
        public ItemData.ItemType type; // ��� Ÿ�� (Glove, Shoe)
        public float rate; // ȿ�� ��

        public void Init(ItemData data)
        {
            // Gear �ʱ�ȭ
            name = "Gear " + data.itemId;
            type = data.itemType;
            rate = data.damages[0]; // �ʱ� ȿ�� �� ����
            ApplyGear();
        }

        public void LevelUp(float nextRate)
        {
            rate = nextRate; // ���ο� ȿ�� �� ����
            ApplyGear();
        }

        private void ApplyGear()
        {
            // ��� ������ ���� ȿ�� ����
            switch (type)
            {
                case ItemData.ItemType.Glove:
                    ApplyRateUp(); // �߻� �ӵ� ����
                    break;

                case ItemData.ItemType.Shoe:
                    ApplySpeedUp(); // �̵� �ӵ� ����
                    break;
            }
        }

        private void ApplyRateUp()
        {
            // SkillManager�� ���� �߻� �ӵ� ���� ����
            if (GameManager.Instance.skillManager != null)
            {
                GameManager.Instance.skillManager.AdjustFireRate(rate);
                Debug.Log($"[Gear] �߻� �ӵ� ���� ����! Rate: {rate}");
            }
        }

        private void ApplySpeedUp()
        {
            // PlayerMovement�� ���� �̵� �ӵ� ���� ����
            if (GameManager.Instance.playerMovement != null)
            {
                GameManager.Instance.playerMovement.AdjustSpeed(rate);
                Debug.Log($"[Gear] �̵� �ӵ� ���� ����! Rate: {rate}");
            }
        }
    }
}
