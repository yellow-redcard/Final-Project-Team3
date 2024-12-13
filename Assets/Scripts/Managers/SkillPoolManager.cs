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

        foreach (GameObject item in pools[index]) // 선택한 풀의 놀고있는(비활성화된) 오브젝트 접근
        {
            if (!item.activeSelf) // 발견하면?
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select) // 못찾았다면?
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; // 풀과 프리팹의 길이 동일하게

        for (int index = 0; index < pools.Length; index++) // 배열 안에 있는 각각의 리스트들 초기화
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
        obj.SetActive(false); // 비활성화
        if (!pools[index].Contains(obj))
        {
            pools[index].Add(obj); // 풀로 반환
        }
    }   
}
   