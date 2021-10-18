using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Sound))]
public class SoundEditor : Editor
{
    private void OnSceneGUI()
    {
        Sound sound = (Sound) target;
        Handles.color = Color.magenta;
        Handles.DrawWireArc(sound.transform.position, Vector3.forward, Vector3.right, 360, sound.transform.localScale.x);
    }
}
