using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpUI : UIBase
{
    [SerializeField] private Slider expBar;
    private float _exp;
    private float maxExp;
    private float monsterExp = 40f;

    public float curExp
    {
        get => _exp;
        private set => _exp = Math.Clamp(value, 0, maxExp);
    }
    private void Start()
    {
        SetExp();
        GameManager.Instance.currentMaxExp = maxExp;
        GameManager.Instance.currentExp = curExp;
    }

    void Update()
    {
        maxExp = GameManager.Instance.currentMaxExp;
        curExp = GameManager.Instance.currentExp;
        if (curExp >= maxExp)
        {
            SetLevelUpExp();
            GameManager.Instance.ShowLevelUpUI();
        }
        GetMonsterExp();
    }
    public void SetExp()
    {
        curExp = 0f;
        maxExp = 100f;
    }

    public void SetLevelUpExp()
    {
        Time.timeScale = 0f;
        GameManager.Instance.Level += 1;
        curExp = curExp - maxExp;
        maxExp = maxExp * 1.4f;
        GameManager.Instance.currentMaxExp = maxExp;
    }


    public void GetMonsterExp()
    {
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
        if (sender is Monster monster)
        {
            GainExperience(monsterExp);
        }
    }
    public void GainExperience(float amount)
    {
        curExp += amount;
        GameManager.Instance.currentExp = curExp;
    }
}