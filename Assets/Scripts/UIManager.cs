using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public void Start()
    {
        instance = this;
    }

    public GameObject actionCardsParent;
    public GameObject fartCardsParent;

    public void spawnActionCards(List<ActionCard> actionCards)
    {
        if (actionCards.Count <= 0)
            return;
    }
    public void spawnFartCards(List<FartCard> fartCards)
    {
        if (fartCards.Count <= 0)
            return;
    }
}
