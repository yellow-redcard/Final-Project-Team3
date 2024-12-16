using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static SkillManager;

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
        var upgradeOptions = GameManager.Instance.skillManager.GetUpgradeOptions();

        // UI에 옵션 표시
        foreach (var option in upgradeOptions)
        {
            Debug.Log($"업그레이드 선택지: 스킬 {option.Item1}, 옵션: {option.Item2}");
        }

        // 예시: 플레이어가 첫 번째 옵션을 선택한 경우
        HandleSkillUpgrade(upgradeOptions[0].Item1, upgradeOptions[0].Item2);
    }
    private void HandleSkillUpgrade(SkillType skillType, string option)
    {
        GameManager.Instance.skillManager.UpgradeSkill(skillType, option);
    }
}
