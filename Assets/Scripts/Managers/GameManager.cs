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
    public float gameTime;
    public float maxGameTime = 30 * 60f;
    public int monsterKill = 0;
    public HealthSystem currentHealthSystem;
    public int Level { get; set; } = 1;
    public float currentMaxExp { get; set; }
    public float currentExp { get; set; }
    public Transform player { get; set; }
    [SerializeField] private string playerTag = "Player";
    public Slime slime;

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
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        uiManager.Show<KillUI>();
        uiManager.Show<ExpUI>();
        uiManager.Show<SlimeHPUI>();
        uiManager.Show<LevelUI>();
        StartCoroutine(skillManager.AutoFireSkills());
        UpdatePlayer(player);
        
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            GameManager.Instance.uiManager.Show<PauseUI>();
        }
        if (slimeManager.currentSlime != null)
        {
            SetPlayerPosition();
        }
    }
    void SetPlayerPosition()
    {
        player.position = slimeManager.currentSlime.transform.position;
    }
    public void UpdatePlayer(Transform newPlayer)
    {
        player = newPlayer;
        playerMovement = player.GetComponent<TopDownMovement>();
    }
    public void ShowLevelUpUI()
    {
        

        // 업그레이드 또는 해금 가능한 스킬 데이터 가져오기
        List<SkillData> upgradeableSkills = skillManager.GetLevelUpOptions();
        if (upgradeableSkills.Count > 0)
        {
            // UIManager를 통해 LevelUp UI 호출
            uiManager.ShowLevelUpUI(upgradeableSkills);
            Time.timeScale = 0f; // 게임 일시 정지
        }
    }
}

