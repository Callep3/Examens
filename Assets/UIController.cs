using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    
    [SerializeField] private TMP_Text carnivoreText;
    [SerializeField] private TMP_Text herbivoreText;
    [SerializeField] private TMP_Text speedText;

    private int numberOfCarnivores;
    private int numberOfHerbivores;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateCarnivoreText();
        UpdateHerbivoreText();
        UpdateSpeedText();
    }

    private void UpdateCarnivoreText()
    {
        carnivoreText.text = $"Carnivores: {numberOfCarnivores}";
    }

    private void UpdateHerbivoreText()
    {
        herbivoreText.text = $"Herbivores: {numberOfHerbivores}";
    }

    public void UpdateSpeedText()
    {
        speedText.text = $"Speed: {Time.timeScale}x";
    }

    public void ChangeNumberOfCarnivores(int delta)
    {
        numberOfCarnivores += delta;
        UpdateCarnivoreText();
    }

    public void ChangeNumberOfHerbivores(int delta)
    {
        numberOfHerbivores += delta;
        UpdateHerbivoreText();
    }
}
