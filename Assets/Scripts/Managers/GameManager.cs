using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public TopDownMovement playerMovement;

    public UIManager uiManager;
    //public SlimeManager slimeManager;
    //public MonsterManager monsterManager;
    //public SpawnManager spawnManager;
    //public ItemManager itemManager;
    public PoolManager poolManager;

    public float gameTime;
    public float maxGameTime = 2 * 10f;

    private void Start()
    {
        uiManager.init();
        //slimeManager.init();
        //monsterManager.init();
        //spawnManager.init();
        //itemManager.init();
        poolManager.init();
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
}