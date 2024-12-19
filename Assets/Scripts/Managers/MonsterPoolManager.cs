using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour, IManager
{
    public GameObject[] prefabs;
    public GameObject[] bossPrefabs;

    List<GameObject>[] bossPools;
    List<GameObject>[] pools;
    private int currentPrefabIndex = 0;
    private int currentBossIndex = 0;

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

    public GameObject GetBoss(int index)
    {
        GameObject select = null;

        // 비활성화된 보스 몬스터 찾기
        foreach (GameObject item in bossPools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = Instantiate(bossPrefabs[index], transform);
            bossPools[index].Add(select);
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

        bossPools = new List<GameObject>[bossPrefabs.Length];
        for (int i = 0; i < bossPrefabs.Length; i++)
        {
            bossPools[i] = new List<GameObject>(); // 각 보스 풀 리스트 생성
        }
    }

    public GameObject GetNextPrefab(int level)
    {
        // 현재 레벨에서 사용할 수 있는 프리팹 인덱스 계산
        int startIndex = GetStartIndexForLevel(level);
        int endIndex = GetEndIndexForLevel(level);

        // 범위 내의 프리팹에서 순서대로 가져오기
        int index = currentPrefabIndex;
        currentPrefabIndex = (currentPrefabIndex + 1) % (endIndex - startIndex + 1) + startIndex;

        return Get(index);
    }

    public GameObject GetNextBossPrefab()
    {
        // 보스 몬스터 순서대로 반환
        int index = currentBossIndex;
        currentBossIndex = (currentBossIndex + 1) % bossPrefabs.Length;
        return GetBoss(index);
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
    public List<Transform> GetActiveMonsters()
    {
        List<Transform> activeMonsters = new List<Transform>();

        foreach (var pool in pools)
        {
            foreach (var monster in pool)
            {
                if (monster.activeSelf) // 활성화된 몬스터만 가져오기
                {
                    activeMonsters.Add(monster.transform);
                }
            }
        }

        return activeMonsters;
    }

    private int GetStartIndexForLevel(int level)
    {
        // 레벨에 따른 시작 인덱스 정의
        return Mathf.Clamp(level * 3, 0, prefabs.Length - 1); // 예: 레벨당 2개씩 활성화
    }

    private int GetEndIndexForLevel(int level)
    {
        // 레벨에 따른 끝 인덱스 정의
        return Mathf.Clamp(level * 3 + 2, 0, prefabs.Length - 1); // 예: 레벨당 2개씩 활성화
    }
}

