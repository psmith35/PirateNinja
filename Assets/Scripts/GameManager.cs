using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Targets")]
    public List<GameObject> targets;
    public float spawnRate = 1.0f;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public GameObject titleScreen;
    public GameObject pauseScreen;
    public GameObject gameOverScreen;

    [Header("Volume")]
    public GameObject volumeScreen;
    public AudioSource backgroundMusic;
    public Slider volumeSlider;

    [Header("Game")]
    private int score = 0;
    private int lives = 0;
    [HideInInspector] public bool isGameActive;
    [HideInInspector] public bool isGamePaused;

    // Start is called before the first frame update
    void Start()
    {
        titleScreen.SetActive(true);
        volumeScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        UpdateScore(0);
        UpdateLives(3);

        scoreText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);

        volumeSlider.value = 1.0f;
        PauseGame(isGamePaused);
    }

    private void Update()
    {
        if(isGameActive && Input.GetButtonDown("Submit"))
        {
            isGamePaused = !isGamePaused;
            PauseGame(isGamePaused);
        }
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;

        titleScreen.SetActive(false);
        volumeScreen.SetActive(false);

        scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);

        spawnRate /= difficulty;
        if (targets.Count > 0) StartCoroutine(SpawnTarget());
        else Debug.Log("Error: No targets to spawn.");
    }

    public void PauseGame(bool isPaused)
    {
        pauseScreen.SetActive(isPaused);
        Time.timeScale = isPaused ? 0.0f : 1.0f;
    }

    IEnumerator SpawnTarget()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnRate);
            if(isGameActive)
            {
                int index = Random.Range(0, targets.Count);
                Instantiate(targets[index]);
            }
            else
            {
                break;
            }
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = string.Format("Score: {0}", score);
    }

    public void UpdateLives(int livesToAdd)
    {
        lives += livesToAdd;
        livesText.text = string.Format("Lives: {0}", lives);
    }

    public void LoseLife()
    {
        UpdateLives(-1);
        if(lives <= 0)
        {
            GameOver();
        }
    }

    public void SetVolume(float sliderValue)
    {
        backgroundMusic.volume = sliderValue;
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
