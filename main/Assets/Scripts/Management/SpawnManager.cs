using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of storing the global spawn pool generating enemy pools for each wave. 
/// </summary>
public class SpawnManager : MonoBehaviour
{
    /// <summary>
    /// An array containing the min and max difficulties allowed at a specific level.
    /// NOTE: Level does not mean wave; Each level is 6 waves long (with 30 waves, this is 5 levels in all). 
    /// </summary>
    public readonly int[,] diffRangeAtLevel = { { 1, 20 }, { 1, 40 }, { 21, 80 }, { 61, 100 }, { 81, 100 } };

    /// <summary>
    /// <para>The starting rate at which enemies will be spawned into the game field. For example,
    /// if the enemy spawn rate is at 2, then an enemy will spawn every 2 seconds. The spawn rate
    /// will decrease each wave until it is equal to the ending spawn rate. </para>
    /// </summary>
    public float startingSpawnRate = 3f;

    /// <summary>
    /// <para>The ending rate at which enemies will spawn. The spawn rate will start at whatever
    /// value the starting spawn rate is, and decrease each wave until it is equal to this value. </para>
    /// </summary>
    public float endingSpawnRate = 1f;

    /// <summary>
    /// <para>The list of all possible enemies that can be spawned into the game field.</para>
    /// </summary>
    public List<EnemyCard> globalSpawnPool;

    /// <summary>
    /// <para>The list of enemies that will spawn during the current wave</para>
    /// </summary>
    private List<EnemyCard> waveSpawnPool;

    /// <summary>
    /// 
    /// </summary>
    public List<GameObject> bosses;

    /// <summary>
    /// <para>A stack containing the indices of waveSpawnPool in a randomized order.</para>
    /// </summary>
    private Stack<int> enemySpawnOrder;

    public Transform[] spawnpoints;
    public Transform bossSpawnpoint;


    public int totalEnemiesInWave;
    public int enemiesLeft;

    public int minEnemiesPerWave = 10;

    public int numWavesBeforeLevelIncrease = 6;

    public int numWavesBeforeEnemyCountIncrease = 3;

    private int currentLevel;
    private GameManager gm;
    private Minimap minimap;
    private Coroutine spawning;
    private int enemiesInSpawnQueue;
    private int currentBoss = 0;

    private int maxIdx, minIdx;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        minimap = FindObjectOfType<Minimap>();
        waveSpawnPool = new List<EnemyCard>();
        enemySpawnOrder = new Stack<int>();

