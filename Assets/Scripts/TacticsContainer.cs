using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Tactics Container", menuName = "Containers/Tactics Container", order = 1)]

public class TacticsContainer : ScriptableObject
{
    public List<TacticClass> AllTactics ;
}
[System.Serializable]
public class TacticClass
{
    public List<Vector3> allPos = new List<Vector3>();
}

