using System.Collections.Generic;
using UnityEngine;
using static SkillManager;

public class GameManager : MonoSingleton<GameManager>
{
    public TopDownMovement playerMovement;

    public UIManager uiManager;
    public SlimeManager slimeManager;
    public MonsterManager monsterManager;
    //public SpawnManager spawnManager;
    //public ItemManager itemManager;
    public MonsterPoolManager monsterPool;
    public SkillPoolManager skillPool;
    public SkillManager skillManager;
    public TileMapManager tileMapManager;
    public float gameTime;
    public float maxGameTime = 30 * 60f;
    public int monsterKill = 0;
    public int Level = 1;
    public Transform player { get; private set; }
    [SerializeField] private string playerTag = "Player";

    private void Start()
    {
        uiManager.init();
        slimeManager.init();
        monsterManager.init();
        //spawnManager.init();
        //itemManager.init();
        monsterPool.init();
        skillPool.init();
        skillManager.init();
        //skillManager.SetCurrentElement(SkillManager.Element.Water);
        player = GameObject.FindGameObjectWithTag(playerTag).transform;

        if (tileMapManager != null)
        {
            tileMapManager.Init(player);
        }

        InvokeRepeating(nameof(AutoFireSkills), 2f, 3f);
        uiManager.Show<KillUI>();
    }


    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 0f;
            GameManager.Instance.uiManager.Show<TagSlimeUI>();
        }
    }
    private void AutoFireSkills()
    {
        Vector3 playerPosition = player.position;
        List<Transform> activeMonsters = monsterPool.GetActiveMonsters(); // 활성화된 몬스터 가져오기

        // 스킬 발사: 한 번의 스킬당 한 마리 몬스터 타겟팅
        skillManager.FireSkill(SkillManager.SkillType.Single, playerPosition, activeMonsters);
        skillManager.FireSkill(SkillManager.SkillType.Cone, playerPosition, activeMonsters);
        skillManager.FireSkill(SkillManager.SkillType.Line, playerPosition, activeMonsters);
        skillManager.FireSkill(SkillManager.SkillType.Area, playerPosition, activeMonsters);
    }
}