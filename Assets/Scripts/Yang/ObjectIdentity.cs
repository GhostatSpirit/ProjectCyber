using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum ObjectType
{
	AI, Hacker, Virus, Boss,
	Wall, Door,
	HackerBullet,
	Line, 
	None,
	Interface,		// interface means an object that allows hacker to control other objects
	Robot, RobotBullet,
	LaserCannon,
    BreakableWall,
	DepthField,		// object enters this field will change its sorting layer accordingly
	Roomba
};


public class ObjectIdentity : MonoBehaviour {

	public ObjectType objType = ObjectType.None;

	// define all controllable objects here
	public static List<ObjectType> controllables = new List<ObjectType>{
		ObjectType.Virus, ObjectType.Interface
	};

	// define all obstacles that could block vision here
	public static List<ObjectType> visionBlockers = new List<ObjectType>{
		ObjectType.Wall, ObjectType.Interface, ObjectType.Door
	};

	public bool isControllable(){
		return controllables.Contains (this.objType);
	}

	public bool isVisionBlocker(){
		return visionBlockers.Contains (this.objType);
	}

}
