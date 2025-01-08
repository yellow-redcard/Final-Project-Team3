using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SlimeManager : MonoBehaviour, IManager
{
    public GameObject slime;
    public List<GameObject> slimeBodies;
    public GameObject currentSlime;
    public int currentIndex;
    private List<GameObject> inactiveSlimes = new List<GameObject>();
    public List<SlimeData> slimeDatas;

    // 슬라임 이름에 따라 속성을 매핑합니다.
    private Dictionary<string, ElementType> slimeBodyToElementMap = new Dictionary<string, ElementType>
{
    { "DarkSlimeBody", ElementType.Dark },
    { "ElectricSlimeBody", ElementType.Electricity },
    { "FlameSlimeBody", ElementType.Flame },
    { "WaterSlimeBody", ElementType.Water }
};


    public void init()
    {
        currentSlime = Instantiate(slime, new Vector3(), Quaternion.identity);
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
        InitializeSlime(); // 슬라임 초기화
    }
    public void release()
    {
        Destroy(currentSlime);
    }

    public void ChangeSlime(Vector2 position)
    {
        // 현재 슬라임 위치 및 상태 변경
        currentSlime.transform.position = position;

        // 기존 슬라임 비활성화
        foreach (var body in slimeBodies)
        {
            body.SetActive(false);
        }

        // 새 슬라임 활성화
        slimeBodies[currentIndex].SetActive(true);

        // 활성화된 슬라임 바디 기반으로 속성 설정
        SetElementBySlime();
    }

    void AddSlimeBody(Transform parent, string bodyName)
    {
        Transform bodyTransform = parent.Find(bodyName);
        if (bodyTransform != null)
        {
            bodyTransform.name = bodyName; // 이름을 명확히 설정
            slimeBodies.Add(bodyTransform.gameObject);
        }
        else
        {
            Debug.LogWarning($"{bodyName} 오브젝트를 찾을 수 없습니다.");
        }
    }
    private void InitializeSlime()
    {
        // 처음 활성화된 슬라임 바디 찾기
        GameObject activeBody = slimeBodies[currentIndex];
        if (activeBody == null)
        {
            Debug.LogWarning("[InitializeSlime] 활성화된 슬라임 바디가 없습니다.");
            GameManager.Instance.skillManager.SetCurrentElement(ElementType.None);
            return;
        }

        // 슬라임 속성 설정
        SetElementBySlime();
        Debug.Log($"[InitializeSlime] 초기 슬라임 속성 설정: {GameManager.Instance.skillManager.currentElement}");
    }
    private void SetElementBySlime()
    {
        GameObject activeBody = slimeBodies[currentIndex];
        if (activeBody == null)
        {
            Debug.LogWarning("[SetElementBySlime] 활성화된 슬라임 바디가 없습니다.");
            GameManager.Instance.skillManager.SetCurrentElement(ElementType.None);
            return;
        }

        string bodyName = activeBody.name.Replace("(Clone)", "").Trim();

        if (slimeBodyToElementMap.TryGetValue(bodyName, out ElementType element))
        {
            GameManager.Instance.skillManager.SetCurrentElement(element);
            Debug.Log($"[SlimeManager] 슬라임 속성 설정: {element}");
        }
        else
        {
            Debug.LogWarning($"[SlimeManager] '{bodyName}'에 해당하는 속성을 찾을 수 없습니다.");
            GameManager.Instance.skillManager.SetCurrentElement(ElementType.None);
        }
    }
}