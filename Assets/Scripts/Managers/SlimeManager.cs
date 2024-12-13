using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour, IManager
{
    public List<GameObject> slimePrefabs;
    public GameObject currentSlime;
    private int currentIndex = 0;

    void Start()
    {
        init();
    }

    public void init()
    {
        if (slimePrefabs.Count > 0)
        {
            currentSlime = Instantiate(slimePrefabs[currentIndex], transform.position, Quaternion.identity);
        }
    }
    public void release()
    {

    }

    public void TagSlime(int index)
    {
        // 현재 슬라임 제거
        Destroy(currentSlime);

        // 새로운 슬라임 인덱스 설정
        currentIndex = index;

        // 새로운 슬라임 생성
        currentSlime = Instantiate(slimePrefabs[currentIndex], transform.position, Quaternion.identity);
    }
}
