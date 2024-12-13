using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; //����Ƽ���� ���͸� ������ ��ġ

    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // �ð��� �帣�� ����� ��

        if (timer > 0.2f)
        {
            Spawn();
            timer = 0f; // �ð� �ʱ�ȭ
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.poolManager.Get(Random.Range(0, 2)); //enemy�� ����
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // enemy�� �����Ǵ� ��ġ�� ����Ƽ���� ���� spawnpoint�鿡�� ����
    }
}
