using System;
using UnityEngine;
using UnityEngine.Events;

public class CreatureMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float accelerationSpeed = 1f;
    [SerializeField] private float decelerationSpeed = 1f;
    [SerializeField] private float minimalTargetProximity = 0.1f;
    
    [Header("Debug")]
    [SerializeField] private Transform target;
    [SerializeField] public Vector3 targetPosition;
#pragma warning disable 108,114
    [SerializeField] private Collider2D collider;
    [SerializeField] private Rigidbody2D rigidbody2D;
#pragma warning restore 108,114
    public static event Action<CreatureMovement> reachedTargetPosition;

    private void Start()
    {
        if (collider == null)
            collider = GetComponent<Collider2D>();
        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        CheckProximity();
    }

    private void CheckProximity()
    {
        if (target != null) targetPosition = target.position;

        if ((targetPosition - transform.position).magnitude < minimalTargetProximity)
        {
            reachedTargetPosition?.Invoke(this);
            return;
        }

        MoveCreature();
    }

    private void MoveCreature()
    {
        var direction = targetPosition - transform.position;
        //TODO: add an accelerationSpeed topped at the movementSpeed and have it deaccelerate to land on the target position

        rigidbody2D.MovePosition(transform.position + direction.normalized * (movementSpeed * Time.deltaTime));
    }

    public void SetTargetPosition(Vector3 newPosition)
    {
        //Set target position to new position
        //Make sure the path to the target position doesn't collide with an obstacle, nor is too close to the obstacle
    }
}
