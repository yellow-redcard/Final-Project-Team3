using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour, IManager
{
    public GameObject[] prefabs; // Resources���� �ڵ����� ä����
    private List<GameObject>[] pools;

    public void init()
    {
        LoadMonsterPrefabs();

        pools = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    private void LoadMonsterPrefabs()
    {
        // Resources/Prefabs/Monster �������� ��� ������ �ε�
        prefabs = Resources.LoadAll<GameObject>("Prefabs/Monster");

        if (prefabs.Length > 0)
        {
            Debug.Log($"�� {prefabs.Length}���� ���� �������� Resources���� �ε�Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("MonsterPoolManager: Prefabs/Monster ������ �������� �����ϴ�!");
        }
    }

    public GameObject Get(int index)
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError("MonsterPoolManager: ������ �迭�� ��� �ֽ��ϴ�.");
            return null;
        }

        if (index < 0 || index >= prefabs.Length)
        {
            Debug.LogError($"MonsterPoolManager: �߸��� �ε��� {index}");
            return null;
        }

        GameObject select = null;

        // ��Ȱ��ȭ�� ������Ʈ ã��
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                return select;
            }
        }

        // �� ������Ʈ ����
        select = Instantiate(prefabs[index], transform);
        pools[index].Add(select);
        return select;
    }

    public void release()
    {
        // �ʿ�� ����
    }

    public List<Transform> GetActiveMonsters()
    {
        List<Transform> activeMonsters = new List<Transform>();

        foreach (var pool in pools)
        {
            foreach (var monster in pool)
            {
                if (monster.activeSelf) activeMonsters.Add(monster.transform);
            }
        }

        return activeMonsters;
    }

    public void ReturnToPool(GameObject obj, int index)
    {
        obj.SetActive(false);
        if (!pools[index].Contains(obj))
        {
            pools[index].Add(obj);
        }
    }
}
