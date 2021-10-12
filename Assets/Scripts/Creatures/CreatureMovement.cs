using UnityEngine;

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
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Collider2D collider;

    private void Start()
    {
        if (collider == null)
            collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        CheckProximity();
    }

    private void CheckProximity()
    {
        if (target != null) targetPosition = target.position;
        
        if ((targetPosition - transform.position).magnitude < minimalTargetProximity) return;

        MoveCreature();
    }

    private void MoveCreature()
    {
        var direction = targetPosition - transform.position;
        //TODO: make the object move forward and have it turn according to the rotation speed
        //TODO: add a accelerationSpeed topped at the movementSpeed and have it deaccelerate to land on the target position

        transform.position += direction.normalized * (movementSpeed * Time.deltaTime);
    }
}
