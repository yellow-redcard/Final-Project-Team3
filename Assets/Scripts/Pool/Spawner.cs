using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; // 소환 위치
    private float timer;
    private float bossTimer;
    private float mimicTimer;

    private int level;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        bossTimer += Time.deltaTime; // 보스 타이머
        mimicTimer += Time.deltaTime; // Mimic 타이머

        level = Mathf.FloorToInt(GameManager.Instance.gameTime / 30f); // 레벨 계산

        // 일반 몬스터 소환 30초 주기 소환
        if (timer > 1f)
        {
            SpawnMonsters(level);
            timer = 0f;
        }

        // 보스 몬스터 1분(60초) 주기 소환
        if (bossTimer > 60f)
        {
            SpawnBoss();
            bossTimer = 0f;
        }

        // Mimic 몬스터 30초 주기 소환
        if (mimicTimer > 30f)
        {
            SpawnMimic();
            mimicTimer = 0f;
        }
    }

    private void SpawnMonsters(int level)
    {
        MonsterPoolManager poolManager = GameManager.Instance.monsterPool;

        for (int i = 0; i < 3; i++)
        {
            GameObject enemy = poolManager.GetNextPrefab(level);
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        }
    }

    private void SpawnBoss()
    {
        MonsterPoolManager poolManager = GameManager.Instance.monsterPool;

        GameObject boss = poolManager.GetNextBossPrefab();
        boss.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }

    private void SpawnMimic()
    {
        MonsterPoolManager poolManager = GameManager.Instance.monsterPool;

        GameObject mimic = poolManager.GetNextMimicPrefab();
        mimic.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}
