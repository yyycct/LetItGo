using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum CardType
{
    Action,
    Fart,
    Environment
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
    public bool oneTimeUse = false;
    public ValueSet Value;
    public List<EnvironmentObject> InappropriateEnvironmentCards;
    public List<string> ReasonablePreviousActionCardNames;
    public List<string> ReasonableNextActionCardNames;
}

[System.Serializable]
public class EnvironmentCard : Card
{
    public float AttentionDamageMultiplier;
    public float SoundDamageMultiplier;
    public float SmellDamageMultiplier;
    public ValueSet InappropriatePunishmentValue;
    public EnvironmentCard()
    {
        cardType = CardType.Environment;
    }
}

[System.Serializable]
public class ValueSet
{
    [Tooltip("Attention (%)")]
    public int attention;
    public int smell;
    public int sound;
}