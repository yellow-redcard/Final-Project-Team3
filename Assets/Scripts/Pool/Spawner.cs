using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; // 소환 위치
    private float timer;
    private float bossTimer;
    private float mimicTimer;
    private float finalBossTimer;

    private int level;
    private float levelTimer;

    [SerializeField] private int maxNormalAndMimicCount = 30;
    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        bossTimer += Time.deltaTime; // 보스 타이머
        mimicTimer += Time.deltaTime; // Mimic 타이머
        finalBossTimer += Time.deltaTime;
        // 레벨 증가 타이머
        levelTimer += Time.deltaTime;
        // 30초마다 레벨 증가
        if (levelTimer >= 25f)
        {
            level++; // 레벨 증가
            levelTimer = 0f; // 타이머 초기화
            Debug.Log($"Level Up! Current Level: {level}");
        }
        // 일반 몬스터 소환 25초 주기 소환
        if (timer > 1f)
        {
            for (int i = 0; i < level; i++)
            {
                GameManager.Instance.monsterPool.DestroyInactiveMonstersOfLevel(i);
            }
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
        if (finalBossTimer > 30 * 60f)
        {
            SpawnFinalBoss();
            finalBossTimer = 0f;
        }
    }
    private void SpawnMonsters(int level)
    {
        MonsterPoolManager poolManager = GameManager.Instance.monsterPool;
        // 현재 활성화되어 있는 일반+미믹 몬스터 총 수
        int currentCount = poolManager.GetActiveNormalAndMimicCount();
        // 이번에 소환하고 싶은 몬스터 수 (예: 3마리)
        int spawnCount = 3;
        // 만약 현재 카운트 + 3마리가 최대 한도를 넘는다면
        // 넘치지 않는 선에서만 스폰하도록 조절
        if (currentCount + spawnCount > maxNormalAndMimicCount)
        {
            spawnCount = maxNormalAndMimicCount - currentCount;
        }
        // spawnCount가 0 이하라면 스폰하지 않음
        if (spawnCount <= 0)
        {
            return;
        }
        // 실제 스폰
        for (int i = 0; i < spawnCount; i++)
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
        int currentCount = poolManager.GetActiveNormalAndMimicCount();
        
        GameObject mimic = poolManager.GetNextMimicPrefab();
        mimic.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
    public void SpawnFinalBoss()
    {
        MonsterPoolManager poolManager = GameManager.Instance.monsterPool;
        GameObject finalBoss = poolManager.GetNextFinalBossPrefab();
        finalBoss.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}
