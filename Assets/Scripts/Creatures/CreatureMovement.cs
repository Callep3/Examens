using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        targetPosition = newPosition;
        CheckPathObstruction();
    }

    public void LookTowardsPosition(Vector3 newPosition)
    {
        targetPosition = (newPosition - transform.position).normalized * (minimalTargetProximity / 2)
                         + transform.position;
    }
    
    private void CheckPathObstruction()
    {
        //from transform.position to targetPosition, get colliders with layer "Obstacles"
        var targetsInTheWay = Physics2D.CapsuleCastAll(
            transform.position, 
            transform.localScale, 
            CapsuleDirection2D.Vertical, 
            Vector2.Angle(transform.position, targetPosition), 
            targetPosition - transform.position,
            Vector2.Distance(targetPosition, transform.position), 
            1 << 6);
        

        //If obstacles has been found, calculate the distance to them and set the target position to the shortest distance
        var distanceToTargetPosition = (targetPosition - transform.position).magnitude;
        foreach (var obstacle in targetsInTheWay)
        {
            Debug.DrawLine(transform.position, obstacle.point);
            Debug.Log("PathCorrection");
            var distanceToCollision = obstacle.distance;

            if (distanceToCollision < distanceToTargetPosition)
                distanceToTargetPosition = distanceToCollision;
        }

        //Shorten the length of the path to target position to the closest point
        var newPosition = (targetPosition - transform.position).normalized * distanceToTargetPosition + transform.position;

        targetPosition = newPosition;
    }
}
