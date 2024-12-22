using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static PauseController instance;
    private GameManager gameManager;
    private Canvas canvas;
    private GameState savedState;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameManager.instance;
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameManager.gameState == GameState.Pause)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Resume();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        savedState = gameManager.gameState;
        gameManager.gameState = GameState.Pause;
        canvas.gameObject.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Resume()
    {
        gameManager.gameState = savedState;
        canvas.gameObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}