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
    /// NOTE: Level does mean wave; Each level is 6 waves long (with 30 waves, this is 5 levels in all). 
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
    public List<GameObject> globalSpawnPool;

    /// <summary>
    /// <para>The list of enemies that will spawn during the current wave</para>
    /// </summary>
    private List<GameObject> waveSpawnPool;

    /// <summary>
    /// <para>A stack containing the indices of waveSpawnPool in a randomized order.</para>
    /// </summary>
    private Stack<int> enemySpawnOrder;

    public Transform[] spawnpoints;


    public int totalEnemiesInWave;
    public int enemiesLeft;

    private int currentLevel;
    private GameManager gm;
    private Coroutine spawning;
    private int enemiesInSpawnQueue;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        waveSpawnPool = new List<GameObject>();
        enemySpawnOrder = new Stack<int>();
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
        GenerateEnemyPool();

        spawning = StartCoroutine(SpawnEnemies());
    }

    public bool allEnemiesDead() {
        return enemiesLeft == 0;
    }

    private void GenerateEnemyPool() {
        //Sort enemy pool by difficulty
        globalSpawnPool.Sort(SortByDifficulty);

        //Find the first index that's within the minimum difficulty.
        int i = 0;
        while (i < globalSpawnPool.Count && globalSpawnPool[i].GetComponent<Enemy>().difficulty < diffRangeAtLevel[currentLevel, 0]) {
            //Debug.Log(globalSpawnPool[i]);
            i++;
        }

        //Debug.Log("@GenerateEnemyPool: Finding enemies starting at index " + i);

        //Add all enemies with difficulty range to wave spawn pool
        while (i < globalSpawnPool.Count && globalSpawnPool[i].GetComponent<Enemy>().difficulty < diffRangeAtLevel[currentLevel, 1]) {
            waveSpawnPool.Add(globalSpawnPool[i]);
            i++;
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

    private static int SortByDifficulty(GameObject x, GameObject y) {

        Enemy ex = x.GetComponent<Enemy>();
        Enemy ey = y.GetComponent<Enemy>();

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
            GameObject enemy = Instantiate<GameObject>(waveSpawnPool[enemySpawnOrder.Pop()], 
                spawnpoints[i % spawnpoints.Length].position, Quaternion.identity);

            enemiesInSpawnQueue--;

            //Wait to spawn another enemy. spawn rate will increase as game progresses. 
            yield return new WaitForSeconds(startingSpawnRate - ((startingSpawnRate - endingSpawnRate) / 30 * gm.wave));
        }

        spawning = null;

        yield return null;
    }
}
