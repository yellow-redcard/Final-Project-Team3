using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InFoType { Exp, Level, Kill, TIme, Health }
    public InFoType type;
    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InFoType.Exp:
                //float curExp = GameManager.Instance.exp;
                //float maxExp = GameManager.Instance.nextExp;
                //mySlider.value = curExp/maxExp;
                break;
            case InFoType.Level:

                break;
            case InFoType.Kill:

                break;
            case InFoType.TIme:

                break;
            case InFoType.Health:

                break;
        }
    }
}
