using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour
{
    private Image FilterImg;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        FilterImg = transform.Find("FilterCanvas").GetComponent<Image>();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // 设置滤镜颜色
    public void SetFilterColor(Color color)
    {
        FilterImg.color = color;
    }

    // 滤镜开关
    public void SetFilterActive(bool active)
    {
        FilterImg.gameObject.SetActive(active);
    }
}