﻿using System;
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

    public GameObject victoryScreen;

    public int availableLives = 3;

    public int Lives { get; set; }

    public bool IsGameStarted { get; set; }

    public static event Action<int> OnLifeLost;

    private void Start()
    {
        this.Lives = this.availableLives;
        Screen.SetResolution(540, 960, false);
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }

    private void OnBrickDestruction(Brick obj)
    {
        if (BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
                // Invoke OnLifeLost to pass remaining lives
                OnLifeLost?.Invoke(this.Lives);
                // Reset balls
                BallsManager.Instance.ResetBalls();
                // Stop the game
                IsGameStarted = false;
                // Reload level
                BricksManager.Instance.LoadLevel(BricksManager.Instance.currentLevel);
            }
        }
    }

    public void ShowVictoryScreen()
    {
        // Show victory screen
        victoryScreen.SetActive(true);
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
    }
}
