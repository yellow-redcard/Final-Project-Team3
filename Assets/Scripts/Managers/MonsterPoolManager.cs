using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour, IManager
{
    public GameObject[] prefabs;
    public GameObject[] bossPrefabs;
    public GameObject[] mimicPrefabs;
    public GameObject[] finalBossPrefabs;

    List<GameObject>[] bossPools;
    List<GameObject>[] pools;
    List<GameObject>[] mimicPools;
    List<GameObject>[] finalBossPools;

    private int currentPrefabIndex = 0;
    private int currentBossIndex = 0;
    private int currentMimicIndex = 0;
    private int currentFinalBossIndex = 0;

    public GameObject Get(int index)
    {
        GameObject select = null;
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                break;
            }
        }
        if (select == null)
        {
            select = Instantiate(prefabs[index], transform);
            // Prefab 이름과 일치시키기
            select.name = prefabs[index].name;
            pools[index].Add(select);
        }
        // 활성화
        select.SetActive(true);
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
    public GameObject GetMimic(int index)
    {
        GameObject select = null;
        // 비활성화된 보스 몬스터 찾기
        foreach (GameObject item in mimicPools[index])
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
            select = Instantiate(mimicPrefabs[index], transform);
            mimicPools[index].Add(select);
        }
        return select;
    }
    public GameObject GetFinalBoss(int index)
    {
        GameObject select = null;
        // 비활성화된 보스 몬스터 찾기
        foreach (GameObject item in finalBossPools[index])
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
            select = Instantiate(finalBossPrefabs[index], transform);
            finalBossPools[index].Add(select);
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
        for (int i = 0; i < bossPools.Length; i++)
        {
            bossPools[i] = new List<GameObject>(); // 각 보스 풀 리스트 생성
        }
        mimicPools = new List<GameObject>[mimicPrefabs.Length];
        for (int i = 0; i < mimicPools.Length; i++)
        {
            mimicPools[i] = new List<GameObject>();
        }
        finalBossPools = new List<GameObject>[finalBossPrefabs.Length];
        for (int i = 0; i < finalBossPools.Length; i++)
        {
            finalBossPools[i] = new List<GameObject>();
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
    public GameObject GetNextMimicPrefab()
    {
        int index = currentMimicIndex;
        currentMimicIndex = (currentMimicIndex + 1) % mimicPrefabs.Length;
        return GetMimic(index);
    }
    public GameObject GetNextFinalBossPrefab()
    {
        int index = currentFinalBossIndex;
        currentFinalBossIndex = (currentFinalBossIndex + 1) % finalBossPrefabs.Length;
        return GetFinalBoss(index);
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
                if (monster != null && monster.activeSelf)
                {
                    activeMonsters.Add(monster.transform);
                }
            }
        }
        foreach (var bossPool in bossPools)
        {
            foreach (var boss in bossPool)
            {
                if (boss != null && boss.activeSelf)
                {
                    activeMonsters.Add(boss.transform);
                }
            }
        }
        foreach (var mimicPool in mimicPools)
        {
            foreach (var mimic in mimicPool)
            {
                if (mimic != null && mimic.activeSelf)
                {
                    activeMonsters.Add(mimic.transform);
                }
            }
        }
        foreach (var finalBossPool in finalBossPools)
        {
            foreach (var finalBoss in finalBossPool)
            {
                if (finalBoss != null && finalBoss.activeSelf)
                {
                    activeMonsters.Add(finalBoss.transform);
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
        return Mathf.Clamp((level + 1) * 3 - 1, 0, prefabs.Length - 1); // 예: 레벨당 2개씩 활성화
    }
    public int GetActiveNormalAndMimicCount()
    {
        int count = 0;
        // 1) 일반 몬스터(prefabs) 풀에서 활성화된 몬스터 수
        foreach (var pool in pools)
        {
            foreach (var monster in pool)
            {
                if (monster != null && monster.activeSelf)
                {
                    count++;
                }
            }
        }
        // 2) 미믹 몬스터(mimicPrefabs) 풀에서 활성화된 몬스터 수
        foreach (var mimicPool in mimicPools)
        {
            foreach (var mimic in mimicPool)
            {
                if (mimic != null && mimic.activeSelf)
                {
                    count++;
                }
            }
        }
        return count;
    }
    public void DestroyInactiveMonstersOfLevel(int level)
    {
        // 일반 몬스터 풀 체크
        for (int i = 0; i < pools.Length; i++)
        {
            for (int j = 0; j < pools[i].Count; j++)
            {
                GameObject monsterObj = pools[i][j];
                if (monsterObj != null && !monsterObj.activeSelf) // 비활성화 상태
                {
                    Monster monsterScript = monsterObj.GetComponent<Monster>();
                    if (monsterScript != null && monsterScript.spawnedLevel == level)
                    {
                        Destroy(monsterObj);
                        pools[i].RemoveAt(j);
                        j--;
                    }
                }
            }
        }
    }
}
