using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerFarted()
    {
        int count = Mathf.RoundToInt((gameManager.CurrentSmell + gameManager.CurrentSound) / 200f * transform.childCount);
        List<int> listToplay = ChildToPlay(count);
        foreach(int index in listToplay)
        {
            Transform child = transform.GetChild(index);
            NPCAnimationController npcController = child.GetComponent<NPCAnimationController>();
            if (npcController != null)
            {
                npcController.disbelief();
            }
        }
    }
    public void OnPlayerStartedClipping()
    {
        int count = 8;
        List<int> listToplay = ChildToPlay(count);
        foreach (int index in listToplay)
        {
            Transform child = transform.GetChild(index);
            NPCAnimationController npcController = child.GetComponent<NPCAnimationController>();
            if (npcController != null)
            {
                npcController.disbelief();
            }
        }
    }

    public List<int> ChildToPlay(int count)
    {
        List<int> result = new List<int>();
        if (count >= transform.childCount)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                result.Add(i);
            }
        }
        else
        {
            while (result.Count < count)
            {
                int c = Random.Range(0, transform.childCount);
                if (!result.Contains(c))
                {
                    result.Add(c);
                }
            }
            
        }
        return result;
    }
    
}
