using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability Container", menuName = "Containers/Ability Container", order = 1)]
public class AbilityContainer : ScriptableObject
{
    public List<AbilityInfo> myAbilitys;
}

[System.Serializable]
public class AbilityInfo
{
    public string AbilityName;
    public int AbilityMax;
    public GameObject ActionObj;
}

