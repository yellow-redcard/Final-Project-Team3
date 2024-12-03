using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; //����Ƽ���� ���͸� ������ ��ġ
    public SpawnData[] spawnData;


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
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            Spawn();
            timer = 0f; // �ð� �ʱ�ȭ
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.poolManager.Get(0); //enemy�� ����
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // enemy�� �����Ǵ� ��ġ�� ����Ƽ���� ���� spawnpoint�鿡�� ����
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}
