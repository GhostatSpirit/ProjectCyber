using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSwitcher : MonoBehaviour {
	public LayerMask enemyLayer;
	public LayerMask friendLayer;

	ControlStatus cs;

	// Use this for initialization
	void Start () {
		cs = GetComponent<ControlStatus> ();

		cs.OnLinkedByPlayer += ChangeLayerToFriend;
		cs.OnCutByEnemy += ChangeLayerToEnemy;
		//cs.OnLinkedByEnemy += ChangeLayerToEnemy;
	}
	
	// Update is called once per frame
	void Update () {
		
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
		return layerNumber - 1;
	}
}
