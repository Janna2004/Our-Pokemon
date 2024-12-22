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
        // 加载场景
        SceneManager.LoadScene("GroundScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("BoardScene", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // 切换玩家
    public void SwitchPlayer()
    {
        curPlayer = curPlayer == 1 ? (byte)2 : (byte)1;
    }

    // 下一回合
    public void NextTurn()
    {
        turnCount++;
    }

    // 开始游戏
    public void StartGame()
    {
        gameState = GameState.Playing;
        turnCount = 1;
    }

    // 暂停游戏
    public void PauseGame()
    {
        gameState = GameState.Pause;
    }

    // 结束游戏
    public void EndGame()
    {
        gameState = GameState.End;
    }
}