using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreatureMovement))]
public class PathEditor : Editor
{
    private void OnSceneGUI()
    {
        CreatureMovement creatureMovement = (CreatureMovement) target;
        Handles.color = Color.blue;
        Handles.DrawLine(creatureMovement.transform.position, creatureMovement.targetPosition);
    }
}