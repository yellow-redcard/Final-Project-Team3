using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; //유니티에서 몬스터를 생성할 위치

    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // 시간을 흐르게 만들어 줌

        if (timer > 0.2f)
        {
            Spawn();
            timer = 0f; // 시간 초기화
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.poolManager.Get(Random.Range(0, 2)); //enemy를 정의
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // enemy가 생성되는 위치를 유니티에서 만든 spawnpoint들에서 생성
    }
}
