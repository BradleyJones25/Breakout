using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
    #region Singleton

    private static BallsManager _instance;

    public static BallsManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    [SerializeField]
    private Ball ballPrefab;

    private Ball initialBall;

    private Rigidbody2D initialBallRb;

    public float intialBallVelocity = 250;

    public List<Ball> Balls { get; set; }

    private void Start()
    {
        InitBall();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted)
        {
            // Align ball position to the paddle position
            Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
            initialBall.transform.position = ballPosition;

            if (Input.GetMouseButtonDown(0))
            {
                initialBallRb.isKinematic = false;
                initialBallRb.AddForce(new Vector2(0, intialBallVelocity));
                GameManager.Instance.IsGameStarted = true;
            }
        }
    }

    public void SpawnBalls(Vector3 position, int count)
    {
        // Iterate on the count
        for (int i = 0; i < count; i++)
        {
            // Spawn the ball
            Ball spawnedBall = Instantiate(ballPrefab, position, Quaternion.identity) as Ball;
            // Get rigidbody component of the ball
            Rigidbody2D spawnedBallRb = spawnedBall.GetComponent<Rigidbody2D>();
            // Apply gravity
            spawnedBallRb.isKinematic = false;
            // Apply force
            spawnedBallRb.AddForce(new Vector2(0, intialBallVelocity));
            // Add this spawned ball to the balls collection
            this.Balls.Add(spawnedBall);
        }
    }

    public void ResetBalls()
    {
        // Destroy all active balls
        foreach (var ball in this.Balls.ToList())
        {
            Destroy(ball.gameObject);
        }

        // Initialise the ball
        InitBall();
    }

    private void InitBall()
    {
        // Get starting position from the paddle 
        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
        initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        initialBallRb = initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<Ball>
        {
            initialBall
        };
    }
}