using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IManager
{
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
    public void ShowLevelUpUI()
    {
        List<SkillManager.SkillType> options = new List<SkillManager.SkillType>();
        var lockedSkills = System.Enum.GetValues(typeof(SkillManager.SkillType))
            .Cast<SkillManager.SkillType>()
            .Except(GameManager.Instance.skillManager.GetUnlockedSkills());

        // 확률적으로 새로운 스킬 추가
        if (lockedSkills.Any() && Random.value < 0.5f)
            options.Add(lockedSkills.First());

        // 기존 스킬 업그레이드 추가
        var unlockedSkills = GameManager.Instance.skillManager.GetUnlockedSkills();
        foreach (var skill in unlockedSkills)
        {
            if (options.Count < 3)
                options.Add(skill);
        }

        // UI 표시
        foreach (var skillOption in options)
        {
            Debug.Log($"레벨업 선택지: {skillOption}");
        }

        // 실제 UI 선택 로직 구현 필요
    }
}
