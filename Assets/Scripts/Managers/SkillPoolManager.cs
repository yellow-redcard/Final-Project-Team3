using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPoolManager : MonoBehaviour
{    
    public GameObject[] prefabs;

    List<GameObject>[] pools;
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
}
   