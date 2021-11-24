using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeControls : MonoBehaviour
{
    [SerializeField] private InputAction increaseSpeed;
    [SerializeField] private InputAction decreaseSpeed;

    private void Start()
    {
        increaseSpeed.performed += IncreaseSpeed;
        increaseSpeed.Enable();

        decreaseSpeed.performed += DecreaseSpeed;
        decreaseSpeed.Enable();
    }

    private void IncreaseSpeed(InputAction.CallbackContext ctx)
    {
        Time.timeScale += 0.25f;
        UIController.Instance.UpdateSpeedText();
    }

    private void DecreaseSpeed(InputAction.CallbackContext ctx)
    {
        Time.timeScale -= 0.25f;
        UIController.Instance.UpdateSpeedText();
    }
}
