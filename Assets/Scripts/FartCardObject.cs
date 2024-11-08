using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FartCard", menuName = "Fart Card", order = 2)]
public class FartCardObject : ScriptableObject
{
    [SerializeField]
    public FartCard card;
}
