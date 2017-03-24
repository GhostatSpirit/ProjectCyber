using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
	AI, Hacker, Virus, Boss, Wall, HackerBullet, None
};


public class ObjectIdentity : MonoBehaviour {

	public ObjectType objType = ObjectType.None;


}
