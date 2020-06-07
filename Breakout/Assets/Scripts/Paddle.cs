using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Paddle : MonoBehaviour
{
    private Camera mainCamera;
    private float paddleIntitialY;
    private float defaultPaddleWidthInPixels = 200;
    private float defaultLeftClamp = 135;
    private float defaultRightClamp = 410;
    private SpriteRenderer sr;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleIntitialY = this.transform.position.y;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        float paddleShift = (defaultPaddleWidthInPixels - ((defaultPaddleWidthInPixels / 2) * this.sr.size.x)) / 2;
        float leftClamp = defaultLeftClamp - paddleShift;
        float rightClamp = defaultRightClamp + paddleShift;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mouseWorldPositionX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
        this.transform.position = new Vector3(mouseWorldPositionX, paddleIntitialY, 0);
    }
}
