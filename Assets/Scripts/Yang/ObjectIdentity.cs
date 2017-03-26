using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
	AI, Hacker, Virus, Boss, Wall, HackerBullet, Line, None
};


public class ObjectIdentity : MonoBehaviour {

	public ObjectType objType = ObjectType.None;

	// define all controllable objects here
	public static List<ObjectType> controllables = new List<ObjectType>{
		ObjectType.Virus
	};

	// define all obstacles that could block vision here
	public static List<ObjectType> visionBlockers = new List<ObjectType>{
		ObjectType.Wall
	};

	public bool isControllable(){
		return controllables.Contains (this.objType);
	}

	public bool isVisionBlocker(){
		return visionBlockers.Contains (this.objType);
	}

}
