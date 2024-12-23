using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

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
    // 游戏状态变化事件
    public event Action<GameState> OnGameStateChange;
    public event Action<int> OnSkyStateChange;

    public byte curPlayer = 2;
    public int turnCount = 0;
    public int moveCount = 0;
    public int maxMoveNum = 3;
    private int preparedPlayer = 0;

    // 输入参数 - 各个 Sky 对应的音频
    public AudioClip morningAudio;
    public AudioClip afternoonAudio;
    public AudioClip nightAudio;
    private int skySelected = 0; // 选择的 Sky 对象

    // 输入参数 - 准备音乐
    public AudioClip selectAudio;
    public AudioSource backgroundSourceAudio; // 用于播放背景音乐
    public AudioSource SFXSourceAudio; // 用于播放音效

    public AudioClip startAudio; // 游戏开始音乐
    public AudioClip switchAudio; // 游戏换边音乐
    public AudioClip winAudio; // 游戏胜利音乐

    // 输入参数 - 游戏信息
    public UnityEngine.UI.Text infoText;

    // 引用 EndWindow prefab
    public GameObject endWindowPrefab;

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
        // 播放准备音乐
        backgroundSourceAudio.clip = selectAudio;
        backgroundSourceAudio.loop = true;
        backgroundSourceAudio.Play();

        // 随机选择天空
        int randomIndex = UnityEngine.Random.Range(0, 3);
        skySelected = randomIndex;

        // 触发天空状态变化事件
        OnSkyStateChange?.Invoke(randomIndex);

        // 更新 UI 文本为“准备阶段”
        UpdateInfoText("准备阶段");

        SwitchPlayer();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // 设置游戏状态事件
    public void SetGameState(GameState newState)
    {
        if (gameState != newState)
        {
            gameState = newState;

            // 触发游戏状态变化事件
            OnGameStateChange?.Invoke(newState);
        }
    }

    // 更新天空状态
    public void UpdateSkyState(int skyState)
    {
        skySelected = skyState;
    }

    // 切换玩家
    public void SwitchPlayer()
    {
        byte oldPlayer = curPlayer;
        curPlayer = oldPlayer == 1 ? (byte)2 : (byte)1;
        moveCount = 0;
        if (preparedPlayer == 2 && gameState != GameState.Playing)
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
            // 播放换边音效
            SFXSourceAudio.PlayOneShot(switchAudio);
            NextTurn();
        }
        // 更新游戏文本信息
        if (gameState == GameState.Playing)
        {
            UpdateInfoText($"回合{turnCount} PLAYER{curPlayer}");
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
        SetGameState(GameState.Playing);

        // 停止准备音乐
        backgroundSourceAudio.Stop();
        // 播放开始音效
        backgroundSourceAudio.PlayOneShot(startAudio);

        // 根据 skySelected 播放对应的背景音乐
        switch (skySelected)
        {
            case 0:
                backgroundSourceAudio.clip = morningAudio;
                break;
            case 1:
                backgroundSourceAudio.clip = afternoonAudio;
                break;
            case 2:
                backgroundSourceAudio.clip = nightAudio;
                break;
        }

        backgroundSourceAudio.loop = true; // 设置音乐循环播放
        backgroundSourceAudio.Play();

        BoardManager.instance.boardController.BlockArea(1);
        BoardManager.instance.boardController.BlockArea(2);

        // 更新游戏文本信息为“回合1 PLAYER1”
        UpdateInfoText($"回合1 PLAYER1");
    }

    // 暂停游戏
    public void PauseGame()
    {
        SetGameState(GameState.Pause);

        // 停止播放任何音乐
        backgroundSourceAudio.Stop();
        SFXSourceAudio.Stop();
    }

    // 结束游戏
    public void WinGame(byte player)
    {
        SetGameState(GameState.End);
        // 停止播放任何音乐
        backgroundSourceAudio.Stop();
        SFXSourceAudio.Stop();
        // 播放胜利音乐
        backgroundSourceAudio.PlayOneShot(winAudio);
        // 文本输出
        UpdateInfoText($"PLAYER{player} 胜利");
        // 显示 EndWindow 并更新文本
        ShowEndWindow(player);
    }

    // 更新 UI 文本内容的方法
    private void UpdateInfoText(string text)
    {
        if (infoText != null)
        {
            infoText.text = text;
        }
        else
        {
            UnityEngine.Debug.LogWarning("InfoText is not assigned in the GameManager!");
        }
    }

    // 显示 EndWindow 并更新文本
    private void ShowEndWindow(byte player)
    {
        if (endWindowPrefab != null)
        {
            // 实例化 EndWindow prefab
            GameObject endWindow = Instantiate(endWindowPrefab, transform);

            // 找到对象中的 Text 组件并更新内容
            /*UnityEngine.UI.Text endText = endWindow.GetComponentInChildren<UnityEngine.UI.Text>();*/
            GameObject content = endWindow.transform.Find("Content").gameObject;
            UnityEngine.UI.Text endText = content.GetComponent<UnityEngine.UI.Text>();
            if (endText != null)
            {
                endText.text = $"玩家{player}赢得了对战!";
            }
            else
            {
                UnityEngine.Debug.LogWarning("EndWindow prefab does not contain a Text component!");
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("EndWindow prefab is not assigned in the GameManager!");
        }
    }
}