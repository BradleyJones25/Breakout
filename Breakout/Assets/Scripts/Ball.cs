using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private SpriteRenderer sr;

    public bool isLightningBall;

    public ParticleSystem lightningBallEffect;

    public float lightningBallDuration = 10;

    public static event Action<Ball> OnBallDeath;
    public static event Action<Ball> OnLightningBallEnable;
    public static event Action<Ball> OnLightningBallDisable;

    private void Awake()
    {
        this.sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }

    public void StartLightningBall()
    {
        // Check whether the ball is a lightning ball or not
        if (!this.isLightningBall)
        {
            // Set the lightning ball as active
            this.isLightningBall = true;
            // Disable the sprite renderer
            this.sr.enabled = false;
            // Enable the reference for the lightning ball effect 
            lightningBallEffect.gameObject.SetActive(true);
            // Start a counter for the lightning ball effect duration
            StartCoroutine(StopLightningBallAfterTime(this.lightningBallDuration));

            OnLightningBallEnable?.Invoke(this);
        }
    }

    private IEnumerator StopLightningBallAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        StopLightningBall();
    }

    private void StopLightningBall()
    {
        // Check whether the ball is a lightning ball or not
        if (this.isLightningBall)
        {
            // Set the lightning ball as inactive
            this.isLightningBall = false;
            // Enable the sprite renderer
            this.sr.enabled = true;
            // Stop the lightning ball effect
            lightningBallEffect.gameObject.SetActive(false);

            OnLightningBallDisable?.Invoke(this);
        }
    }
}
