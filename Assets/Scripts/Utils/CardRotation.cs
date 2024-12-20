using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CardRotation : MonoBehaviour
{
    public Transform cardFront;
    public Transform cardBack;
    private bool showingBack = false;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Camera.main == null || Camera.current == null)
        {
            return;
        }
        // 获取卡片正面的法线
        Vector3 cardNormal = cardFront.transform.forward;
        // 获取从摄像机到卡片的向量
        Vector3 rayDirection = (cardFront.position - Camera.current.transform.position).normalized;

        float dotProduct = Vector3.Dot(cardNormal, rayDirection);

        // 如果dotProduct为负值，说明射线穿透了卡片背面
        bool shouldShowBack = dotProduct < 0;

        if (shouldShowBack != showingBack)
        {
            showingBack = shouldShowBack;
            cardFront.gameObject.SetActive(!showingBack);
            cardBack.gameObject.SetActive(showingBack);
        }
    }
}