        //Sort enemy pool by difficulty
        globalSpawnPool.Sort(SortByDifficulty);
        UpdateSpawnSelectionRange();
    }

    //Keep track of the current number of enemies in field. 
    void Update()
    {
        enemiesLeft = FindObjectsOfType<Enemy>().Length + enemiesInSpawnQueue;
    }

    /// <summary>
    /// Activates the enemy spawner. 
    /// </summary>
    public void ActivateSpawner() {
        //Check if the min and max indeces need to be updated. 
        int oldCurrentLevel = currentLevel;
        currentLevel = gm.wave / numWavesBeforeLevelIncrease;
        if (oldCurrentLevel != currentLevel) UpdateSpawnSelectionRange();

        GenerateEnemyPool();

        spawning = StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Updates the min and max indeces of the global spawn pool such that all enemies between the indeces 
    /// are within the difficulty range of the current level. 
    /// </summary>
    private void UpdateSpawnSelectionRange() {
        //Get the lowest index that contains an enemy within the current difficulty range. 
        minIdx = 0;
        while (minIdx < globalSpawnPool.Count &&
            globalSpawnPool[minIdx].difficulty < diffRangeAtLevel[currentLevel, 0]) {
            //Debug.Log(globalSpawnPool[i]);
            minIdx++;
        }

        //Get the highest index that contains an enemy within the current difficulty range. 
        for (maxIdx = minIdx; maxIdx < globalSpawnPool.Count
            && globalSpawnPool[maxIdx].difficulty < diffRangeAtLevel[currentLevel, 1]; maxIdx++) ;
    }

    public Boss SpawnBoss() {
        GameObject boss = Instantiate<GameObject>(bosses[currentBoss], bossSpawnpoint.position, Quaternion.identity);

        RaycastHit2D[] hits = new RaycastHit2D[10];
        int x = 0;
        while (x < 5 && Physics2D.Raycast(transform.position, Vector2.zero, new ContactFilter2D().NoFilter(), hits) != 0) {
            boss.transform.position += new Vector3(-Mathf.Sign(boss.transform.position.x), 1, 0);    
            x++;
        }

        currentBoss = (currentBoss + 1) % bosses.Count;
        return boss.GetComponent<Boss>();
    }

    public bool allEnemiesDead() {
        return enemiesLeft == 0;
    }

    private void GenerateEnemyPool() {

        waveSpawnPool.Clear();

        //Generate a random selection of enemies to spawn during the swarm phase. 
        for (int i = 0; i < minEnemiesPerWave + (gm.wave / numWavesBeforeEnemyCountIncrease); i++) {
            waveSpawnPool.Add(globalSpawnPool[Random.Range(minIdx, maxIdx)]);
        }

        totalEnemiesInWave = waveSpawnPool.Count;
        enemiesLeft = totalEnemiesInWave;

        //CREATE RANDOM ORDER

        int[] temp = new int[waveSpawnPool.Count];
        //fill temp array with all of waveSpawnPool's indices
        for (int k = 0; k < waveSpawnPool.Count; k++) {
            temp[k] = k;
        }

        //scramble values 
        for (int j = 0; j < temp.Length; j++) {
            int randIdx = Random.Range(0, temp.Length - 1);
            int val = temp[j];
            temp[j] = temp[randIdx];
            temp[randIdx] = val;
        }

        //Add scramble indeces to stack. 
        for (int j = 0; j < temp.Length; j++) {
            enemySpawnOrder.Push(temp[j]);
        }
    }

    private static int SortByDifficulty(EnemyCard x, EnemyCard y) {

        Enemy ex = x.prefab.GetComponent<Enemy>();
        Enemy ey = y.prefab.GetComponent<Enemy>();

        if (ex == null || ey == null) {
            Debug.LogError("Spawn list sorting error: object does not have enemy component");
            throw new System.ArgumentException();
        }

        return (int) Mathf.Sign(ex.difficulty - ey.difficulty);
    }

    public void Reset() {
        waveSpawnPool.Clear();
        enemySpawnOrder.Clear();

        totalEnemiesInWave = 0;
        enemiesLeft = 0;
    }

    public IEnumerator SpawnEnemies() {

        //Spawn an enemy at one of the spawnpoints in the game field. 
        //Keep going until all enemies have been spawned. 
        int i = 0;
        enemiesInSpawnQueue = enemySpawnOrder.Count;
        while (enemySpawnOrder.Count > 0) {

            EnemyCard ec = waveSpawnPool[enemySpawnOrder.Pop()];

            Debug.Log(ec); //Dispay enemy info to the console. 

            GameObject enemy = Instantiate<GameObject>(ec.prefab, 
                spawnpoints[i % spawnpoints.Length].position, Quaternion.identity);
            enemy.GetComponent<Enemy>().enemyCard = ec;


            //Keep moving the enemy to the right in case it's on top of a building. 
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int x = 0;
            while (x < 5 && Physics2D.Raycast(transform.position, Vector2.zero, new ContactFilter2D().NoFilter(), hits) != 0) {
                enemy.transform.position += new Vector3(-Mathf.Sign(enemy.transform.position.x), 0, 0);    
                x++;
            }


            minimap.CreateMinimapIcon(enemy.GetComponent<Enemy>(), false);

            enemiesInSpawnQueue--;

            //Wait to spawn another enemy. spawn rate will increase as game progresses. 
            i++;
            yield return new WaitForSeconds(startingSpawnRate - ((startingSpawnRate - endingSpawnRate) / 30 * gm.wave));
        }

        spawning = null;

        yield return null;
    }
}
