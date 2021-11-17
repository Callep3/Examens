using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSight : MonoBehaviour
{
    [Header("Settings")] 
    public LayerMask obstacleMask;
    public LayerMask targetMask;
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    
    public List<Transform> visibleTargets = new List<Transform>();

    [Header("Debug")]
    public CreatureMovement creatureMovement;
    public bool sleeping;

    private void Start()
    {
        if (creatureMovement == null)
            creatureMovement = GetComponent<CreatureMovement>();
        
        StartCoroutine(FindTargetsWithDelay(.2f));
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        visibleTargets.Clear();
        if (sleeping) return;
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(creatureMovement.targetPosition - transform.position, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }
    
    public Vector2 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees -= transform.eulerAngles.z;
        }

        if (creatureMovement.targetPosition.x - transform.position.x > 0)
            angleInDegrees += Vector2.Angle(creatureMovement.targetPosition - transform.position, transform.up);
        else if (creatureMovement.targetPosition.x - transform.position.x < 0)
            angleInDegrees -= Vector2.Angle(creatureMovement.targetPosition - transform.position, transform.up);

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
