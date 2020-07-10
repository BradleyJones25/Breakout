using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType
{
    START_GAME,
    QUIT_GAME,
    RESTART_GAME,
    MAIN_MENU
}

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour
{
    public ButtonType buttonType;

    CanvasManager canvasManager;
    Button menuButton;

    private void Start()
    {
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(OnButtonClicked);
        canvasManager = CanvasManager.GetInstance();
    }

    void OnButtonClicked()
    {
        switch (buttonType)
        {
            case ButtonType.START_GAME:
                // Start the game
                canvasManager.SwitchCanvas(CanvasType.HUD);
                GameManager.Instance.InitGame();
                break;
            case ButtonType.QUIT_GAME:
                // Quit the game
                QuitGame();
                break;
            case ButtonType.RESTART_GAME:
                // Restart the game
                canvasManager.SwitchCanvas(CanvasType.HUD);
                GameManager.Instance.InitGame();
                GameManager.Instance.RestartGame();
                BricksManager.Instance.LoadLevel(0);
                break;
            case ButtonType.MAIN_MENU:
                // Back to main menu
                canvasManager.SwitchCanvas(CanvasType.MainMenuScreen);
                GameManager.Instance.RestartGame();
                BricksManager.Instance.LoadLevel(0);
                break;
        }
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME!");
        Application.Quit();
    }
}