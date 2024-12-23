using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DragCard : DragAction
{
    public AudioClip success;
    public AudioClip fail;
    public AudioClip attack;
    public AudioSource source;
    private GameManager gameManager;
    private BoardManager boardManager;
    private BoardController boardController;
    private CardController cardController;
    private Vector3 startPos;
    private int startCellIdx;

    private void Start()
    {
        gameManager = GameManager.instance;
        boardController = BoardManager.instance.boardController;
        cardController = GetComponent<CardController>();
    }

    public override bool CanDrag
    {
        get
        {
            if (!cardController.IsOnBoard() && !boardController.CanAddCard(cardController.owner, cardController.cardAsset.level))
            {
                if (!boardController.CanAddCard(cardController.owner, cardController.cardAsset.level))
                {
                    source.PlayOneShot(fail);
                }
                return false;
            }
            if (cardController.owner != gameManager.curPlayer)
            {
                return false;
            }
            return true;
        }
    }

    public override void OnStartDrag()
    {
        // 调入ignoreRaycast层，避免拖拽时被遮挡
        gameObject.layer = 2;
        startPos = transform.position;
        transform.DOScale(1.2f, 0.5f);
        startCellIdx = boardController.OnCardStartDrag(transform);
        cardController.RotateToFront(false);
    }

    public override void OnEndDrag()
    {
        // 调回default层
        gameObject.layer = 0;
        transform.DOScale(1f, 0.5f);

        // 调用 OnCardEndDrag 一次，并获取返回的结果
        bool[] result = boardController.OnCardEndDrag(transform, startCellIdx);
        bool isAttack = result[0]; // 是否是攻击
        bool isSuccess = result[1]; // 是否成功

        // 根据操作结果播放对应的音效
        if (isSuccess)
        {
            source.PlayOneShot(success); // 播放成功音效
            if (!cardController.IsOnBoard())
            {
                cardController.SetOnBoard(true);
            }
            else
            {
                gameManager.NextMove();
            }
        }
        else
        {
            if (isAttack)
            {
                source.PlayOneShot(attack); // 播放攻击音效
            }
            else
            {
                source.PlayOneShot(fail); // 播放失败音效
            }
            transform.DOMove(startPos, 0.5f); // 移动回原始位置
        }

        // 翻转卡片到正面
        cardController.RotateToFront(true);
    }

    public override void OnDrag()
    {
    }

    protected override bool DragSuccess()
    {
        return true;
    }
}