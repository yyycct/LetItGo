using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<ActionCard> actionCards = new List<ActionCard>();
    public List<FartCard> fartCards = new List<FartCard>();
    public int actionCardsPerRound = 3;
    public int fartCardsPerRound = 3;
    public void DrawRandomCard()
    {
        List<ActionCard> pickedActionCards = new List<ActionCard>();
        List<FartCard> pickedFartCards = new List<FartCard>();
        if (actionCards.Count > 0)
        {
            
            while (pickedActionCards.Count < actionCardsPerRound|| pickedActionCards.Count < actionCards.Count)
            {
                int num = Random.Range(0, actionCards.Count + 1 );
                if (!pickedActionCards.Contains(actionCards[num])){
                    pickedActionCards.Add(actionCards[num]);
                }
            }
            UIManager.instance.spawnActionCards(pickedActionCards);
        }
        if (fartCards.Count > 0)
        {
            
            while (pickedFartCards.Count < fartCardsPerRound || pickedFartCards.Count < fartCards.Count)
            {
                int num = Random.Range(0, fartCards.Count + 1);
                if (!pickedFartCards.Contains(fartCards[num]))
                {
                    pickedFartCards.Add(fartCards[num]);
                }
            }
            UIManager.instance.spawnFartCards(pickedFartCards);
        }
    }
}
