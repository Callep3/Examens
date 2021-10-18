using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public CreatureCharacteristics creatureCharacteristics;
    
    public float volume;
    public float lifespan;

    private void OnEnable()
    {
        if (volume <= 0)
            Debug.LogError("Volume is not set or is either negative or zero");
        if (lifespan <= 0)
            Debug.LogError("Lifespan is not set or is either negative or zero");
        if (creatureCharacteristics == null)
            Debug.LogError("No referenced creatures characteristics found");
    }

    private void Update()
    {
        //Expand by set amount over its designated lifespan
        if (lifespan + Time.time < Time.time)
        {
            gameObject.SetActive(false);
            return;
        }
        lifespan -= Time.deltaTime;
    }
    
    //TODO: Object pool the sound prefab
}
