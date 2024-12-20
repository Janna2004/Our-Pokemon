using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// CardManager�࣬���ڹ���Ƭ��Ϣ
public class CardManager : MonoBehaviour
{
    public CardAsset cardAsset;
    private string pokemonName;
    private Sprite pokemonImg;
    private Elemental elemental;
    private Sprite bgSprite;
    private string[] skills;
    private int hp;
    private int attack;
    private int level;

    // ��ʼ����Ƭ��Ϣ
    public void InitCardInfo()
    {
        pokemonName = cardAsset.pokemonName;
        pokemonImg = cardAsset.pokemonImg;
        elemental = cardAsset.elemental;
        bgSprite = Resources.Load<Sprite>(ElementalImg.imgPath[elemental]);
        skills = cardAsset.skills;
        hp = cardAsset.hp;
        attack = cardAsset.attack;
        level = cardAsset.level;
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        InitCardInfo();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // ����Ԫ���������ÿ�ƬͼƬ
        Image pokemon = transform.Find("CardPanel/CardFront/pokemon").GetComponent<Image>();
        pokemon.sprite = pokemonImg;
        // ����Ԫ���������ÿ�Ƭ����ͼƬ
        Image bgImage = transform.Find("CardPanel/CardFront/bg").GetComponent<Image>();
        bgImage.sprite = bgSprite;
        // ���ÿ�Ƭ�����������Ϣ
        Text nameText = transform.Find("CardPanel/CardFront/name").GetComponent<Text>();
        nameText.text = pokemonName;
        Text hpText = transform.Find("CardPanel/CardFront/hp").GetComponent<Text>();
        hpText.text = hp.ToString();
        Text attackText = transform.Find("CardPanel/CardFront/attack").GetComponent<Text>();
        attackText.text = attack.ToString();
        // ���ÿ�Ƭ�����������Ϣ
        Text levelText = transform.Find("CardPanel/CardBack/level").GetComponent<Text>();
        levelText.text = "Lv." + level;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}