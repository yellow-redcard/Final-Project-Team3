using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillUI : MonoBehaviour
{
    [SerializeField] private Text KillTxt;

    void Update()
    {
        KillTxt.text = GameManager.Instance.monsterKill.ToString();
    }
}
