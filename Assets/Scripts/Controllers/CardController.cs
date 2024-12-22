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

    // ���ؿ�ƬUI
    public void LoadCardUI()
    {
        // ����Ԫ���������ÿ�ƬͼƬ
        Image pokemon = transform.Find("CardPanel/CardFront/pokemon").GetComponent<Image>();
        pokemon.sprite = cardAsset.pokemonImg;
        // ����Ԫ���������ÿ�Ƭ����ͼƬ
        Image bgImage = transform.Find("CardPanel/CardFront/bg").GetComponent<Image>();
        Sprite bgSprite = Resources.Load<Sprite>(ElementalImg.imgPath[cardAsset.elemental]);
        bgImage.sprite = bgSprite;
        // ���ÿ�Ƭ�����������Ϣ
        Text nameText = transform.Find("CardPanel/CardFront/name").GetComponent<Text>();
        nameText.text = cardAsset.pokemonName;
        Text hpText = transform.Find("CardPanel/CardFront/hp").GetComponent<Text>();
        hpText.text = cardAsset.hp.ToString();
        Text attackText = transform.Find("CardPanel/CardFront/attack").GetComponent<Text>();
        attackText.text = cardAsset.attack.ToString();
        // ���ÿ�Ƭ�����������Ϣ
        Text levelText = transform.Find("CardPanel/CardBack/level").GetComponent<Text>();
        levelText.text = "Lv." + cardAsset.level;
    }

    // ���ÿ�Ƭ�Ƿ���������
    public void SetOnBoard(bool onBoard)
    {
        this.onBoard = onBoard;
        boardManager.boardController.AddCardForPlayer(owner, cardAsset.level);
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