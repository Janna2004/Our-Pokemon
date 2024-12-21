using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DragCard : DragAction
{
    private BoardManager boardManager;
    private Vector3 startPos;

    private void Start()
    {
        boardManager = BoardManager.instance;
    }

    public override void OnStartDrag()
    {
        // 调入ignoreRaycast层，避免拖拽时被遮挡
        gameObject.layer = 2;
        startPos = transform.position;
        transform.DOScale(1.2f, 0.5f);
    }

    public override void OnEndDrag()
    {
        // 调回default层
        gameObject.layer = 0;
        transform.DOScale(1f, 0.5f);
        if (!boardManager.AddCard(transform))
        {
            transform.DOMove(startPos, 0.5f);
        }
    }

    public override void OnDrag()
    {
    }

    protected override bool DragSuccess()
    {
        return true;
    }
}