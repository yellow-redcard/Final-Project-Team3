using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; //유니티에서 몬스터를 생성할 위치

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // 시간을 흐르게 만들어 줌
        level = Mathf.FloorToInt(GameManager.Instance.gameTime / 20f);

        if (timer > 1f)
        {
            Spawn();
            timer = 0f; // 시간 초기화
        }
    }

    void Spawn()
    {
        MonsterPoolManager poolManager = GameManager.Instance.monsterPool;

        // 3개의 몬스터를 레벨에 따라 소환
        for (int i = 0; i < 3; i++)
        {
            GameObject enemy = poolManager.GetNextPrefab(level);
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        }
    }
}
