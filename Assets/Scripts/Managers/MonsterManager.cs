using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour, IManager
{
    public MonsterPoolManager monsterPool; // ���� Ǯ �Ŵ���
    public Transform spawnArea; // ���� ���� ����
    public int maxMonsters = 10; // ���ÿ� �����Ǵ� �ִ� ���� ��
    public float spawnInterval = 2f; // ���� ����

    private float spawnTimer;
    private void Start()
    {
        if (monsterPool == null)
        {
            Debug.LogError("MonsterManager: MonsterPoolManager�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        monsterPool.init();
    }
    public void init()
    {
        spawnTimer = spawnInterval;
    }

    public void release()
    {
        // �ʿ�� ���ҽ� ����
    }

    private void Update()
    {
        if (monsterPool == null)
        {
            Debug.LogError("MonsterManager: MonsterPoolManager�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // ���� ���� ����
    }

    // ���� ���� �޼���
    private void SpawnMonster()
    {
        int randomIndex = Random.Range(0, monsterPool.prefabs.Length); // ������ ���� Ÿ�� ����
        Vector3 spawnPosition = GetRandomPositionInArea(); // ���� �������� ���� ��ġ ��������
        GameObject monster = monsterPool.Get(randomIndex);
        monster.transform.position = spawnPosition;

        // ���� �ʱ�ȭ (�ʿ� �� ��ũ��Ʈ ȣ��)
        Monster monsterScript = monster.GetComponent<Monster>();
        if (monsterScript != null)
        {
            monsterScript.Initialize(); // ���� �ʱ�ȭ �޼��� ȣ��
        }
    }

    // ���� ���� �� ���� ��ġ ��������
    private Vector3 GetRandomPositionInArea()
    {
        Vector3 areaSize = spawnArea.localScale;
        Vector3 randomPosition = new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            Random.Range(-areaSize.y / 2, areaSize.y / 2),
            0f
        );
        return spawnArea.position + randomPosition;
    }

    // ��� Ȱ��ȭ�� ���� ����
    public void ClearAllMonsters()
    {
        List<Transform> activeMonsters = monsterPool.GetActiveMonsters();
        foreach (Transform monster in activeMonsters)
        {
            monster.gameObject.SetActive(false);
        }
    }
}
