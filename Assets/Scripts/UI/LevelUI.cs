using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Text levelTxt;

    void Update()
    {
        levelTxt.text = "Lv " + GameManager.Instance.Level.ToString();
    }
}
