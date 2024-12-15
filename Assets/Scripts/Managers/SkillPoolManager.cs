using System.Collections.Generic;
using UnityEngine;

public class SkillPoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private List<GameObject>[] pools;

    private void Awake()
    {
        // 초기화는 init()에서 수행
    }

    public void init()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                return select;
            }
        }

        select = Instantiate(prefabs[index], transform);
        pools[index].Add(select);
        return select;
    }

    public void ReturnToPool(GameObject obj, int index)
    {
        obj.SetActive(false);
        if (!pools[index].Contains(obj))
        {
            pools[index].Add(obj);
        }
    }

    public void release()
    {
        // 필요에 따라 구현
    }
}
