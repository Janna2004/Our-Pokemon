using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DragCard : DragAction
{
    public AudioClip success;
    public AudioClip fail;
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
        if (!boardController.OnCardEndDrag(transform, startCellIdx))
        {
            source.PlayOneShot(fail);
            transform.DOMove(startPos, 0.5f);
        }
        else
        {
            source.PlayOneShot(success);
            if (!cardController.IsOnBoard())
            {
                cardController.SetOnBoard(true);
            }
            else
            {
                gameManager.NextMove();
            }
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