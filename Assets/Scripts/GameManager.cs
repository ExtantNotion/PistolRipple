using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public List<GameObject> enemiesInLevel = new List<GameObject>();
    //public List<int> endlessModeEnemiesPerRound = new List<int>();
    public List<GameObject> endlessModeEnemySpawnSpots = new List<GameObject>();
    public List<GameObject> octopusSpots = new List<GameObject>();

    public Transform playerTransform;

    public TMP_Text timerText;
    public TMP_Text enemiesLeftText;

    public bool paused = false;

    public bool endlessMode = false;
    public GameObject enemyPrefab;
    public GameObject enemyHealthbarPrefab;
    public int currentRound = 0;
    public int enemiesKilled = 0;

    public GameObject octopus;
    public SpriteRenderer octopusSprite;

    [SerializeField] private RectTransform levelOver;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private RectTransform levelFailed;
    [SerializeField] private RectTransform levelWon;
    [SerializeField] private RectTransform waveCleared;

    public AudioSource music;

    public Canvas worldSpaceCanvas;

    public GameObject inkPrefab;

    private float currentTime;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        enemiesLeftText.text = enemiesInLevel.Count.ToString();
        timerText.text = "00:00";

        currentTime = 0;

        /*if (endlessMode)
        {
            if (enemiesInLevel.Count == 0)
            {
                InvokeRepeating("SpawnEnemies", 1f, 1f); 
            }
        }*/

        octopusSprite.DOFade(0, 0.01f);

        music.Play();
    }

    public void Pause()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void LevelOver(bool state)
    {
        levelOver.gameObject.SetActive(true);
        if (state) // Level passed
        {
            levelWon.gameObject.SetActive(true);
        }
        else // Level failed, played died
        {
            levelFailed.gameObject.SetActive(true);
        }

        //scoreText.text = "SCORE: " + enemiesKilled;
    }

    bool spawnDelayed = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (!paused)
        {
            currentTime = currentTime + Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);

            timerText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
        }

        if (endlessMode && !spawnDelayed)
        {
            spawnDelayed = true;
            StartCoroutine(SpawnEnemy());
        }

        /*if (currentRound == endlessModeEnemiesPerRound.Count && enemiesInLevel.Count == 0 && !oncePlease)
        {
            oncePlease = true;
            //CancelInvoke(SpawnEnemies);
            //LevelOver(true);
            //Pause();
        }**/
    }

    /*public void SpawnEnemies()
    {
        if (!enemiesSpawned)
        {
            if (currentRound != 0) waveCleared.gameObject.SetActive(true);
            enemiesSpawned = true;
            StartCoroutine(SpawnDelay());
        }
    }*/

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f,7f));

        GameObject spawnSpot = endlessModeEnemySpawnSpots[UnityEngine.Random.Range(0, endlessModeEnemySpawnSpots.Count - 1)];
        GameObject newEnemy = Instantiate(enemyPrefab, spawnSpot.transform.position, Quaternion.identity);
        GameObject newEnemyHealthbar = Instantiate(enemyHealthbarPrefab, Vector2.zero, Quaternion.identity, worldSpaceCanvas.transform);

        Healthbar enemyHB = newEnemyHealthbar.GetComponent<Healthbar>();
        enemyHB.targetTransform = newEnemy.transform;

        EnemyController controller = newEnemy.GetComponent<EnemyController>();
        controller.playerTransform = playerTransform;
        controller.healthbar = enemyHB;

        spawnDelayed = false;

        ShowOctopus();

        //enemiesInLevel.Insert(i, newEnemy);

        /*for (int i = 0; i < endlessModeEnemiesPerRound[currentRound]; i++)
        {
            GameObject spawnSpot = endlessModeEnemySpawnSpots[UnityEngine.Random.Range(0, endlessModeEnemySpawnSpots.Count - 1)];
            GameObject newEnemy = Instantiate(enemyPrefab, spawnSpot.transform.position, Quaternion.identity);
            GameObject newEnemyHealthbar = Instantiate(enemyHealthbarPrefab, Vector2.zero, Quaternion.identity, worldSpaceCanvas.transform);

            Healthbar enemyHB = newEnemyHealthbar.GetComponent<Healthbar>();
            enemyHB.targetTransform = newEnemy.transform;

            EnemyController controller = newEnemy.GetComponent<EnemyController>();
            controller.playerTransform = playerTransform;
            controller.healthbar = enemyHB;

            enemiesInLevel.Insert(i, newEnemy);

        }*/

        //currentRound++;
        //waveCleared.gameObject.SetActive(false);
    }

    public void EnemyKilled(GameObject enemy)
    {
        enemiesKilled++;
        enemiesLeftText.text = enemiesKilled.ToString();

        /**enemiesInLevel.Remove(enemy);
        enemiesLeftText.text = enemiesInLevel.Count.ToString();

        if (!endlessMode)
        {
            if (enemiesInLevel.Count < 1)
            {
                Debug.Log("Enemies killed");
                enemiesSpawned = false;
            }
        }**/
    }

    public void ShowOctopus()
    {
        var chance = UnityEngine.Random.Range(1,3);
        if (chance == 2)
        {
            octopus.SetActive(true);

            GameObject chosenSpot = octopusSpots[UnityEngine.Random.Range(0, octopusSpots.Count - 1)];
            octopus.transform.position = chosenSpot.transform.position - new Vector3(0,-5,0);
            octopus.transform.DOMove(chosenSpot.transform.position,0.5f,false).SetEase(Ease.OutBack);
            octopusSprite.DOFade(1, 0.5f);

            var x = UnityEngine.Random.Range(-5, 5);
            var y = UnityEngine.Random.Range(-5, 5);

            GameObject newInk = Instantiate(inkPrefab, playerTransform.position + new Vector3(x, y, 0), Quaternion.identity);
            SpriteRenderer inkSprite = newInk.GetComponent<SpriteRenderer>();

            StartCoroutine(HideOctopus(inkSprite));

            
            //octopusSprite.DOFade(0, 10f);

            Destroy(newInk,12f);
        }
    }

    IEnumerator HideOctopus(SpriteRenderer sprite)
    {
        yield return new WaitForSeconds(2f);
        octopusSprite.DOFade(0, 2f);
        sprite.DOFade(0, 10f);

        octopus.SetActive(false);
    }

    public void ChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
