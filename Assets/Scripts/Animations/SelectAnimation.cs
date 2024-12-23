using UnityEngine;

public class PrefabAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    public float startX = -48f;            // 动画开始的 X 坐标
    public float targetX = -200f;          // 动画结束的 X 坐标
    public float transitionDuration = 3f; // 动画持续时间

    private float transitionTimer = 0f;   // 动画计时器
    private bool isAnimating = false;     // 动画进行状态

    private void Start()
    {
        // 初始化位置
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);

        // 订阅游戏状态变化事件
        GameManager.instance.OnGameStateChange += OnGameStateChangeHandler;
    }

    private void Update()
    {
        // 如果正在进行动画，更新位置
        if (isAnimating)
        {
            AnimatePosition();
        }
    }

    private void OnDestroy()
    {
        // 取消订阅事件，防止内存泄漏
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameStateChange -= OnGameStateChangeHandler;
        }
    }

    // 游戏状态变化处理逻辑
    private void OnGameStateChangeHandler(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            StartAnimation();
        }
    }

    // 开始动画
    private void StartAnimation()
    {
        transitionTimer = 0f;   // 重置计时器
        isAnimating = true;     // 标记为动画进行中
    }

    // 动画更新逻辑
    private void AnimatePosition()
    {
        transitionTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(transitionTimer / transitionDuration); // 进度归一化到 0~1

        // 使用 Ease-In-Out 平滑插值
        float smoothProgress = EaseInOut(progress);

        // 更新位置，仅改变 X 坐标
        float currentX = Mathf.Lerp(startX, targetX, smoothProgress);
        transform.position = new Vector3(currentX, transform.position.y, transform.position.z);

        // 动画完成后停止
        if (progress >= 1f)
        {
            isAnimating = false;
        }
    }

    // Ease-In-Out 插值函数
    private float EaseInOut(float t)
    {
        return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
    }
}