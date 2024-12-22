using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Prepare,
    Playing,
    Pause,
    End
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState = GameState.Prepare;
    public int playerNum = 2;
    public byte curPlayer = 1;
    public int turnCount = 0;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        // ���س���
        SceneManager.LoadScene("GroundScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("BoardScene", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // �л����
    public void SwitchPlayer()
    {
        curPlayer = curPlayer == 1 ? (byte)2 : (byte)1;
    }

    // ��һ�غ�
    public void NextTurn()
    {
        turnCount++;
    }

    // ��ʼ��Ϸ
    public void StartGame()
    {
        gameState = GameState.Playing;
        turnCount = 1;
    }

    // ��ͣ��Ϸ
    public void PauseGame()
    {
        gameState = GameState.Pause;
    }

    // ������Ϸ
    public void EndGame()
    {
        gameState = GameState.End;
    }
}