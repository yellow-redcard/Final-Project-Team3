using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeManager : MonoBehaviour, IManager
{
    [SerializeField] private SlimeData slimeData;
    public GameObject slime;
    public List<GameObject> slimeBodies;
    public GameObject currentSlime;
    public int currentIndex;
    private List<GameObject> inactiveSlimes = new List<GameObject>();

    // 슬라임 이름에 따라 속성을 매핑합니다.
    private Dictionary<string, ElementType> slimeToElementMap = new Dictionary<string, ElementType>
    {
        { "DarkSlime", ElementType.Dark },
        { "FireSlime", ElementType.Flame },
        { "WaterSlime", ElementType.Water },
        { "ElectricSlime", ElementType.Electricity }
    };

    public void init()
    {
        currentSlime = Instantiate(slime, new Vector3() , Quaternion.identity);
        Transform slimeBodiesTransform = currentSlime.transform.Find("SlimeBodies");
        if (slimeBodiesTransform != null)
        {
            // 각각의 SlimeBody 타입 오브젝트를 리스트에 추가합니다.
            AddSlimeBody(slimeBodiesTransform, "DarkSlimeBody");
            AddSlimeBody(slimeBodiesTransform, "ElectricSlimeBody");
            AddSlimeBody(slimeBodiesTransform, "FlameSlimeBody");
            AddSlimeBody(slimeBodiesTransform, "WaterSlimeBody");
        }
        currentIndex = Random.Range(0, slimeBodies.Count);
        slimeBodies[currentIndex].SetActive(true);
    }
    public void release()
    {
        Destroy(currentSlime);
    }

    public void ChangeSlime(Vector2 position)
    {
        slimeBodies[currentIndex].SetActive(true); 
        

        // 생성된 슬라임 이름으로 속성을 설정
        if (currentSlime != null)
        {
            SetElementBySlime(currentSlime.name);
        }
    }
    void AddSlimeBody(Transform parent, string bodyName)
    {
        Transform bodyTransform = parent.Find(bodyName);
        if (bodyTransform != null)
        {
            slimeBodies.Add(bodyTransform.gameObject);
        }
        else
        {
            Debug.LogWarning($"{bodyName} 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void SetElementBySlime(string slimeName)
    {
        // 이름에서 "(Clone)" 제거
        string cleanName = slimeName.Replace("(Clone)", "").Trim();

        if (slimeToElementMap.TryGetValue(cleanName, out ElementType element))
        {
            // SkillManager의 현재 속성을 설정
            GameManager.Instance.skillManager.SetCurrentElement(element);
            Debug.Log($"슬라임 '{cleanName}'에 따라 속성을 '{element}'로 설정했습니다.");
        }
        else
        {
            Debug.LogWarning($"'{cleanName}'에 해당하는 속성을 찾을 수 없습니다.");
        }
    }
}