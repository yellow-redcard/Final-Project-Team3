using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; // ���� ����Ʈ �迭

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
            timer = 0f; // Ÿ�̸� �ʱ�ȭ
        }
    }

    private void Spawn()
    {
        if (GameManager.Instance == null || GameManager.Instance.monsterPool == null)
        {
            Debug.LogError("Spawner: GameManager �Ǵ� MonsterPoolManager�� �������� �ʾҽ��ϴ�.");
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

