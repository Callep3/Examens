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

public class WaypointsController : MonoBehaviour
{
    public GameObject[] waypoints;
    public Link[] links;
    public Graph graph = new Graph();

    void Start()
    {
        if (waypoints.Length > 0)
        {
            foreach (GameObject waypoint in waypoints)
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
    }

    // Update is called once per frame
    void Update()
    {
        graph.debugDraw();
    }
}
