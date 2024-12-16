using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpUI : MonoBehaviour
{
    [SerializeField] private Slider expBar;
    private float _exp;
    private float maxExp = 100f;
    private float monsterExp = 40f;

    public float curExp
    {
        get => _exp;
        private set => _exp = Math.Clamp(value, 0, _exp);
    }
    private void Start()
    {
        expBar.value = 0;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"GetKeyDown:{KeyCode.E}");
            curExp += 40;
        }
        if(curExp >= maxExp)
        {
            SetLevelUpExp();
            //GameManager.Instance.uiManager.Show<LevelUpUI>();
        }
        GetMonsterExp();
    }
    public void SetExp()
    {
        expBar.value = 0;
    }

    public void SetLevelUpExp()
    {
        GameManager.Instance.Level += 1;
        maxExp = maxExp * 1.2f;
        SetExp();
    }


    public void GetMonsterExp()
    {
        //monsterExp = 몬스터 유형
        //몬스터가 파괴되었을때 경험치 증가
        //curExp += monsterExp * monsterType
        expBar.value = curExp / maxExp;
    }
}
