using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public void awake()
    {
        instance = this;
    }

    public List<ActionCardObject> actionCards = new List<ActionCardObject>();
    public List<FartCardObject> fartCards = new List<FartCardObject>();
    public List<EnvironmentObject> environmentCards = new List<EnvironmentObject>();

    public int actionCardsPerRound = 3;
    // private int fartCardsPerRound = 1;
    // private int environmentCardsPerRound = 1;
    private UIManager uiManager;
    private EnvironmentCard pickedEnvironmentCard;

    private int CurrentGas = 100;
    private int CurrentSmell = 0;
    private int CurrentSound = 0;

    // Game loop:
    // 1. Get one random fart card, one random environment card, and three random action cards
    // 2. Display the cards
    // 3. Player place the cards
    // 4. Calculate the score
    // 5. Repeat
    public void Start()
    {
        uiManager = UIManager.instance;
        DrawEnvironmentCard();
        DrawFartCard();
        DrawActionCards();
        pickedEnvironmentCard = new EnvironmentCard();
    }
    public void Update()
    {
    }

    public void DrawEnvironmentCard()
    {
        // Draw a random environment card
        pickedEnvironmentCard = environmentCards[Random.Range(0, environmentCards.Count)].card;
        uiManager.spawnEnvironmentCard(pickedEnvironmentCard);
    }

    public void DrawFartCard()
    {
        // Draw a random fart card
        FartCard pickedFartCard = new FartCard();
        pickedFartCard = fartCards[Random.Range(0, fartCards.Count)].card;
        uiManager.spawnFartCards(pickedFartCard);
    }

    public void DrawActionCards()
    {
        // Draw three random action cards
        List<ActionCard> pickedActionCards = new List<ActionCard>();
        for (int i = 0; i < actionCardsPerRound; i++)
        {
            ActionCardObject pickedActionCard = new ActionCardObject();
            pickedActionCard = actionCards[Random.Range(0, actionCards.Count)];
            pickedActionCards.Add(pickedActionCard.card);
        }
        uiManager.spawnActionCards(pickedActionCards);
    }

    public void CalculateScore(List<ActionCard> acitonCardsPlayed, FartCard fartCardPlayed)
    {
        // Calculate the score
        int fartDamageSound = fartCardPlayed.soundAmount;
        int fartDamageSmell = fartCardPlayed.smellAmount;

        Debug.Log("fartDamageSound: " + fartDamageSound);
        Debug.Log("fartDamageSmell: " + fartDamageSmell);

        foreach (ActionCard actionCard in acitonCardsPlayed)
        {
            int attention = actionCard.Value.attention;
            int smell = actionCard.Value.smell;
            int sound = actionCard.Value.sound;

            float attentionMultiplier = 1.0f + (attention / 100.0f);
            // calculate the damage
            if (fartDamageSound > 0)
            {
                fartDamageSound = times(fartDamageSound, attentionMultiplier);
                fartDamageSound += sound;
            }
            if (fartDamageSmell > 0)
            {
                fartDamageSmell = times(fartDamageSmell, attentionMultiplier);
                fartDamageSmell += smell;
            }

            if (fartDamageSound < 0) fartDamageSound = 0;
            if (fartDamageSmell < 0) fartDamageSmell = 0;
        }
        Debug.Log("fartDamageSound2: " + fartDamageSound);
        Debug.Log("fartDamageSmell2: " + fartDamageSmell);

        // finally apply the environment buffs
        float envAttention = pickedEnvironmentCard.AttentionDamageMultiplier;
        float envSound = pickedEnvironmentCard.SoundDamageMultiplier;
        float envSmell = pickedEnvironmentCard.SmellDamageMultiplier;
        bool isInappropriate = false;
        foreach (ActionCard actionCard in acitonCardsPlayed)
        {
            // determine the scene sensitivity
            foreach (EnvironmentObject InappropriateEnvironmentCard in actionCard.InappropriateEnvironmentCards)
            {
                if (InappropriateEnvironmentCard.card.name == pickedEnvironmentCard.name)
                {
                    isInappropriate = true;
                    break;
                }
            }
        }

        if (isInappropriate)
        {
            envAttention = pickedEnvironmentCard.InappropriatePunishmentValue.attention;
            envSound = pickedEnvironmentCard.InappropriatePunishmentValue.sound;
            envSmell = pickedEnvironmentCard.InappropriatePunishmentValue.smell;
        }

        // calculate the damage
        if (envAttention > 0)
        {
            fartDamageSound = times(fartDamageSound, envAttention);
            fartDamageSmell = times(fartDamageSmell, envAttention);
        }
        if (envSound > 0)
        {
            fartDamageSound = times(fartDamageSound, envSound);
        }
        if (envSmell > 0)
        {
            fartDamageSmell = times(fartDamageSmell, envSmell);
        }
        Debug.Log("fartDamageSound4: " + fartDamageSound);
        Debug.Log("fartDamageSmell4: " + fartDamageSmell);

        // update the player stats
        CurrentGas -= fartCardPlayed.gasAmount;
        CurrentSound += fartDamageSound;
        CurrentSmell += fartDamageSmell;

        // update the UI
        uiManager.UpdateGasSlider(CurrentGas);
        uiManager.UpdateSmellSlider(CurrentSmell);
        uiManager.UpdateSoundSlider(CurrentSound);
    }

    // On play button click
    public void OnPlayButtonClicked(GameObject actionCardParent, GameObject fartCardParent)
    {
        // Player place the cards
        List<ActionCard> actionCardsPlayed = new List<ActionCard>();
        int fartCardPlayedIndex = -1;
        FartCard fartCardPlayed = new FartCard();
        foreach(Transform child in actionCardParent.transform)
        {
            if(child.childCount > 0)
            {
                ActionCard card = (ActionCard)child.GetChild(0).GetComponent<CardDragObject>().card;
                actionCardsPlayed.Add(card);
            }
        }
        for(int i = 0; i<fartCardParent.transform.childCount; i++)
        {
            if (fartCardParent.transform.GetChild(i).childCount > 0)
            {
                fartCardPlayedIndex = i;
                fartCardPlayed = (FartCard)fartCardParent.transform.GetChild(i).GetChild(0).GetComponent<CardDragObject>().card;
                break;
            }
        }

        CalculateScore(actionCardsPlayed, fartCardPlayed);
    }

    // Determine the end of the game
    public void EndGame()
    {
        // End the game
    }

    private int times(int v, float multiplier) {
        return (int)System.Math.Floor(v * multiplier);
    }
}
