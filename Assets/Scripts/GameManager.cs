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

    public TMP_Text timerText;
    public TMP_Text enemiesLeftText;

    public bool paused = false;

    private float currentTime;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        enemiesLeftText.text = enemiesInLevel.Count.ToString();
        timerText.text = "00:00";

        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

        if (!paused)
        {
            currentTime = currentTime + Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);

            timerText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
        }
    }

    public void EnemyKilled(GameObject enemy)
    {
        enemiesInLevel.Remove(enemy);
        enemiesLeftText.text = "ENEMIES LEFT: " + enemiesInLevel.Count;

        if (enemiesInLevel.Count < 1)
        {
            Debug.Log("Door Opened");
        }
    }

    public void ChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
