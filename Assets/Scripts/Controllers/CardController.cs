using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardController : MonoBehaviour
{
    private BoardManager boardManager;
    private GameManager gameManager;
    private byte owner = 0;
    private bool onBoard;
    public bool isFront = true;
    public Transform cardFront;
    public Transform cardBack;

    // Start is called before the first frame update
    private void Start()
    {
        boardManager = BoardManager.instance;
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameManager.curPlayer != owner && owner != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    // ���ÿ�Ƭ�Ƿ���������
    public void SetOnBoard(bool onBoard)
    {
        this.onBoard = onBoard;
        owner = gameManager.curPlayer;
        boardManager.AddCardForPlayer(owner);
    }

    // ��ȡ��Ƭ�Ƿ���������
    public bool IsOnBoard()
    {
        return onBoard;
    }

    // ��Ƭչʾ��
    public void ShowFront(bool toFront)
    {
        cardFront.gameObject.SetActive(toFront);
        cardBack.gameObject.SetActive(!toFront);
        isFront = toFront;
    }

    // ��ת��Ƭ������
    public void RotateToFront(bool toFront)
    {
        // ��ȡ��Ƭ��ǰ��ת�Ƕ�
        Vector3 currentRotation = transform.eulerAngles;
        int rotateY = toFront ? 0 : 180;
        Vector3 halfRotation = new Vector3(currentRotation.x, 90, currentRotation.z);
        Vector3 fullRotation = new Vector3(currentRotation.x, rotateY, currentRotation.z);
        transform.DORotate(halfRotation, 0.25f);
        // ��תһ����ٸ�����Ƭ��
        DOVirtual.DelayedCall(0.25f, () => ShowFront(toFront));
        transform.DORotate(fullRotation, 0.25f).SetDelay(0.25f);
    }

    // ��Ƭ����
    public void ShakeCard()
    {
        transform.DOShakeRotation(0.5f, 10, 50);
    }
}