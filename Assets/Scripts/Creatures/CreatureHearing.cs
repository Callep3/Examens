using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHearing : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private LayerMask targetMask;
    [SerializeField] public float hearingRadius;
    [SerializeField] private CreatureCharacteristics creatureCharacteristics;
    
    public List<Transform> heardTargets = new List<Transform>();

    public float checkInterval;
    public float checkCooldown = 0.2f;
    public List<Transform> heardHostileTargets = new List<Transform>();

    private void Start()
    {
        if (creatureCharacteristics == null)
            creatureCharacteristics = GetComponent<CreatureCharacteristics>();
    }

    private void OnEnable()
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
        heardHostileTargets.Clear();
        Collider2D[] targetsInHearingRadius = Physics2D.OverlapCircleAll(transform.position, hearingRadius, targetMask);

        for (int i = 0; i < targetsInHearingRadius.Length; i++)
        {
            Transform target = targetsInHearingRadius[i].transform;
            var distanceToSound = (target.transform.position - transform.position).magnitude;
            var volume = target.localScale.x;

            if (volume / distanceToSound > creatureCharacteristics.hearingSensitivity)
                heardTargets.Add(target);

            if (!heardTargets.Contains(target)) continue;
            var sound = target.GetComponent<Sound>();
            if (!sound.hostile) continue;
            if (sound.parent == creatureCharacteristics.creatureTypeName) continue;
            
            heardHostileTargets.Add(target);
        }
    }
}
