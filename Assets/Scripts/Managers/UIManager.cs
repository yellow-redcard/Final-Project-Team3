using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static SkillManager;

public class UIManager : MonoBehaviour, IManager
{
    [SerializeField] private LevelUpUI levelUpUIPrefab;
    private LevelUpUI levelUpUIInstance; // 동적 생성된 LevelUpUI 인스턴스
    [SerializeField] private Transform canvas;
    public static float ScreenWidth = 1920;
    public static float ScreenHeight = 1080;
    private List<UIBase> uiList = new List<UIBase>();
    public void init()
    {
        GameObject uiPrefab = Resources.Load<GameObject>("UI/LevelUpUI");
        if (uiPrefab != null)
        {
            levelUpUIInstance = Instantiate(uiPrefab).GetComponent<LevelUpUI>();
            levelUpUIInstance.gameObject.SetActive(false); // 처음에는 비활성화

            // LevelUpUI 캔버스 생성
            GameObject levelUpUICanvas = new GameObject("LevelUpUI");
            Canvas canvas = levelUpUICanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // GraphicRaycaster 추가
            GraphicRaycaster graphicRaycaster = levelUpUICanvas.AddComponent<GraphicRaycaster>();

            //캔버스 화면 중앙
            CanvasScaler canvasScaler = levelUpUICanvas.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920f, 1080f); // 기준 해상도 설정
            canvasScaler.matchWidthOrHeight = 0.5f; // 화면 비율 유지
            levelUpUIInstance.transform.SetParent(levelUpUICanvas.transform);
            levelUpUICanvas.layer = LayerMask.NameToLayer("UI");
            canvas.sortingOrder = 2;

        }
        else
        {
            Debug.LogError("[UIManager] LevelUpUI 프리팹을 찾을 수 없습니다!");
        }
    }

    public void release()
    {
        Debug.Log("[UIManager] 해제 완료");
    }

    public T Show<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).ToString();

        // 기존 UI가 이미 로드되었는지 확인
        T uiInstance = (T)uiList.Find(ui => ui.GetType() == typeof(T));
        if (uiInstance != null)
        {
            uiInstance.Opened(param); // 이미 생성된 경우 Opened 호출
            uiInstance.gameObject.SetActive(true);
            return uiInstance;
        }

        // 새로 로드
        UIBase prefab = Resources.Load<UIBase>("UI/" + uiName);
        if (prefab == null)
        {
            Debug.LogError($"[UIManager] Resources/UI/{uiName}을(를) 찾을 수 없습니다.");
            return null;
        }

        var ui = Load<T>(prefab, uiName);
        uiList.Add(ui);
        ui.Opened(param);

        return ui;
    }

    private T Load<T>(UIBase prefab, string uiName) where T : UIBase
    {
        GameObject newCanvasObject = new GameObject(uiName + " Canvas");

        var canvas = newCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(ScreenWidth, ScreenHeight);

        newCanvasObject.AddComponent<GraphicRaycaster>();

        UIBase ui = Instantiate(prefab, newCanvasObject.transform);
        ui.name = ui.name.Replace("(Clone)", "");
        ui.canvas = canvas;
        ui.canvas.sortingOrder = uiList.Count;

        return (T)ui;
    }
    public void Hide<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString();
        Hide(uiName);
    }

    public void Hide(string uiName)
    {
        UIBase go = uiList.Find(obj => obj.name == uiName);
        uiList.Remove(go);
        Destroy(go.canvas.gameObject);
    }
    public void CloseUI()
    {
        foreach (UIBase ui in uiList)
        {
            Destroy(ui.canvas.gameObject);
        }
        uiList.Clear();
    }
    public void ShowLevelUpUI(List<SkillData> upgradeableSkills)
    {
        if (levelUpUIInstance == null)
        {
            Debug.LogError("[UIManager] LevelUpUI 인스턴스가 생성되지 않았습니다!");
            return;
        }

        if (upgradeableSkills == null || upgradeableSkills.Count == 0)
        {
            Debug.LogError("[UIManager] 업그레이드 가능한 스킬이 없습니다.");
            return;
        }

        levelUpUIInstance.ConfigureButtons(upgradeableSkills); // 버튼 데이터 설정
        levelUpUIInstance.gameObject.SetActive(true); // UI 활성화
        Debug.Log($"[UIManager] LevelUpUI 활성화. 버튼 개수: {upgradeableSkills.Count}"); 
        if (levelUpUIInstance == null)
        {
            Debug.LogError("[UIManager] LevelUpUI 프리팹이 로드되지 않았습니다!");
        }
        Debug.Log($"LevelUpUI 활성화 상태: {levelUpUIInstance.gameObject.activeSelf}");
    }
}