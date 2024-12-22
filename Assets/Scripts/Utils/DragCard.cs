using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DragCard : DragAction
{
    private BoardController boardController;
    private CardController cardController;
    private Vector3 startPos;
    private int startCellIdx;

    private void Start()
    {
        boardController = BoardManager.instance.boardController;
        cardController = GetComponent<CardController>();
    }

    public override bool CanDrag
    {
        get
        {
            return cardController.IsOnBoard() || boardController.CanAddCard(cardController.owner, cardController.cardAsset.level);
        }
    }

    public override void OnStartDrag()
    {
        // ����ignoreRaycast�㣬������קʱ���ڵ�
        gameObject.layer = 2;
        startPos = transform.position;
        transform.DOScale(1.2f, 0.5f);
        startCellIdx = boardController.OnCardStartDrag(transform);
        cardController.RotateToFront(false);
    }

    public override void OnEndDrag()
    {
        // ����default��
        gameObject.layer = 0;
        transform.DOScale(1f, 0.5f);
        if (!boardController.OnCardEndDrag(transform, startCellIdx))
        {
            transform.DOMove(startPos, 0.5f);
        }
        else if (!cardController.IsOnBoard())
        {
            cardController.SetOnBoard(true);
        }
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