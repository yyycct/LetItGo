using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentCard", menuName = "Environment Card", order = 3)]
public class EnvironmentObject : ScriptableObject
{
    [SerializeField]
    public EnvironmentCard card;
}
