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

	public Controller initialController = Controller.Boss;

	private Controller m_controller = Controller.Boss;

	public Transform Boss;
	public Transform Hacker;


	[HideInInspector] public Controller controller{
		get{
			return m_controller;
		}
		set{
			Controller oldController = m_controller;
			Controller newController = value;
			// trigger an event accordingly 
			// if the old & new controllers matches certain combination
			if(oldController == Controller.Boss && newController == Controller.None){
				if (OnCutByPlayer != null) {
					OnCutByPlayer (this.transform);
				}
			}
			else if(oldController == Controller.Hacker && newController == Controller.None){
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
			else if(oldController == Controller.Hacker && newController == Controller.Boss){
				if (OnCutByEnemy != null) {
					OnCutByEnemy (this.transform);
				}
				if (OnLinkedByEnemy != null) {
					OnLinkedByEnemy (this.transform);
				}
			}
			else if(oldController == Controller.Boss && newController == Controller.Hacker){
				if (OnCutByPlayer != null) {
					OnCutByPlayer (this.transform);
				}
				if (OnLinkedByPlayer != null) {
					OnLinkedByPlayer (this.transform);
				}
			}
			// reset the actions and change the controller value
			m_controller = value;
		}
	}

	// events for triggering different behaviors when the controller changes
	public event Action<Transform> OnCutByPlayer;	 // transform -> controllable object's transform
	public event Action<Transform> OnCutByEnemy;
	public event Action<Transform> OnLinkedByPlayer;
	public event Action<Transform> OnLinkedByEnemy;



    void Start()
    {
		m_controller = initialController;
		controller = m_controller;
    }

	void ResetActionsToNull(){
		OnCutByPlayer = null;
		OnCutByEnemy = null;
		OnLinkedByPlayer = null;
		OnLinkedByEnemy = null;
	}


}
