using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;


public class ExplosionDamage : MonoBehaviour {
	public LayerMask layerMask = Physics2D.AllLayers;
	public float radius = 0.6f;
	public Vector2 offset = new Vector2(0f, 0f);

	public float magnitude = 100f;

	public event Action<Transform> OnExplosionEnd;

	[Range(0f, 5f)]
	public float delay = 0f;
	[Range(0f, 10f)]
	public float duration = 0.8f;

	public TypeDamagePair[] DamageOtherList;
	Dictionary<ObjectType, float> DamageOtherDict = new Dictionary<ObjectType, float>();

	CircleCollider2D explosionArea;

	ContactFilter2D raycastFilter;

	// Use this for initialization
	IEnumerator Start () {
		explosionArea = GetComponent<CircleCollider2D> ();

		foreach (TypeDamagePair pair in DamageOtherList){
			DamageOtherDict.Add (pair.type, pair.damage);
		}
		
		raycastFilter = new ContactFilter2D ();
		raycastFilter.useTriggers = false;
		raycastFilter.useLayerMask = false;
		raycastFilter.useDepth = false;
		raycastFilter.useNormalAngle = false;

		yield return new WaitForSeconds (delay);
		StartCoroutine (EndExplosionIE ());
	}

	IEnumerator EndExplosionIE(){
		yield return new WaitForSeconds (duration);
		if(OnExplosionEnd != null){
			OnExplosionEnd (this.transform);
		}
		enabled = false;
	}

	void FixedUpdate(){
		Vector3 localOffset = new Vector3 (explosionArea.offset.x, explosionArea.offset.y, 0f);
		float localRadius = explosionArea.radius;
		Vector3 localRadiusVec = new Vector3 (localRadius, 0f, 0f);

		Vector3 globalOffset = transform.TransformVector (localOffset);
		float globalRadius = transform.TransformVector (localRadiusVec).x;

		Vector3 explosionCenter = transform.position + globalOffset;


		Collider2D[] hitColliders = 
			Physics2D.OverlapCircleAll (explosionCenter, globalRadius, layerMask);
		if (hitColliders.Length != 0) {
			// we actually hit something here
			foreach (Collider2D coll in hitColliders) {
				Transform target = coll.transform;
				// if the target has a dynamic rigidbody
				if(HasVisionBlock(target)){
					continue;
				}

				Rigidbody2D body = coll.transform.GetComponent<Rigidbody2D> ();
				if(body && body.bodyType == RigidbodyType2D.Dynamic){
					Vector3 forceDir = (target.position - explosionCenter).normalized;
					body.AddForce (forceDir * magnitude * Time.fixedDeltaTime, ForceMode2D.Impulse);
				}

				ObjectIdentity oi = target.GetComponent<ObjectIdentity> ();
				if(oi){
					float damage;
					if(DamageOtherDict.TryGetValue(oi.objType, out damage)){
//						Debug.Log (oi);
//						Debug.Log (damage);
						HealthSystem hs = coll.transform.GetComponent<HealthSystem> ();
						if(hs){
							if(hs.hasImmunuePeriod)
								hs.Damage (damage);
							else{
								hs.Damage (damage * 60f * Time.fixedDeltaTime);
							}
						}
					}
				}
			}
		}

	}

	// check if there is a vison blocker between the target and this object
	bool HasVisionBlock(Transform target){
		float dist = Vector2.Distance (transform.position, target.position);
		Vector2 dir = target.transform.position - transform.position;
		dir.Normalize ();

		// assume at most 10 objects would in between
		RaycastHit2D[] hits = new RaycastHit2D[10];	
		Physics2D.Raycast
			(transform.position, dir, raycastFilter, hits, dist);

		foreach(RaycastHit2D hit in hits){
			Transform hitTrans = hit.transform;
			if(hitTrans == target || hitTrans == null){
				continue;
			}

			if(hit.collider &&
				hit.collider.gameObject.layer == LayerMask.NameToLayer("HighWall")){
				return true;
			}

			// defined vision blocker
			ObjectIdentity oi = hitTrans.GetComponent<ObjectIdentity> ();
			if (oi && oi.isVisionBlocker()) {
				return true;
			}
			// defined polynavobstacle
			PolyNavObstacle obstacle = hitTrans.GetComponent<PolyNavObstacle> ();
			if(obstacle){
				return true;
			}

		}
		return false;
	}


	public float nodeSize = 7.5f;

	public bool Equals(LaserNode other) {
		return this.GetInstanceID() == other.GetInstanceID();
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Vector3 tempOffset = new Vector3 (offset.x, offset.y, 0f);
		Vector3 explosionCenter = transform.position + tempOffset;
		Gizmos.DrawCube(explosionCenter, (Vector3.one * GetGizmoSize(transform.position)));
	}

	public float GetGizmoSize(Vector3 position)
	{
		Camera current = Camera.current;
		position = Gizmos.matrix.MultiplyPoint(position);

		if (current)
		{
			Transform transform = current.transform;
			Vector3 position2 = transform.position;
			float z = Vector3.Dot(position - position2, transform.TransformDirection(new Vector3(0f, 0f, 1f)));
			Vector3 a = current.WorldToScreenPoint(position2 + transform.TransformDirection(new Vector3(0f, 0f, z)));
			Vector3 b = current.WorldToScreenPoint(position2 + transform.TransformDirection(new Vector3(1f, 0f, z)));
			float magnitude = (a - b).magnitude;
			return nodeSize / Mathf.Max(magnitude, 0.0001f);
		}

		return nodeSize / 4f;
	}
}

