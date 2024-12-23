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

    public AudioSource sourceAudio; // 用于播放背景音乐
    // 输入参数 - 各个 Sky 对应的音频
    public AudioClip morningAudio;
    public AudioClip afternoonAudio;
    public AudioClip nightAudio;
    private int skySelected = 0; // 选择的 Sky 对象

    // 输入参数 - 准备音乐
    public AudioClip selectAudio;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        instance = this;
        // 加载场景
        SceneManager.LoadScene("GroundScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("BoardScene", LoadSceneMode.Additive);
    }

    public void UpdateSkyState(int skyState)
    {
        skySelected = skyState;
    }

    // Start is called before the first frame update
    private void Start()
    {
        // 播放准备音乐
        sourceAudio.clip = selectAudio;
        sourceAudio.loop = true;
        sourceAudio.Play();

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
            BoardManager.instance.boardController.ResetSelect();
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

        // 停止准备音乐
        sourceAudio.Stop();

        // 根据 skySelected 播放对应的背景音乐
        switch (skySelected)
        {
            case 0:
                sourceAudio.clip = morningAudio;
                break;
            case 1:
                sourceAudio.clip = afternoonAudio;
                break;
            case 2:
                sourceAudio.clip = nightAudio;
                break;
        }

        sourceAudio.loop = true; // 设置音乐循环播放
        sourceAudio.Play();

        BoardManager.instance.boardController.BlockArea(1);
        BoardManager.instance.boardController.BlockArea(2);
    }

    // 暂停游戏
    public void PauseGame()
    {
        gameState = GameState.Pause;

        // 停止播放任何音乐
        sourceAudio.Stop();
    }

    // 结束游戏
    public void WinGame(byte player)
    {
        gameState = GameState.End;
        sourceAudio.Stop();
        Debug.Log("Player " + player + " wins!");
    }
}