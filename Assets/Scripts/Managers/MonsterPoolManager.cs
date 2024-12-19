using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour, IManager
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;
    private int currentPrefabIndex = 0;

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index]) // ������ Ǯ�� ����ִ�(��Ȱ��ȭ��) ������Ʈ ����
        {
            if (!item.activeSelf) // �߰��ϸ�?
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select) // ��ã�Ҵٸ�?
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; // Ǯ�� �������� ���� �����ϰ�

        for (int index = 0; index < pools.Length; index++) // �迭 �ȿ� �ִ� ������ ����Ʈ�� �ʱ�ȭ
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject GetNextPrefab(int level)
    {
        // ���� �������� ����� �� �ִ� ������ �ε��� ���
        int startIndex = GetStartIndexForLevel(level);
        int endIndex = GetEndIndexForLevel(level);

        // ���� ���� �����տ��� ������� ��������
        int index = currentPrefabIndex;
        currentPrefabIndex = (currentPrefabIndex + 1) % (endIndex - startIndex + 1) + startIndex;

        return Get(index);
    }

    public void init()
    {

    }
    public void release()
    {

    }
    public void ReturnToPool(GameObject obj, int index)
    {
        obj.SetActive(false); // ��Ȱ��ȭ
        if (!pools[index].Contains(obj))
        {
            pools[index].Add(obj); // Ǯ�� ��ȯ
        }
    }
    public List<Transform> GetActiveMonsters()
    {
        List<Transform> activeMonsters = new List<Transform>();

        foreach (var pool in pools)
        {
            foreach (var monster in pool)
            {
                if (monster.activeSelf) // Ȱ��ȭ�� ���͸� ��������
                {
                    activeMonsters.Add(monster.transform);
                }
            }
        }

        return activeMonsters;
    }

    private int GetStartIndexForLevel(int level)
    {
        // ������ ���� ���� �ε��� ����
        return Mathf.Clamp(level * 3, 0, prefabs.Length - 1); // ��: ������ 2���� Ȱ��ȭ
    }

    private int GetEndIndexForLevel(int level)
    {
        // ������ ���� �� �ε��� ����
        return Mathf.Clamp(level * 3 + 2, 0, prefabs.Length - 1); // ��: ������ 2���� Ȱ��ȭ
    }
}

