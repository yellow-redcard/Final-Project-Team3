using System.Collections.Generic;
using UnityEngine;
using static SkillManager;

public class GameManager : MonoSingleton<GameManager>
{
    public TopDownMovement playerMovement;

    public UIManager uiManager;
    public SlimeManager slimeManager;
    //public MonsterManager monsterManager;
    //public SpawnManager spawnManager;
    //public ItemManager itemManager;
    public MonsterPoolManager monsterPool;
    public SkillPoolManager skillPool;
    public SkillManager skillManager;
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public Transform player { get; private set; }
    [SerializeField] private string playerTag = "Player";

    private void Awake()
    {

    }

    private void Start()
    {
        uiManager.init();
        //slimeManager.init();
        //monsterManager.init();
        //spawnManager.init();
        //itemManager.init();
        monsterPool.init();
        skillPool.init();
        skillManager.init();
        skillManager.SetCurrentElement(SkillManager.Element.Water);
        player = GameObject.FindGameObjectWithTag(playerTag).transform;

        InvokeRepeating(nameof(AutoFireSkills), 2f, 3f);
        if (Input.GetKeyDown(KeyCode.L)) // LŰ�� ������ �׽�Ʈ
        {
            Debug.Log("������! ��ų ���׷��̵� UI ǥ��");
            uiManager.ShowLevelUpUI();
        }
    }


    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
    private void AutoFireSkills()
    {
        Vector3 playerPosition = player.position;
        List<Transform> activeMonsters = monsterPool.GetActiveMonsters(); // Ȱ��ȭ�� ���� ��������

        // ��ų �߻�: �� ���� ��ų�� �� ���� ���� Ÿ����
        skillManager.FireSkill(SkillManager.SkillType.Single, playerPosition, activeMonsters);
        skillManager.FireSkill(SkillManager.SkillType.Cone, playerPosition, activeMonsters);
        skillManager.FireSkill(SkillManager.SkillType.Line, playerPosition, activeMonsters);
        skillManager.FireSkill(SkillManager.SkillType.Area, playerPosition, activeMonsters);
    }
    private void OnLevelUp()
    {
        Debug.Log("������! ��ų ������ ǥ��.");
        uiManager.ShowLevelUpUI();
    }
}