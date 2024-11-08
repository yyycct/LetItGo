using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionCard", menuName = "Action Card", order = 1)]
public class ActionCardObject : ScriptableObject
{
    [SerializeField]
    public ActionCard card;
}


