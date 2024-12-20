using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Elemental
{
    Fire,
    Water,
    Grass,
    Electric,
    Flying,
    Rock
}

// 元素属性和背景图片的映射关系
public class ElementalImg
{
    public static Dictionary<Elemental, string> imgPath = new Dictionary<Elemental, string>
    {
        {Elemental.Fire, "CardFrames/Card_Fire"},
        {Elemental.Water, "CardFrames/Card_Water"},
        {Elemental.Grass, "CardFrames/Card_Grass"},
        {Elemental.Electric, "CardFrames/Card_Electric"},
        {Elemental.Flying, "CardFrames/Card_Flying"},
        {Elemental.Rock, "CardFrames/Card_Rock"}
    };
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardAsset : ScriptableObject
{
    [Header("Basic Information")]
    public string pokemonName;

    public Sprite pokemonImg;
    public Elemental elemental;
    public string[] skills;

    [Header("Card Stats")]
    public int hp;

    public int attack;
    public int level;
}