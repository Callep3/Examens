using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public CreatureCharacteristics creatureCharacteristics;
    
    public float volume;
    public float lifespan;
    private float maxSize;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        maxSize = volume;
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
        
        var localScale = transform.localScale;
        var newScale = new Vector3(
            localScale.x + 1f * Time.deltaTime, 
            localScale.y + 1f * Time.deltaTime, 
            localScale.z);

        if (newScale.x > maxSize)
        {
            transform.localScale = Vector3.zero;
            return;
        }
        
        transform.localScale = newScale;
    }
    
    //TODO: Object pool the sound prefab
}
