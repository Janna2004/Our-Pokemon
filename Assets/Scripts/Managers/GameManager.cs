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
    public byte curPlayer = 2;
    public int turnCount = 0;
    public int moveCount = 0;
    public int maxMoveNum = 3;
    private int preparedPlayer = 0;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        instance = this;
        // 加载场景
        SceneManager.LoadScene("GroundScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("BoardScene", LoadSceneMode.Additive);
    }

    // Start is called before the first frame update
    private void Start()
    {
        SwitchPlayer();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // 切换玩家
    public void SwitchPlayer()
    {
        byte oldPlayer = curPlayer;
        curPlayer = oldPlayer == 1 ? (byte)2 : (byte)1;
        moveCount = 0;
        if (preparedPlayer == 2)
        {
            StartGame();
        }
        if (gameState == GameState.Prepare)
        {
            BoardManager.instance.boardController.BlockArea(curPlayer);
            BoardManager.instance.boardController.UnblockArea(oldPlayer);
            BoardManager.instance.boardController.ResetSelect(curPlayer);
            preparedPlayer += 1;
        }
        else if (curPlayer == 1)
        {
            NextTurn();
        }
    }

    // 下一回合
    public void NextTurn()
    {
        turnCount++;
    }

    // 下一行动
    public bool NextMove()
    {
        moveCount++;
        if (moveCount == maxMoveNum)
        {
            SwitchPlayer();
            return false;
        }
        return true;
    }

    // 开始游戏
    public void StartGame()
    {
        gameState = GameState.Playing;
        BoardManager.instance.boardController.BlockArea(1);
        BoardManager.instance.boardController.BlockArea(2);
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