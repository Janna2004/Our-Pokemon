using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Positions")]
    public Vector3 preparePosition = new Vector3(-60, 0, -10); // 准备阶段摄像机位置
    public Vector3 playingPosition = new Vector3(0, 0, -10);   // 游戏阶段摄像机位置

    [Header("Transition Settings")]
    public float transitionDuration = 3.0f;                   // 摄像机移动动画的持续时间

    private Camera mainCamera;                                // 主摄像机
    private bool isTransitioning = false;                     // 是否正在进行动画
    private float transitionTimer = 0.0f;                     // 动画计时器
    private Vector3 startPosition;                            // 动画起始位置
    private Vector3 targetPosition;                           // 动画目标位置

    private void Start()
    {
        // 获取主摄像机
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            UnityEngine.Debug.LogError("No main camera found in the scene!");
            return;
        }

        // 订阅游戏状态改变事件
        GameManager.instance.OnGameStateChange += OnGameStateChangeHandler;

        // 初始化摄像机位置
        SetCameraPositionInstant(GameManager.instance.gameState);
    }

    private void Update()
    {
        // 如果正在进行摄像机移动动画，则更新动画状态
        if (isTransitioning)
        {
            AnimateCameraTransition();
        }
    }

    private void OnDestroy()
    {
        // 取消订阅事件，避免内存泄漏
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameStateChange -= OnGameStateChangeHandler;
        }
    }

    // 处理游戏状态变化
    private void OnGameStateChangeHandler(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            StartCameraTransition(playingPosition);
        }
        else if (newState == GameState.Prepare)
        {
            StartCameraTransition(preparePosition);
        }
    }

    // 立即设置摄像机位置
    private void SetCameraPositionInstant(GameState gameState)
    {
        if (mainCamera == null) return;

        mainCamera.transform.position = gameState == GameState.Prepare ? preparePosition : playingPosition;
    }

    // 开始动画过渡
    private void StartCameraTransition(Vector3 target)
    {
        if (mainCamera == null) return;

        startPosition = mainCamera.transform.position; // 当前摄像机的位置作为起始位置
        targetPosition = target;                      // 设置目标位置
        transitionTimer = 0.0f;                       // 重置计时器
        isTransitioning = true;                       // 开始动画
    }

    // 动画更新逻辑
    private void AnimateCameraTransition()
    {
        if (mainCamera == null) return;

        transitionTimer += Time.deltaTime; // 更新动画计时器
        float progress = Mathf.Clamp01(transitionTimer / transitionDuration); // 归一化进度（0~1）

        if (progress >= 1.0f)
        {
            // 动画结束
            mainCamera.transform.position = targetPosition;
            isTransitioning = false;
        }
        else
        {
            // 平滑插值，使用 Ease-In-Out 计算平滑因子
            float smoothFactor = EaseInOut(progress);
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, smoothFactor);
        }
    }

    // Ease-In-Out 插值函数
    private float EaseInOut(float t)
    {
        // t^2 插值：前半段慢速加速，后半段慢速减速
        return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
    }
}