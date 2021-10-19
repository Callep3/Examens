using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public CreatureCharacteristics creatureCharacteristics;
    
    [Tooltip("Volume equals the amount of unity units from the creature the sound will be heard when the creature has a default sensitivity of 1")]
    public float volume;
    public float lifespan;

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        transform.localScale *= volume;
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
}
