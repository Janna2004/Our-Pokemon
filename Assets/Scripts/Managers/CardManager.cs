using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// CardManager类，用于管理卡片信息
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

    // 初始化卡片信息
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
        // 根据元素属性设置卡片图片
        Image pokemon = transform.Find("CardPanel/CardFront/pokemon").GetComponent<Image>();
        pokemon.sprite = pokemonImg;
        // 根据元素属性设置卡片背景图片
        Image bgImage = transform.Find("CardPanel/CardFront/bg").GetComponent<Image>();
        bgImage.sprite = bgSprite;
        // 设置卡片正面的文字信息
        Text nameText = transform.Find("CardPanel/CardFront/name").GetComponent<Text>();
        nameText.text = pokemonName;
        Text hpText = transform.Find("CardPanel/CardFront/hp").GetComponent<Text>();
        hpText.text = hp.ToString();
        Text attackText = transform.Find("CardPanel/CardFront/attack").GetComponent<Text>();
        attackText.text = attack.ToString();
        // 设置卡片背面的文字信息
        Text levelText = transform.Find("CardPanel/CardBack/level").GetComponent<Text>();
        levelText.text = "Lv." + level;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}