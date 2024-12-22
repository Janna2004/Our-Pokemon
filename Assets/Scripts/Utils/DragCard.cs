using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DragCard : DragAction
{
    private BoardManager boardManager;
    private CardController cardController;
    private Vector3 startPos;
    private int startCellIdx;

    private void Start()
    {
        boardManager = BoardManager.instance;
        cardController = GetComponent<CardController>();
    }

    public override void OnStartDrag()
    {
        // 调入ignoreRaycast层，避免拖拽时被遮挡
        gameObject.layer = 2;
        startPos = transform.position;
        transform.DOScale(1.2f, 0.5f);
        startCellIdx = boardManager.boardController.OnCardStartDrag(transform);
        cardController.RotateToFront(false);
    }

    public override void OnEndDrag()
    {
        // 调回default层
        gameObject.layer = 0;
        transform.DOScale(1f, 0.5f);
        if (!boardManager.boardController.OnCardEndDrag(transform, startCellIdx))
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