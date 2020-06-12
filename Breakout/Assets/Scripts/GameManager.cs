using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public GameObject gameOverScreen;

    public int availableLives = 3;

    public int Lives { get; set; }

    public bool IsGameStarted { get; set; }

    private void Start()
    {
        this.Lives = this.availableLives;
        Screen.SetResolution(540, 960, false);
        Ball.OnBallDeath += OnBallDeath;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBallDeath(Ball obj)
    {
        // Check how many balls are remaining
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            // If the count is less than 0 we lose 1 life
            this.Lives--;

            // If all lives are lost
            if (this.Lives < 1)
            {
                // Show game over screen
                gameOverScreen.SetActive(true);
            }
            else
            {
                // Reset balls
                BallsManager.Instance.ResetBalls();
                // Stop the game
                IsGameStarted = false;
                // Reload level
                BricksManager.Instance.LoadLevel(BricksManager.Instance.currentLevel);
            }
        }
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
    }
}
