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
        // ��ȡ��Ƭ����ķ���
        Vector3 cardNormal = cardFront.transform.forward;
        // ��ȡ�����������Ƭ������
        Vector3 rayDirection = (cardFront.position - Camera.current.transform.position).normalized;

        float dotProduct = Vector3.Dot(cardNormal, rayDirection);

        // ���dotProductΪ��ֵ��˵�����ߴ�͸�˿�Ƭ����
        bool shouldShowBack = dotProduct < 0;

        if (shouldShowBack != showingBack)
        {
            showingBack = shouldShowBack;
            cardFront.gameObject.SetActive(!showingBack);
            cardBack.gameObject.SetActive(showingBack);
        }
    }
}