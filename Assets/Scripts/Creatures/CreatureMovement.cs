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
        //TODO Fix this mess asap
        targetPosition = newPosition;
        //Set target position to new position
        //Make sure the path to the target position doesn't collide with an obstacle, nor is too close to the obstacle
        CheckPathObstruction();
    }
    
    private void CheckPathObstruction()
    {
        //from transform.position to targetPosition, get colliders with layer "Obstacles"
        //If obstacles has been found, calculate the distance to them and set the target position to the shortest distance
        //Check if the obstacle is too close to the targetPosition, if it is, move it away from the obstacle and check again
        
        
        /*
        var distance = Vector2.Distance(gameObject.transform.position, creatureMovement.targetPosition);
        var targetsInTheWay = Physics2D.RaycastAll(gameObject.transform.position,
            creatureMovement.targetPosition - gameObject.transform.position,
            distance, creatureSight.obstacleMask);
        
        //Save the entire length of the vector to a default variable "distanceToCollision"
        //Check if any colliding distance from origin is shorter than the default variable
        //Set the target position to the shortest distance
        
        var distanceToCollision = (creatureMovement.targetPosition - gameObject.transform.position).magnitude;
        for (int i = 0; i < targetsInTheWay.Length; i++)
        {
            var tempDistanceToCollision = targetsInTheWay[i].distance;
            Debug.Log(tempDistanceToCollision);

            if (tempDistanceToCollision < distanceToCollision)
                distanceToCollision = tempDistanceToCollision;
        }

        Debug.Log($"distanceToCollision {distanceToCollision}");
        Debug.Log($"distanceToMove {(creatureMovement.targetPosition - gameObject.transform.position).magnitude}");

        if (distanceToCollision >= (creatureMovement.targetPosition - gameObject.transform.position).magnitude)
        {
            Debug.Log("No obstruction");
            return;
        }
        
        creatureMovement.targetPosition = (creatureMovement.targetPosition - gameObject.transform.position).normalized 
            * Mathf.Clamp(distanceToCollision - gameObject.transform.localScale.x * 0.75f, 
                0, 
                int.MaxValue) + gameObject.transform.position;
        Debug.Log($"Vector Length {(creatureMovement.targetPosition - gameObject.transform.position).magnitude}");
        */
    }
}
