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
        level = Mathf.FloorToInt(GameManager.Instance.gameTime / 20f);

        if (timer > 1f)
        {
            Spawn();
            timer = 0f; // �ð� �ʱ�ȭ
        }
    }

    void Spawn()
    {
        MonsterPoolManager poolManager = GameManager.Instance.monsterPool;

        // 3���� ���͸� ������ ���� ��ȯ
        for (int i = 0; i < 3; i++)
        {
            GameObject enemy = poolManager.GetNextPrefab(level);
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        }
    }
}
