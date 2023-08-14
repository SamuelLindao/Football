using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class SetTactics : EditorWindow
{
    public GameObject Gk;
    public GameObject DefenderOne;
    public GameObject DefenderTwo;
    public GameObject AttackOne;
    public GameObject AttackTwo;
    public GameObject ball;

    public TacticsContainer container;
    [MenuItem("Tactics/SetTactics")]
    public static void Open()
    {
        GetWindow<SetTactics>("Tactics");
    }
    private void OnGUI()
    {
        Gk = (GameObject)EditorGUILayout.ObjectField(Gk, typeof(GameObject), true);
        DefenderOne = (GameObject)EditorGUILayout.ObjectField(DefenderOne, typeof(GameObject), true);
        DefenderTwo = (GameObject)EditorGUILayout.ObjectField(DefenderTwo, typeof(GameObject), true);
        AttackOne = (GameObject)EditorGUILayout.ObjectField(AttackOne, typeof(GameObject), true);
        AttackTwo = (GameObject)EditorGUILayout.ObjectField(AttackTwo, typeof(GameObject), true);
        ball = (GameObject)EditorGUILayout.ObjectField(ball, typeof(GameObject), true);
        container = (TacticsContainer)EditorGUILayout.ObjectField(container, typeof(TacticsContainer), true);
        if(GUILayout.Button("Set All"))
        {
            container.AllTactics[0].allPos.Add(Gk.transform.position - ball.transform.position);
            container.AllTactics[0].allPos.Add(DefenderOne.transform.position - ball.transform.position);
            container.AllTactics[0].allPos.Add(DefenderTwo.transform.position - ball.transform.position);
            container.AllTactics[0].allPos.Add(AttackOne.transform.position - ball.transform.position);
            container.AllTactics[0].allPos.Add(AttackTwo.transform.position - ball.transform.position);
        }
    }
}
