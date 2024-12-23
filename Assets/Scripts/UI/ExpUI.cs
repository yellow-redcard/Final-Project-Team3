using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpUI : UIBase
{
    [SerializeField] private Slider expBar;
    private float _exp;
    private float maxExp = 100f;
    private float monsterExp = 40f;

    public float curExp
    {
        get => _exp;
        private set => _exp = Math.Clamp(value, 0, maxExp);
    }
    private void Start()
    {
        expBar.value = 0;
    }

    void Update()
    {
        if(curExp >= maxExp)
        {
            SetLevelUpExp();
            GameManager.Instance.uiManager.Show<LevelUpUI>();
        }
        GetMonsterExp();
    }
    public void SetExp()
    {
        curExp = 0f;
    }

    public void SetLevelUpExp()
    {
        Time.timeScale = 0f;
        GameManager.Instance.Level += 1;
        curExp = curExp - maxExp;
        maxExp = maxExp * 1.4f;
    }


    public void GetMonsterExp()
    {
        Debug.Log($"{curExp} {maxExp}");
        expBar.value = curExp / maxExp;
    }
    private void OnEnable()
    {
        Monster.OnMonsterDie += Monster_OnMonsterDie;
    }
    private void OnDisable()
    {
        Monster.OnMonsterDie -= Monster_OnMonsterDie;
    }
    private void Monster_OnMonsterDie(object sender, EventArgs e)
    {
        Debug.Log("MonsterDie");
        if (sender is Monster monster)
        {
            GainExperience(monsterExp);
        }
    }
    public void GainExperience(float amount)
    {
        Debug.Log("Gain");
        curExp += amount;
    }
}