using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour, IManager
{
    public GameObject[] prefabs; // Resources에서 자동으로 채워짐
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
        // Resources/Prefabs/Monster 폴더에서 모든 프리팹 로드
        prefabs = Resources.LoadAll<GameObject>("Prefabs/Monster");

        if (prefabs.Length > 0)
        {
            Debug.Log($"총 {prefabs.Length}개의 몬스터 프리팹이 Resources에서 로드되었습니다.");
        }
        else
        {
            Debug.LogError("MonsterPoolManager: Prefabs/Monster 폴더에 프리팹이 없습니다!");
        }
    }

    public GameObject Get(int index)
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError("MonsterPoolManager: 프리팹 배열이 비어 있습니다.");
            return null;
        }

        if (index < 0 || index >= prefabs.Length)
        {
            Debug.LogError($"MonsterPoolManager: 잘못된 인덱스 {index}");
            return null;
        }

        GameObject select = null;

        // 비활성화된 오브젝트 찾기
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                return select;
            }
        }

        // 새 오브젝트 생성
        select = Instantiate(prefabs[index], transform);
        pools[index].Add(select);
        return select;
    }

    public void release()
    {
        // 필요시 구현
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
