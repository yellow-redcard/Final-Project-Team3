using UnityEngine;

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
    public Transform player {  get; private set; }
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
        // 플레이어 위치 기준으로 스킬 발사
        Vector3 spawnPosition = playerMovement.transform.position;

        skillManager.FireSkill(Skill.SkillType.Single, spawnPosition);
        skillManager.FireSkill(Skill.SkillType.Cone, spawnPosition);
        skillManager.FireSkill(Skill.SkillType.Line, spawnPosition);
        skillManager.FireSkill(Skill.SkillType.Area, spawnPosition);
    }
}
