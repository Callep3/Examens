using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node
{
	public List<Edge> edgelist = new List<Edge>();
	public Node path = null;
	public GameObject id;
	public float xPos;
	public float yPos;
	public float zPos;
	public float f, g, h;
	public Node cameFrom;
	public Waypoint.waypointType WaypointType;
	
	public Node(Waypoint wp)
	{
		id = wp.gameObject;
		xPos = wp.gameObject.transform.position.x;
		yPos = wp.gameObject.transform.position.y;
		zPos = wp.gameObject.transform.position.z;
		path = null;
		WaypointType = wp.type;
	}
	
	public GameObject getId()
	{
		return id;	
	}

	public Waypoint.waypointType getType()
	{
		return WaypointType;
	}
	
}
