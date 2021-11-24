using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Link
{
    public enum direction { UNI, BI };
    public GameObject node1;
    public GameObject node2;
    public direction dir;
}

[Serializable]
public struct Waypoint
{
    public enum waypointType { DefaultSpot, GrazingSpot, DrinkingSpot, NestingSpot, CarnivoreNestingSpot };
    public GameObject gameObject;
    public waypointType type;
}

public class WaypointsController : MonoBehaviour
{
    public static WaypointsController Instance;
    public Waypoint[] waypoints;
    public Link[] links;
    public Graph graph = new Graph();
    

    private void Awake()
    {
        Instance = this;
        
        if (waypoints.Length <= 0) return;
        
        foreach (Waypoint waypoint in waypoints)
        {
            graph.AddNode(waypoint);
        }

        foreach (Link link in links)
        {
            graph.AddEdge(link.node1, link.node2);
            if (link.dir == Link.direction.BI)
                graph.AddEdge(link.node2, link.node1);
        }
    }

    private void Update()
    {
        graph.debugDraw();
    }
}
