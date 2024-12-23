using System;
using System.Diagnostics;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    // 输入参数 - 各个 Sky 对象
    public GameObject morning; // 引用 morning 对象
    public GameObject afternoon; // 引用 afternoon 对象
    public GameObject night; // 引用 night 对象

    // 输入参数 - 各子层
    public Transform morningLayer01, morningLayer02, morningLayer03; // Morning 子层
    public Transform afternoonLayer01, afternoonLayer02, afternoonLayer03; // Afternoon 子层
    public Transform nightLayer01, nightLayer02; // Night 子层

    private GameObject selectedSky; // 当前选择的 Sky 对象
    private Transform layer01, layer02, layer03; // 动态绑定的子层

    private float time = 0f; // 用于正弦函数的时间变量
    private const float floatSpeed = 1f; // 浮动速度
    private const float floatAmplitude = 0.05f; // 浮动振幅

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnSkyStateChange += HandleSkyStateChange;
            UnityEngine.Debug.Log("SkyController successfully subscribed to OnSkyStateChange in Awake.");
        }
    }

    private void OnDestroy()
    {
        // 取消订阅事件，防止内存泄漏
        if (GameManager.instance != null)
        {
            GameManager.instance.OnSkyStateChange -= HandleSkyStateChange;
        }
    }

    private void Update()
    {
        time += Time.deltaTime; // 更新时间变量

        if (selectedSky == morning || selectedSky == afternoon)
        {
            // 控制 layer02 和 layer03 以正弦函数规律上下浮动
            if (layer02 != null)
            {
                Vector3 pos = layer02.localPosition;
                pos.y = layer02.localPosition.y + Mathf.Sin(time * floatSpeed) * floatAmplitude;
                layer02.localPosition = pos;
            }

            if (layer03 != null)
            {
                Vector3 pos = layer03.localPosition;
                pos.y = layer03.localPosition.y - Mathf.Sin(time * floatSpeed) * floatAmplitude;
                layer03.localPosition = pos;
            }
        }
        else if (selectedSky == night)
        {
            // 控制 layer01 的透明度以正弦函数规律变化
            if (layer01 != null)
            {
                Renderer renderer = layer01.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Color color = renderer.material.color;
                    color.a = (Mathf.Sin(time * floatSpeed * 3 / 4) + 1) / 2; // 将透明度值限制在 0 到 1 之间
                    renderer.material.color = color;
                }
            }
        }
    }

    // 处理 Sky 状态变化
    private void HandleSkyStateChange(int skyState)
    {
        UnityEngine.Debug.Log($"SkyController received skyState: {skyState}"); // 检查是否收到了事件。

        // 根据 skyState 激活指定的 Sky 对象
        switch (skyState)
        {
            case 0:
                selectedSky = morning;
                layer01 = morningLayer01;
                layer02 = morningLayer02;
                layer03 = morningLayer03;
                break;
            case 1:
                selectedSky = afternoon;
                layer01 = afternoonLayer01;
                layer02 = afternoonLayer02;
                layer03 = afternoonLayer03;
                break;
            case 2:
                selectedSky = night;
                layer01 = nightLayer01;
                layer02 = nightLayer02;
                layer03 = null;
                break;
        }

        UnityEngine.Debug.Log($"Selected sky: {selectedSky.name}"); // 确认选中的天空对象。

        // 激活选中的对象，其他的隐藏
        morning.SetActive(selectedSky == morning);
        afternoon.SetActive(selectedSky == afternoon);
        night.SetActive(selectedSky == night);
    }
}