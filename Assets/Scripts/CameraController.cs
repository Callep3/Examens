using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputAction zoom;
    [SerializeField] private InputAction movement;

    private Vector3 movementVector; 
    private Camera gameCamera;
    
    private void Start()
    {
        gameCamera = GetComponent<Camera>();
        
        zoom.performed += CameraZoom;
        zoom.Enable();

        movement.performed += CameraMovement;
        movement.canceled += CameraMovement;
        movement.Enable();
    }

    private void CameraMovement(InputAction.CallbackContext ctx)
    {
        movementVector = new Vector3(ctx.ReadValue<Vector2>().x * 0.03f, ctx.ReadValue<Vector2>().y * 0.03f);
    }

    private void Update()
    {
        if (movementVector.x != 0 || movementVector.y != 0)
            transform.position += movementVector;
    }

    private void CameraZoom(InputAction.CallbackContext ctx)
    {
        gameCamera.orthographicSize -= ctx.ReadValue<float>() * 0.003f;
        gameCamera.orthographicSize = Mathf.Clamp(gameCamera.orthographicSize, 1f, 15f);
    }
}
