using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; //����Ƽ���� ���͸� ������ ��ġ

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // �ð��� �帣�� ����� ��
       
        level = Mathf.FloorToInt(GameManager.Instance.gameTime / 20f);  //���� ���: ���� �ð��� �������� 20�ʸ��� ���� ����

        if (timer > 0.2f) //Ÿ�̸Ӱ� 0.2�� �̻��� �� ���� ���� 
        {
            Spawn();
            timer = 0f; // �ð� �ʱ�ȭ
        }
    }

    void Spawn()
    {    
        if (GameManager.Instance == null || GameManager.Instance.monsterPool == null) // GameManager�� MonsterPoolManager�� �ùٸ��� �����Ǿ����� Ȯ�� �Ѹ���� ����ڵ�
        {
            Debug.LogError("Spawner: GameManager �Ǵ� MonsterPoolManager�� �������� �ʾҽ��ϴ�.");
            return;
        }
        MonsterPoolManager poolManager = GameManager.Instance.monsterPool;

        GameObject enemy = poolManager.GetNextPrefab(level);  // ���� ������� �� ������ ��������


        if (enemy != null)
        {
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // ������ ���� ����Ʈ�� ��ġ
        }
        else
        {
            Debug.LogError($"Spawner: ���� {level}�� ���� ���� �������� �����ϴ�.");
        }
    }
}
