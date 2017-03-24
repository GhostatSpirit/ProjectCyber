using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

/* Controller enum definition - could be used everywhere
 * This enum specifies all the possible controllers of a controllable object
 */
public enum Controller { Boss, None, Hacker, Destroyer };


public class ControlStatus : MonoBehaviour {


	private Controller m_controller = Controller.Boss;


	[HideInInspector] public Controller controller{
		get{
			return m_controller;
		}
		set{
			Controller oldController = controller;
			Controller newController = value;
			// trigger an event accordingly 
			// if the old & new controllers matches certain combination
			if(oldController == Controller.Boss && newController == Controller.None){
				if (OnCutByPlayer != null) {
					OnCutByPlayer (this.transform);
				}
			}
			else if(oldController == Controller.Hacker && newController == Controller.Boss){
				if (OnCutByEnemy != null) {
					OnCutByEnemy (this.transform);
				}
			}
			else if(oldController == Controller.None && newController == Controller.Hacker){
				if (OnLinkedByPlayer != null) {
					OnLinkedByPlayer (this.transform);
				}
			}
			else if(oldController == Controller.None && newController == Controller.Boss){
				if (OnLinkedByEnemy != null) {
					OnLinkedByEnemy (this.transform);
				}
			}
			// reset the actions and change the controller value
			ResetActions ();
			m_controller = value;
		}
	}
    
    public GameObject Boss;
    public GameObject Hacker;

	public LayerMask enemyLayer;
	public LayerMask friendLayer;

	// events for triggering different behaviors when the controller changes
	public event Action<Transform> OnCutByPlayer;	 // transform -> controllable object's transform
	public event Action<Transform> OnCutByEnemy;
	public event Action<Transform> OnLinkedByPlayer;
	public event Action<Transform> OnLinkedByEnemy;


	// read only controller transform
	// if controlled by hacker, return hacker transform
	// if controlled by boss, return boss transform
	// if controlled by none, return null
	[HideInInspector]public Transform controllerTransfrom{
		get{
			switch(this.controller){
			case(Controller.Hacker):
				return Hacker.transform;
			case(Controller.Boss):
				return Boss.transform;
			default:
				return null;
			}
		}
	}

	// StopMovement: constants
	Rigidbody2D myRigidbody2D;
	public float changeDragFactor = 100f;
	float oldDrag;
	float increasedDrag{
		get{
			float originalFactor = (oldDrag == 0f) ? 1f : oldDrag;
			return originalFactor * changeDragFactor;
		}
	}
	float oldAngularDrag;
	float increasedAngularDrag{
		get{
			float originalFactor = (oldAngularDrag == 0f) ? 1f : oldAngularDrag;
			return originalFactor  * changeDragFactor;
		}
	}

    void Start()
    {
		myRigidbody2D = this.transform.GetComponent<Rigidbody2D> ();

		controller = m_controller;
		oldDrag = (myRigidbody2D != null) ? myRigidbody2D.drag : 0f;
		oldAngularDrag = (myRigidbody2D != null) ? myRigidbody2D.angularDrag : 0f;
    }

	void ResetActions(){
		
		OnCutByPlayer = null;
		OnCutByPlayer += ChaseNone;
		OnCutByPlayer += StopMovement;

		OnCutByEnemy = null;
		OnCutByEnemy += ChaseNone;
		OnCutByEnemy += StopMovement;

		OnLinkedByPlayer = null;
		OnLinkedByPlayer += ChaseBoss;
		OnLinkedByPlayer += StartMovement;
		OnLinkedByPlayer += ChangeLayerToFriend;

		OnLinkedByEnemy = null;
		OnLinkedByEnemy += ChasePlayer;
		OnLinkedByEnemy += StartMovement;
		OnLinkedByEnemy += ChangeLayerToEnemy;
	}

	void ResetActionsToNull(){
		OnCutByPlayer = null;
		OnCutByEnemy = null;
		OnLinkedByPlayer = null;
		OnLinkedByEnemy = null;
	}

	// methods for the events above

	/* StopMovement:
	 * 1. stop the enemy from chasing the target
	 * 2. increase linear drag and angular drag
	 */
	void StopMovement(Transform objTrans){
		ChaseTarget ct = this.transform.GetComponent<ChaseTarget> ();
		if(ct != null){
			ct.enabled = false;
		}
		if(myRigidbody2D != null){
			// increase my drag
			myRigidbody2D.angularDrag = increasedDrag;
			myRigidbody2D.drag = increasedAngularDrag;
		}
	}

	/* StartMovement:
	 * 1. start the enemy from chasing the target again
	 * 2. reset the linear/angular drag
	 */
	void StartMovement(Transform objTrans){
		ChaseTarget ct = this.transform.GetComponent<ChaseTarget> ();
		if(ct != null){
			ct.enabled = true;
		}
		if(myRigidbody2D != null){
			// increase my drag
			myRigidbody2D.angularDrag = oldDrag;
			myRigidbody2D.drag = oldAngularDrag;
		}
	}



	/* Paralyze
	 * stop the enemy from chasing the target 
	 * in a given amount of time (seconds)
	 */
	public void Paralyze(float time){
		// first stop this object
		StopMovement (this.transform);
		// then start this object after a given amont of time
		StartCoroutine (StartMoveIE (this.transform, time));
	}
	// IEnumerator for invoking a function with parameters
	IEnumerator StartMoveIE(Transform trans, float delay){
		yield return new WaitForSeconds (delay);
		StartMovement (trans);
	}


	/* ChasePlayer:
	 * set the targetGroup as "Player" in ChaseTarget
	 */
	void ChasePlayer(Transform objTrans){
		VirusTargetPicker ct = this.transform.GetComponent<VirusTargetPicker> ();
		if(ct != null){
			ct.targetGroup = VirusTargetPicker.Target.Player;
		}
	}

	/* ChaseNone:
	 * set the targetGroup as "None" in ChaseTarget
	 */
	void ChaseNone(Transform objTrans){
		VirusTargetPicker ct = this.transform.GetComponent<VirusTargetPicker> ();
		if(ct != null){
			ct.targetGroup = VirusTargetPicker.Target.None;
		}
	}

	/* ChaseBoss:
	 * set the targetGroup as "Boss" in ChaseTarget
	 */
	void ChaseBoss(Transform objTrans){
		VirusTargetPicker ct = this.transform.GetComponent<VirusTargetPicker> ();
		if(ct != null){
			ct.targetGroup = VirusTargetPicker.Target.Boss;
		}
	}

	void ChangeLayerToFriend(Transform objTrans){
//		Debug.Log (friendLayer.value);
		this.gameObject.layer = LayerMaskToLayerNum(friendLayer);

	}

	void ChangeLayerToEnemy(Transform objTrans){
		this.gameObject.layer = LayerMaskToLayerNum(enemyLayer);
	}

	// translate layerMask value into layer number [0 - 31]
	int LayerMaskToLayerNum(LayerMask layerMask){
		int layerNumber = 0;
		int layer = layerMask.value;
		while(layer > 0){
			layer = layer >> 1;
			layerNumber++;
		}
		return layerNumber;
	}
}
