using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private GameManager gameManager;

    [SerializeField] private GameObject PlayCardPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [SerializeField] private GameObject actionCardInventorySlot;
    [SerializeField] private GameObject fartCardInventorySlot;
    [SerializeField] private GameObject environmentCardUI;
    [SerializeField] private GameObject actionCardsSlotParent;
    [SerializeField] private GameObject fartCardSlotParent;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject gasSlider;

    [SerializeField] private GameObject smellSlider;

    [SerializeField] private GameObject soundSlider;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        // Find GameManager.cs and assign it to gameManager
        gameManager = FindObjectOfType<GameManager>();
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void showPlayCardPanel()
    {
        PlayCardPanel.SetActive(true);
    }

    public void hidePlayCardPanel()
    {
        PlayCardPanel.SetActive(false);
    }

    public void CleanUp()
    {
        // Clean up the cards
        foreach (Transform child in actionCardInventorySlot.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in fartCardInventorySlot.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in actionCardsSlotParent.transform)
        {
            if (child.childCount != 0)
                Destroy(child.GetChild(0).gameObject);
        }
        foreach (Transform child in fartCardSlotParent.transform)
        {
            if (child.childCount != 0)
                Destroy(child.GetChild(0).gameObject);
        }
    }
    public void spawnActionCards(List<ActionCard> actionCards)
    {
        if (actionCards.Count <= 0)
            return;
        
        Debug.Log("Spawning action cards " + actionCards.Count);
        for (int i = 0; i < actionCards.Count; i++)
        {
            GameObject actionCardUI = Instantiate(cardPrefab, actionCardInventorySlot.transform); 
            actionCardUI.GetComponent<CardDragObject>().InitializeCard(actionCards[i]);
        }
    }
    public void spawnFartCards(FartCard fartCard)
    {
        if (fartCard == null)
            return;

        Debug.Log("Spawning fart card");
        GameObject fartCardUI = Instantiate(cardPrefab, fartCardInventorySlot.transform);
        fartCardUI.GetComponent<CardDragObject>().InitializeCard(fartCard);
    }
    public void spawnEnvironmentCard(EnvironmentCard environmentCard)
    {
        if (environmentCard == null)
            return;
    
        Debug.Log("Spawning environment card");
        GameObject text = environmentCardUI.transform.GetChild(0).gameObject;
        text.GetComponent<TextMeshProUGUI>().text = environmentCard.name;
    }

    public void onPlayButtonClicked()
    {
        // Find GameManager.cs and call its onPlayButtonClicked method
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        if (checkifActionSlotFilled() && checkifFartSlotFilled())
        {
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }
            gameManager.OnPlayButtonClicked(actionCardsSlotParent, fartCardSlotParent);
        }
    }

    public void UpdateGasSlider(float value){
        gasSlider.GetComponent<Slider>().value = value/100.0f;
    }

    public void UpdateSmellSlider(float value, float duration, System.Action callback = null)
    {
        StartCoroutine(AnimateSmellSlider(value, duration, callback));
    }

    private IEnumerator AnimateSmellSlider(float targetValue, float duration, System.Action callback)
    {
        RectTransform rectTransform = smellSlider.GetComponent<RectTransform>();
        Vector2 initialSize = rectTransform.sizeDelta;
        Vector2 targetSize = new Vector2(targetValue, initialSize.y);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            rectTransform.sizeDelta = Vector2.Lerp(initialSize, targetSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.sizeDelta = targetSize;

        callback?.Invoke();
    }

    public void UpdateSoundSlider(float value, float duration, System.Action callback = null)
    {
        StartCoroutine(AnimateSoundSlider(value, duration, callback));
    }

    private IEnumerator AnimateSoundSlider(float targetValue, float duration, System.Action callback)
    {
        RectTransform rectTransform = soundSlider.GetComponent<RectTransform>();
        Vector2 initialSize = rectTransform.sizeDelta;
        Vector2 targetSize = new Vector2(targetValue, initialSize.y);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            rectTransform.sizeDelta = Vector2.Lerp(initialSize, targetSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.sizeDelta = targetSize;

        callback?.Invoke();
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }

    private bool checkifActionSlotFilled(){
        // Check if at least one action slots are filled
        for (int i = 0; i < actionCardsSlotParent.transform.childCount; i++)
        {
            if (actionCardsSlotParent.transform.GetChild(i).childCount != 0)
                return true;
        }
        return false;
    }

    private bool checkifFartSlotFilled(){
        // Check if at least one fart slots are filled
        for (int i = 0; i < fartCardSlotParent.transform.childCount; i++)
        {
            if (fartCardSlotParent.transform.GetChild(i).childCount != 0)
                return true;
        }
        return false;
    }
}
