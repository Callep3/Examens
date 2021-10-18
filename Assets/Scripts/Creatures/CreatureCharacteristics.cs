using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreatureCharacteristics : MonoBehaviour
{
    [Header("Stats")] 
    public string creatureTypeName;
    [Space]
    public float food;
    public float maxFood = 100f;
    public float eatingSpeed;
    public float water;
    public float maxWater = 100f;
    public float drinkingSpeed;
    public float energy;
    public float maxEnergy = 100f;
    public float restingSpeed;
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

    #region Food

    public void AddFood(float amount)
    {
        amount = Mathf.Abs(amount);
        food += amount;
        food = Mathf.Clamp(food, 0, maxFood);
    }

    public void RemoveFood(float amount)
    {
        amount = Mathf.Abs(amount);
        food -= amount;
        food = Mathf.Clamp(food, 0, maxFood);
    }

    #endregion

    #region Water

    public void AddWater(float amount)
    {
        amount = Mathf.Abs(amount);
        water += amount;
        water = Mathf.Clamp(water, 0, maxWater);
    }

    public void RemoveWater(float amount)
    {
        amount = Mathf.Abs(amount);
        water -= amount;
        water = Mathf.Clamp(water, 0, maxWater);
    }

    #endregion

    #region Energy

    public void AddEnergy(float amount)
    {
        amount = Mathf.Abs(amount);
        energy += amount;
        energy = Mathf.Clamp(energy, 0, maxEnergy);
    }

    public void RemoveEnergy(float amount)
    {
        amount = Mathf.Abs(amount);
        energy -= amount;
        energy = Mathf.Clamp(energy, 0, maxEnergy);
    }

    #endregion
    
    #region Health

    public void AddHealth(float amount)
    {
        amount = Mathf.Abs(amount);
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public void RemoveHealth(float amount)
    {
        amount = Mathf.Abs(amount);
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    #endregion
}
