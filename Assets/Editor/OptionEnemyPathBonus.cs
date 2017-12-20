using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyPathBonus))]
public class OptionEnemyPathBonus : Editor {

    public override void OnInspectorGUI()
    {
        EnemyPathBonus mytarget = (EnemyPathBonus)target;
        DrawDefaultInspector();

        if(GUILayout.Button("Add new position"))
        {
            mytarget.AddNewPointPath();
        }
        if(GUILayout.Button("Remove last position"))
        {
            mytarget.RemoveLastPointPath();
        }
    }

    public void OnSceneGUI()
    {
        EnemyPathBonus mytarget = (EnemyPathBonus)target;
        if (mytarget.tabPath.Count > 0)
        {
            var paths = mytarget.transform.parent.GetChild(1);
            for (int i = 0; i < paths.childCount - 1; i++)
            {
                /*Vector3 A = paths.GetChild(i).position;
                Vector3 B = paths.GetChild(i + 1).position;
                Vector3 AB = VectorBetween2Vector(A, B);
                Vector3 AI = AB / 2;
                Vector3 I = VectorBetween2Vector(-A, AI);
                Vector3 S = new Vector3(I.x + 2 * Mathf.Cos((Mathf.PI / 2) - Vector3.Angle(VectorBetween2Vector(I, B), Vector3.right)), I.y, I.z + 2 * Mathf.Sin((Mathf.PI / 2) - Vector3.Angle(VectorBetween2Vector(I, B), Vector3.right)));


                Handles.DrawBezier(paths.GetChild(i).position, paths.GetChild(i + 1).position, S, S + AI, Color.red, null, 1);*/

                Handles.DrawBezier(paths.GetChild(i).position, paths.GetChild(i + 1).position, paths.GetChild(i).position, paths.GetChild(i + 1).position, Color.red, null, 3);
            }
        }
    }


    private Vector3 VectorBetween2Vector(Vector3 p_a, Vector3 p_b)
    {
        return p_b - p_a;
    }
}
