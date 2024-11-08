using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    public bool isAction = true;
    public bool isInventory = false;
    public GameObject inventoryParent;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (transform.childCount == 0||isInventory)
        {
            GameObject cardDropped = eventData.pointerDrag;      
            CardDragObject cardDragObject = cardDropped.GetComponent<CardDragObject>();
            if(cardDragObject.isAction == isAction)
            {
                if (isInventory)
                {
                    cardDragObject.parent = inventoryParent.transform;
                }
                else
                {
                    cardDragObject.parent = transform;
                }
                
            }
        }
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
}
