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
    public int Level = 1;
    public Transform player { get; set; }
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
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
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
    public void UpdatePlayer(Transform newPlayer)
    {
        player = newPlayer;
        Debug.Log($"[GameManager] 플레이어가 업데이트되었습니다: {newPlayer.name}");
        playerMovement = player.GetComponent<TopDownMovement>();
    }
    public void ShowLevelUpUI()
    {
        if (uiManager == null)
        {
            Debug.LogError("[GameManager] UIManager가 초기화되지 않았습니다.");
            return;
        }

        List<SkillData> upgradeableSkills = skillManager.GetLevelUpOptions();
        if (upgradeableSkills.Count > 0)
        {
            uiManager.ShowLevelUpUI(upgradeableSkills);
            Time.timeScale = 0f; // 게임 일시 정지
            Debug.Log($"[GameManager] 레벨업 UI 호출. 가능한 스킬 개수: {upgradeableSkills.Count}");
        }
        else
        {
            Debug.LogWarning("[GameManager] 업그레이드 가능한 스킬이 없습니다.");
        }
    }
}

