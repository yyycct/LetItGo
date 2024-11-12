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
    public int roundSmellDecrease = 10;
    [Tooltip("The multiplier for the unmatch index, fart card is not under the action card")]
    public float unmatchIndexMultiplier = 0.5f;
    // private int fartCardsPerRound = 1;
    // private int environmentCardsPerRound = 1;
    private UIManager uiManager;
    private EnvironmentCard pickedEnvironmentCard = new EnvironmentCard();
    
    private int CurrentGas = 100;
    private int CurrentSmell = 0;
    private int CurrentSound = 0;
    private int round = 0;

    // Game loop:
    // 1. Get one random fart card, one random environment card, and three random action cards
    // 2. Display the cards
    // 3. Player place the cards
    // 4. Calculate the score
    // 5. Repeat
    public void Start()
    {
        uiManager = UIManager.instance;
        ShuffleEnvCards();
        GameLoopSetup();
    }
    
    private void GameLoopSetup()
    {   
        uiManager.showPlayCardPanel();
        // Clean up the cards first
        CleanUp();
        // Draw the cards
        DrawEnvironmentCard();
        DrawFartCard();
        DrawActionCards();
    }

    private void CleanUp()
    {
        // Clean up the cards
        uiManager.CleanUp();

        // Decrease the smell damage every round
        CurrentSmell -= roundSmellDecrease;
        if (CurrentSmell < 0) CurrentSmell = 0;
        uiManager.UpdateSmellSlider(CurrentSmell, 1.0f);
    }

    public void ShuffleEnvCards(){
        // shuffle the environment cards
        for (int i = 0; i < environmentCards.Count; i++)
        {
            EnvironmentObject temp = environmentCards[i];
            int randomIndex = Random.Range(i, environmentCards.Count);
            environmentCards[i] = environmentCards[randomIndex];
            environmentCards[randomIndex] = temp;
        }
    }

    public void DrawEnvironmentCard()
    {
        // Draw a random environment card
        pickedEnvironmentCard = environmentCards[round].card;
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

        // Shuffle the action cards
        for (int i = 0; i < actionCards.Count; i++)
        {
            ActionCardObject temp = actionCards[i];
            int randomIndex = Random.Range(i, actionCards.Count);
            actionCards[i] = actionCards[randomIndex];
            actionCards[randomIndex] = temp;
        }
        // Pick the first three action cards
        for (int i = 0; i < actionCardsPerRound; i++)
        {
            pickedActionCards.Add(actionCards[i].card);

            // Remove one-time-use action cards from the list
            if (actionCards[i].card.oneTimeUse)
            {
                actionCards.Remove(actionCards[i]);
            }
        }
        uiManager.spawnActionCards(pickedActionCards);
    }

    public void CalculateScore(List<ActionCard> acitonCardsPlayed, FartCard fartCardPlayed, int fartCardIndex)
    {
        // Calculate the score
        int fartDamageSound = fartCardPlayed.soundAmount;
        int fartDamageSmell = fartCardPlayed.smellAmount;

        Debug.Log("initial sound/smell damage: " + fartDamageSound + "/" + fartDamageSmell);

        foreach (ActionCard actionCard in acitonCardsPlayed)
        {
            if (actionCard == null) continue;

            int attention = actionCard.Value.attention;
            int smell = actionCard.Value.smell;
            int sound = actionCard.Value.sound;

            // calculate the index matching
            int actionIndex = acitonCardsPlayed.IndexOf(actionCard);

            if (fartCardIndex != actionIndex)
            {
                Debug.Log("attention/sound/smell before unmatch index multiplier: " + attention + "/" + sound + "/" + smell);
                Debug.Log("unmatch index, fart index: " + fartCardIndex + ", action index: " + actionIndex);
                attention = times(attention, unmatchIndexMultiplier);
                smell = times(smell, unmatchIndexMultiplier);
                sound = times(sound, unmatchIndexMultiplier);
                Debug.Log("attention/sound/smell unmatch index multiplier: " + attention + "/" + sound + "/" + smell);
            }

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
            Debug.Log("sound/smell damage after action card: " + fartDamageSound + "/" + fartDamageSmell);
        }

        // finally apply the environment buffs
        float envAttention = pickedEnvironmentCard.AttentionDamageMultiplier;
        float envSound = pickedEnvironmentCard.SoundDamageMultiplier;
        float envSmell = pickedEnvironmentCard.SmellDamageMultiplier;
        bool isInappropriate = false;
        foreach (ActionCard actionCard in acitonCardsPlayed)
        {
            if (actionCard == null) continue;
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
            Debug.Log("Inappropriate environment card");
            Debug.Log("attention/sound/smell inappropriate environment multiplier: " + envAttention + "/" + envSound + "/" + envSmell);
        }

        // calculate the damage
        if (envAttention > 0)
        {
            float attentionMultiplier = 1.0f + (envAttention / 100.0f);
            fartDamageSound = times(fartDamageSound, attentionMultiplier);
            fartDamageSmell = times(fartDamageSmell, attentionMultiplier);
        }
        else {
            if (envSound > 0)
            {
                fartDamageSound = times(fartDamageSound, envSound);
            }
            if (envSmell > 0)
            {
                fartDamageSmell = times(fartDamageSmell, envSmell);
            }
        }
        Debug.Log("sound/smell damage after environment card: " + fartDamageSound + "/" + fartDamageSmell);

        // update the player stats
        CurrentGas -= fartCardPlayed.gasAmount;
        CurrentSound += fartDamageSound;
        CurrentSmell += fartDamageSmell;

        // update the UI
        uiManager.UpdateGasSlider(CurrentGas);
        uiManager.UpdateSmellSlider(CurrentSmell, 1.0f);
        uiManager.UpdateSoundSlider(CurrentSound, 1.0f, () => {
            // check if the game is over
            if (IsEnd())
            {
                // end the game
            }
            else
            {
                // continue the game
                round++;
                GameLoopSetup();
            }
        });
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
            } else {
                actionCardsPlayed.Add(null);
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

        CalculateScore(actionCardsPlayed, fartCardPlayed, fartCardPlayedIndex);
    }

    // Determine the end of the game
    public bool IsEnd()
    {
        if (CurrentGas <= 0 && CurrentSmell + CurrentSound < 100)
        {
            Debug.Log("You WIN!");
            uiManager.hidePlayCardPanel();
            uiManager.ShowWinPanel();
            return true;
        }
        else if (CurrentSmell + CurrentSound >= 100)
        {
            Debug.Log("You LOSE!");
            uiManager.hidePlayCardPanel();
            uiManager.ShowLosePanel();
            return true;
        }
        return false;
    }

    private int times(int v, float multiplier) {
        return (int)System.Math.Floor(v * multiplier);
    }
}
