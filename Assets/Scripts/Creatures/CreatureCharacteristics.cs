using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCharacteristics : MonoBehaviour
{
    [Header("Stats")]
    public float food;
    public float maxFood = 100f;
    public float water;
    public float maxWater = 100f;
    public float energy;
    public float maxEnergy = 100f;
    public float health;
    public float maxHealth = 100f;
    
    [Header("Characteristics")] 
    public bool herbivore;
    public bool carnivore;

    private void Start()
    {
        food = maxFood;
        water = maxWater;
        energy = maxEnergy;
        health = maxHealth;
    }

    private void Update()
    {
        
    }
}
