using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum CardType
{
    Action,
    Fart
}

[System.Serializable]
public class Card
{
    public string name;
    public string description;
    public CardType cardType;
    public Image image;

}

[System.Serializable]
public class FartCard : Card
{
    public int soundAmount;
    public int smellAmount;
    public int gasAmount;
    public FartCard()
    {
        cardType = CardType.Fart;
    }
}
[System.Serializable]
public class ActionCard : Card
{
    public ValueSet RewardValue;
    public ValueSet PunishmentValue;
}
[System.Serializable]
public class ValueSet
{
    public int attention;
    public int smell;
    public int sound;
}