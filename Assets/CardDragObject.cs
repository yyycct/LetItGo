using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CardDragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    public Transform parent;
    public bool isAction;
    public Card card;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        parent = transform.parent;
      
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin drag");
        canvasGroup.blocksRaycasts = false;
        transform.parent = transform.root;
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        transform.position = Input.mousePosition;
        //rect.anchoredPosition += eventData.delta / UIManager.instance.gameObject.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("onEnd Drag");
        transform.parent = parent;
        canvasGroup.blocksRaycasts = true;
    }
}
