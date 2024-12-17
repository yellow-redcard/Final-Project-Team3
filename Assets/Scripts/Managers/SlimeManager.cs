using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour, IManager
{
    public List<GameObject> slimePrefabs;
    public GameObject currentSlime;
    public int currentIndex;

    public void init()
    {
        currentIndex = Random.Range(0, slimePrefabs.Count);
        if (slimePrefabs.Count > 0)
        {
            currentSlime = Instantiate(slimePrefabs[currentIndex], transform.position, Quaternion.identity);
        }
    }
    public void release()
    {
        Destroy(currentSlime);
    }
    public void CreateSlime()
    {
        currentSlime = Instantiate(slimePrefabs[currentIndex], transform.position, Quaternion.identity);
    }
}
