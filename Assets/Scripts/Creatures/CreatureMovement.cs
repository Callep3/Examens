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
    [SerializeField] public float runningModifier = 2f;
    [SerializeField] private CreatureCharacteristics creatureCharacteristics;
    [SerializeField] private Transform sprite;
    
    [Header("Debug")]
    [SerializeField] public Transform target;
    [SerializeField] public Vector3 targetPosition;
#pragma warning disable 108,114
    [SerializeField] private Collider2D collider;
    [SerializeField] private Rigidbody2D rigidbody2D;
#pragma warning restore 108,114

    private float footstepInveterval;
    private float footstepCooldown = 0.3f;

    private void Start()
    {
        if (collider == null)
            collider = GetComponent<Collider2D>();
        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        FootStep();
    }

    private void FootStep()
    {
        if (footstepInveterval > Time.time) return;
        footstepInveterval = Time.time + footstepCooldown;

        var obj = ObjectPooler.Instance.GetPooledObject("Sound");
        var script = obj.GetComponent<Sound>();
        script.lifespan = 1f;
        script.volume = 5f;
        script.hostile = creatureCharacteristics.carnivore;
        script.parent = creatureCharacteristics.creatureTypeName;
        obj.transform.position = gameObject.transform.position;
        obj.SetActive(true);
    }

    private void FixedUpdate()
    {
        MoveCreature();
    }

    private void MoveCreature()
    {
        if (target != null)
            if (Physics2D.Linecast(target.position, transform.position, 1 << 6))
                target = null;

        if (target != null)
            targetPosition = target.position;
        var direction = targetPosition - transform.position;
        //TODO: add an accelerationSpeed topped at the movementSpeed and have it deaccelerate to land on the target position

        rigidbody2D.MovePosition(transform.position + direction.normalized * (movementSpeed * Time.deltaTime));

        RotateSprite();
    }

    public bool CheckProximity()
    {
        if (target != null) targetPosition = target.position;

        return (targetPosition - transform.position).magnitude < minimalTargetProximity;
    }

    public float CheckDistanceToTarget()
    {
        if (target == null) Debug.LogError("No Target");

        return (target.position - gameObject.transform.position).magnitude;
    }

    public void SetTargetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
        CheckPathObstruction();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void LookTowardsPosition(Vector3 newPosition)
    {
        targetPosition = (newPosition - transform.position).normalized * (minimalTargetProximity / 2)
                         + transform.position;
    }

    private void RotateSprite()
    {
        sprite.up = targetPosition - transform.position;
    }

    private void CheckPathObstruction()
    {
        //from transform.position to targetPosition, get colliders with layer "Obstacles"
        var targetsInTheWay = Physics2D.LinecastAll(transform.position, 
            targetPosition,
            1 << 6);

        //If obstacles has been found, calculate the distance to them and set the target position to the shortest distance
        var distanceToTargetPosition = (targetPosition - transform.position).magnitude;
        foreach (var obstacle in targetsInTheWay)
        {
            var distanceToCollision = obstacle.distance - transform.localScale.x / 2;

            if (distanceToCollision < distanceToTargetPosition)
                distanceToTargetPosition = distanceToCollision;
        }

        //Shorten the length of the path to target position to the closest point
        var newPosition = (targetPosition - transform.position).normalized * distanceToTargetPosition + transform.position;

        targetPosition = newPosition;
    }
}
