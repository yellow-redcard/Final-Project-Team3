using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; //유니티에서 몬스터를 생성할 위치
    public SpawnData[] spawnData;


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
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gameTime / 10f), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            Spawn();
            timer = 0f; // 시간 초기화
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.Instance.poolManager.Get(0); //enemy를 정의
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // enemy가 생성되는 위치를 유니티에서 만든 spawnpoint들에서 생성
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}
