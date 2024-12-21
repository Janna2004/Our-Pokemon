using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    public GameObject morning; // 引用 morning 对象
    public GameObject afternoon; // 引用 afternoon 对象
    public GameObject night; // 引用 night 对象

    private GameObject selectedSky; // 当前选择的对象
    private Transform layer01, layer02, layer03; // 子层

    private float time = 0f; // 用于正弦函数的时间变量
    private const float floatSpeed = 200f; // 浮动速度
    private const float floatAmplitude = 50f; // 浮动振幅

    void Start()
    {
        // 随机选择 morning、afternoon 或 night
        int randomIndex = Random.Range(0, 3);
        switch (randomIndex)
        {
            case 0:
                selectedSky = morning;
                break;
            case 1:
                selectedSky = afternoon;
                break;
            case 2:
                selectedSky = night;
                break;
        }

        // 激活选中的对象，其他的隐藏
        morning.SetActive(selectedSky == morning);
        afternoon.SetActive(selectedSky == afternoon);
        night.SetActive(selectedSky == night);

        // 获取子层
        if (selectedSky == morning || selectedSky == afternoon)
        {
            layer01 = selectedSky.transform.Find("layer01");
            layer02 = selectedSky.transform.Find("layer02");
            layer03 = selectedSky.transform.Find("layer03");
        }
        else if (selectedSky == night)
        {
            layer01 = selectedSky.transform.Find("layer01");
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        if (selectedSky == morning || selectedSky == afternoon)
        {
            // 控制 layer02 和 layer03 以正弦函数规律上下浮动
            if (layer02 != null)
            {
                Vector3 pos = layer02.localPosition;
                pos.y = Mathf.Sin(time * floatSpeed) * floatAmplitude;
                layer02.localPosition = pos;
            }

            if (layer03 != null)
            {
                Vector3 pos = layer03.localPosition;
                pos.y = Mathf.Sin(time * floatSpeed) * floatAmplitude;
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
                    color.a = (Mathf.Sin(time * floatSpeed) + 1) / 2; // 将透明度值限制在 0 到 1 之间
                    renderer.material.color = color;
                }
            }
        }
    }
}