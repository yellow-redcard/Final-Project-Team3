using UnityEngine;
using UnityEngine.UI;

public class TagSlimeUI : MonoBehaviour
{
    public int slimeIndex;

    void Start()
    {
        // SlimeManager ã��
        GameManager.Instance.slimeManager = FindObjectOfType<SlimeManager>();
        GetComponent<Button>().onClick.AddListener(OnTag1);
    }

    void OnTag1()
    {
        GameManager.Instance.slimeManager.TagSlime(slimeIndex);
    }
}

