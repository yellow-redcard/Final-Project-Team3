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
        // ���� ������ ����
        Destroy(currentSlime);

        // ���ο� ������ �ε��� ����
        currentIndex = index;

        // ���ο� ������ ����
        currentSlime = Instantiate(slimePrefabs[currentIndex], transform.position, Quaternion.identity);
    }
}
