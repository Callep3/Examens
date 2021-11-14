using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Gender
{
    male,
    female
}

public enum CreatureStats
{
    none,
    food,
    water,
    energy,
    health
}

public class CreatureCharacteristics : MonoBehaviour
{
    [Header("Stats")] 
    public string creatureTypeName;
    public float baseThreatLevel;
    public Gender gender;
    [Header("Food")]
    public float food;
    public float maxFood = 100f;
    public float eatingSpeed;
    public float hungerRate;
    [Header("Water")]
    public float water;
    public float maxWater = 100f;
    public float drinkingSpeed;
    public float thirstRate;
    [Header("Energy")]
    public float energy;
    public float maxEnergy = 100f;
    public float restingSpeed;
    public float drainRate;
    [Header("Health")]
    public float health;
    public float maxHealth = 100f;

    [Header("Characteristics")] 
    public bool herbivore;
    public bool carnivore;
    public float hearingSensitivity;

    private void Start()
    {
        food = maxFood;
        water = maxWater;
        energy = maxEnergy;
        health = maxHealth;
    }
    
    //TODO: Add a bool return function for deciding which threat is greater

    public CreatureStats GetLowestStat()
    {
        if (food / maxFood < water / maxWater && 
            food / maxFood < energy / maxEnergy &&
            food / maxFood < health / maxHealth)
        {
            return CreatureStats.food;
        }

        if (water / maxWater < food / maxFood && 
            water / maxWater < energy / maxEnergy && 
            water / maxWater < health / maxHealth)
        {
            return CreatureStats.water;
        }
        
        if (energy / maxEnergy < food / maxFood && 
            energy / maxEnergy < water / maxWater && 
            energy / maxEnergy < health / maxHealth)
        {
            return CreatureStats.energy;
        }
        
        if (health / maxHealth < food / maxFood && 
            health / maxHealth < water / maxWater && 
            health / maxHealth < energy / maxEnergy)
        {
            return CreatureStats.health;
        }

        return CreatureStats.none;
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
