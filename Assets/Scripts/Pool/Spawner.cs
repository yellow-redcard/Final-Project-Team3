using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; // 스폰 포인트 배열

    private float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.2f)
        {
            Spawn();
            timer = 0f; // 타이머 초기화
        }
    }

    private void Spawn()
    {
        if (GameManager.Instance == null || GameManager.Instance.monsterPool == null)
        {
            Debug.LogError("Spawner: GameManager 또는 MonsterPoolManager가 설정되지 않았습니다.");
            return;
        }

        int randomIndex = Random.Range(0, 3);
        GameObject enemy = GameManager.Instance.monsterPool.Get(randomIndex);

        if (enemy != null)
        {
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        }
    }
}

