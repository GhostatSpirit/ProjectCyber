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

	// events for triggering different behaviors when the controller changes
	public event Action<Transform> OnCutByPlayer;	 // transform -> controllable object's transform
	public event Action<Transform> OnCutByEnemy;
	public event Action<Transform> OnLinkedByPlayer;
	public event Action<Transform> OnLinkedByEnemy;

	// StopMovement: constants
	public float changeDragFactor = 100f;


    void Start()
    {
		controller = m_controller;

    }

	void ResetActions(){
		
		OnCutByPlayer = null;
		OnCutByPlayer += ChaseNone;
		OnCutByPlayer += StopMovement;

		OnCutByEnemy = null;
		OnCutByEnemy += ChaseNone;
		OnCutByEnemy += StopMovement;

		OnLinkedByPlayer = null;
		OnCutByEnemy += ChaseBoss;
		OnLinkedByPlayer += StartMovement;

		OnLinkedByEnemy = null;
		OnCutByEnemy += ChasePlayer;
		OnLinkedByEnemy += StartMovement;

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
		Rigidbody2D myRigidbody2D = this.transform.GetComponent<Rigidbody2D> ();
		if(myRigidbody2D != null){
			// increase my drag
			myRigidbody2D.angularDrag *= changeDragFactor;
			myRigidbody2D.drag *= changeDragFactor;
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
		Rigidbody2D myRigidbody2D = this.transform.GetComponent<Rigidbody2D> ();
		if(myRigidbody2D != null){
			// increase my drag
			myRigidbody2D.angularDrag /= changeDragFactor;
			myRigidbody2D.drag /= changeDragFactor;
		}
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

    
}
