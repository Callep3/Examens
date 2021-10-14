using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreatureHearing))]
public class FieldOfHearingEditor : Editor
{
    private void OnSceneGUI()
    {
        CreatureHearing foh = (CreatureHearing) target;
        Handles.color = Color.green;
        Handles.DrawWireArc(foh.transform.position, Vector3.forward, Vector3.right, 360, foh.hearingRadius);

        Handles.color = Color.green;
        foreach (Transform visibleTarget in foh.heardTargets)
        {
            Handles.DrawLine(foh.transform.position, visibleTarget.position);
        }
    }
}
