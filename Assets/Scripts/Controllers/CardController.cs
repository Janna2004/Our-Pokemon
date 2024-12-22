using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardController : MonoBehaviour
{
    public CardAsset cardAsset;
    private BoardManager boardManager;
    private GameManager gameManager;
    public byte owner;
    public bool onBoard;
    public bool isFront = true;
    public Transform cardFront;
    public Transform cardBack;

    // Start is called before the first frame update
    private void Start()
    {
        boardManager = BoardManager.instance;
        gameManager = GameManager.instance;
        owner = gameManager.curPlayer;
        LoadCardUI();
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameManager.curPlayer != owner)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    // 加载卡片UI
    public void LoadCardUI()
    {
        // 根据元素属性设置卡片图片
        Image pokemon = transform.Find("CardPanel/CardFront/pokemon").GetComponent<Image>();
        pokemon.sprite = cardAsset.pokemonImg;
        // 根据元素属性设置卡片背景图片
        Image bgImage = transform.Find("CardPanel/CardFront/bg").GetComponent<Image>();
        Sprite bgSprite = Resources.Load<Sprite>(ElementalImg.imgPath[cardAsset.elemental]);
        bgImage.sprite = bgSprite;
        // 设置卡片正面的文字信息
        Text nameText = transform.Find("CardPanel/CardFront/name").GetComponent<Text>();
        nameText.text = cardAsset.pokemonName;
        Text hpText = transform.Find("CardPanel/CardFront/hp").GetComponent<Text>();
        hpText.text = cardAsset.hp.ToString();
        Text attackText = transform.Find("CardPanel/CardFront/attack").GetComponent<Text>();
        attackText.text = cardAsset.attack.ToString();
        // 设置卡片背面的文字信息
        Text levelText = transform.Find("CardPanel/CardBack/level").GetComponent<Text>();
        levelText.text = "Lv." + cardAsset.level;
    }

    // 设置卡片是否在棋盘上
    public void SetOnBoard(bool onBoard)
    {
        this.onBoard = onBoard;
        boardManager.boardController.AddCardForPlayer(owner, cardAsset.level);
    }

    // 获取卡片是否在棋盘上
    public bool IsOnBoard()
    {
        return onBoard;
    }

    // 卡片展示面
    public void ShowFront(bool toFront)
    {
        cardFront.gameObject.SetActive(toFront);
        cardBack.gameObject.SetActive(!toFront);
        isFront = toFront;
    }

    // 旋转卡片到背面
    public void RotateToFront(bool toFront)
    {
        // 获取卡片当前旋转角度
        Vector3 currentRotation = transform.eulerAngles;
        int rotateY = toFront ? 0 : 180;
        Vector3 halfRotation = new Vector3(currentRotation.x, 90, currentRotation.z);
        Vector3 fullRotation = new Vector3(currentRotation.x, rotateY, currentRotation.z);
        transform.DORotate(halfRotation, 0.25f);
        // 旋转一半后再更换卡片面
        DOVirtual.DelayedCall(0.25f, () => ShowFront(toFront));
        transform.DORotate(fullRotation, 0.25f).SetDelay(0.25f);
    }

    // 卡片抖动
    public void ShakeCard()
    {
        transform.DOShakeRotation(0.5f, 10, 50);
    }
}