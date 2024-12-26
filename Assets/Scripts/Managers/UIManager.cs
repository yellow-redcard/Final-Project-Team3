using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static SkillManager;

public class UIManager : MonoBehaviour, IManager
{
    private LevelUpUI levelUpUI;
    [SerializeField] private Transform canvas;
    public static float ScreenWidth = 1920;
    public static float ScreenHeight = 1080;
    private List<UIBase> uiList = new List<UIBase>();
    public void init()
    {

    }

    public void release()
    {

    }

    public T Show<T>(params object[] param) where T : UIBase
    {
        string uiName = typeof(T).ToString();
        UIBase go = Resources.Load<UIBase>("UI/" + uiName);
        var ui = Load<T>(go, uiName);
        uiList.Add(ui);
        ui.Opened(param);

        return (T)ui;
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
    public void ShowLevelUpUI(List<SkillData> skillOptions)
    {
        var levelUpUI = FindObjectOfType<LevelUpUI>();

        if (levelUpUI != null)
        {
            Debug.Log("[UIManager] 레벨업 UI에 전달할 데이터:");
            foreach (var skill in skillOptions)
            {
                Debug.Log($"[UIManager] 스킬 이름: {skill.skillName}, 타입: {skill.skillType}, 속성: {skill.element}");
            }

            levelUpUI.ConfigureButtons(
                skillOptions.Count > 0 ? skillOptions[0] : null,
                skillOptions.Count > 1 ? skillOptions[1] : null,
                skillOptions.Count > 2 ? skillOptions[2] : null
            );

            levelUpUI.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("[UIManager] LevelUpUI를 찾을 수 없습니다.");
        }
    }
}