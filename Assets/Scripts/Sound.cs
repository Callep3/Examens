using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public CreatureCharacteristics creatureCharacteristics;
    
    public float volume;
    public float lifespan;

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
}
