using UnityEngine;

public class UIBase : MonoBehaviour
{
    public Canvas canvas;

    public virtual void Opened(params object[] param)
    {

    }

    public void Hide()
    {
        GameManager.Instance.uiManager.Hide(gameObject.name);
    }
}
