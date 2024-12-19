using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; //유니티에서 몬스터를 생성할 위치

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // 시간을 흐르게 만들어 줌
       
        level = Mathf.FloorToInt(GameManager.Instance.gameTime / 20f);  //레벨 계산: 게임 시간을 기준으로 20초마다 레벨 증가

        if (timer > 0.2f) //타이머가 0.2초 이상일 때 스폰 실행 
        {
            Spawn();
            timer = 0f; // 시간 초기화
        }
    }

    void Spawn()
    {    
        if (GameManager.Instance == null || GameManager.Instance.monsterPool == null) // GameManager와 MonsterPoolManager가 올바르게 설정되었는지 확인 한마디로 방어코드
        {
            Debug.LogError("Spawner: GameManager 또는 MonsterPoolManager가 설정되지 않았습니다.");
            return;
        }
        MonsterPoolManager poolManager = GameManager.Instance.monsterPool;

        GameObject enemy = poolManager.GetNextPrefab(level);  // 레벨 기반으로 적 프리팹 가져오기


        if (enemy != null)
        {
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // 랜덤한 스폰 포인트에 배치
        }
        else
        {
            Debug.LogError($"Spawner: 레벨 {level}에 대한 몬스터 프리팹이 없습니다.");
        }
    }
}
