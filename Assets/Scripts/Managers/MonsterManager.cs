using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour, IManager
{
    public MonsterPoolManager monsterPool; // 몬스터 풀 매니저
    public Transform spawnArea; // 몬스터 스폰 영역
    public int maxMonsters = 10; // 동시에 스폰되는 최대 몬스터 수
    public float spawnInterval = 2f; // 스폰 간격

    private float spawnTimer;
    private void Start()
    {
        if (monsterPool == null)
        {
            Debug.LogError("MonsterManager: MonsterPoolManager가 할당되지 않았습니다.");
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
        // 필요시 리소스 정리
    }

    private void Update()
    {
        if (monsterPool == null)
        {
            Debug.LogError("MonsterManager: MonsterPoolManager가 할당되지 않았습니다.");
            return;
        }

        // 몬스터 스폰 로직
    }

    // 몬스터 스폰 메서드
    private void SpawnMonster()
    {
        int randomIndex = Random.Range(0, monsterPool.prefabs.Length); // 랜덤한 몬스터 타입 선택
        Vector3 spawnPosition = GetRandomPositionInArea(); // 스폰 영역에서 랜덤 위치 가져오기
        GameObject monster = monsterPool.Get(randomIndex);
        monster.transform.position = spawnPosition;

        // 몬스터 초기화 (필요 시 스크립트 호출)
        Monster monsterScript = monster.GetComponent<Monster>();
        if (monsterScript != null)
        {
            monsterScript.Initialize(); // 몬스터 초기화 메서드 호출
        }
    }

    // 스폰 영역 내 랜덤 위치 가져오기
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

    // 모든 활성화된 몬스터 제거
    public void ClearAllMonsters()
    {
        List<Transform> activeMonsters = monsterPool.GetActiveMonsters();
        foreach (Transform monster in activeMonsters)
        {
            monster.gameObject.SetActive(false);
        }
    }
}
