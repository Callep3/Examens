using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHearing : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private LayerMask targetMask;
    [SerializeField] public float hearingRadius;
    
    public List<Transform> heardTargets = new List<Transform>();

    private void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }
    
    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindHeardTargets();
        }
    }
    
    private void FindHeardTargets()
    {
        heardTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, hearingRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            heardTargets.Add(target);
        }
    }
}
