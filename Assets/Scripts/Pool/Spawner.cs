using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; //����Ƽ���� ���͸� ������ ��ġ

    int level;
    float timer;
    float bossTimer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // �ð��� �帣�� ����� ��
        bossTimer += Time.deltaTime;

        level = Mathf.FloorToInt(GameManager.Instance.gameTime / 20f);

        if (timer > 1f)
        {
            SpawnMonsters();
            timer = 0f; // �ð� �ʱ�ȭ
        }
        if (bossTimer > 20f)
        {
            SpawnBossMonster();
            bossTimer = 0f;
        }
    }

    private void SpawnBossMonster()
    {
        MonsterPoolManager poolManager = GameManager.Instance.monsterPool;

        // ���� ���� ��ȯ
        GameObject boss = poolManager.GetNextBossPrefab();
        boss.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }

    void SpawnMonsters()
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
