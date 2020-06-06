using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Paddle : MonoBehaviour
{
    private Camera mainCamera;
    private float paddleIntitialY;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleIntitialY = this.transform.position.y;
    }

    private void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        float leftClamp = 135;
        float rightClamp = 410;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mouseWorldPositionX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
        this.transform.position = new Vector3(mouseWorldPositionX, paddleIntitialY, 0);
    }
}
