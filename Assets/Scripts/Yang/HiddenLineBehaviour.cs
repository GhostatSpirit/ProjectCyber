﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenLineBehaviour : MonoBehaviour {
	public Transform statusReceiver;
	public Transform statusAssigner;

	public Material enemyLineMaterial;
	public Material playerLineMaterial;

	public bool statusAffects = true;

	public string sortingLayerName = "ControlLine";

	ControlStatus _assignerCS;
	ControlStatus assignerCS{
		get{
			if (!statusAssigner)
				return null;
			if (!_assignerCS)
				_assignerCS = statusAssigner.GetComponentInChildren<ControlStatus> ();
			return _assignerCS;
		}
	}
	ControlStatus _receiverCS;
	ControlStatus receiverCS{
		get{
			if (!statusReceiver)
				return null;
			if (!_receiverCS)
				_receiverCS = statusReceiver.GetComponentInChildren<ControlStatus> ();
			return _receiverCS;
		}
	}

	LineRenderer lr;

	List<Transform> nodeTransforms;
	Vector3[] nodePositions;
	// Use this for initialization
	IEnumerator Start () {
		lr = GetComponent<LineRenderer> ();
		if (lr) {
			lr.enabled = true;
			lr.useWorldSpace = true;
		}
		// try to find all hidden line node in child and add them to node positions
		nodeTransforms = new List<Transform> ();
		foreach(Transform child in transform){
			if(child.GetComponent<HiddenLineNode>()){
				nodeTransforms.Add (child);
			}
		}
		// insert the receiver at the start
		if(statusReceiver){
			nodeTransforms.Insert (0, statusReceiver);
		}
		// insert the assigner at the end
		if(statusAssigner){
			nodeTransforms.Add (statusAssigner);
		}
		// update nodePositions
		UpdateNodePositions ();
		// set linerenderernodes
		SetLineRendererNodes ();
		lr.material = enemyLineMaterial;

		yield return new WaitUntil (() => receiverCS != null && assignerCS != null);

		assignerCS.OnLinkedByPlayer += SyncPlayerLink;

	}

	public void SyncPlayerLink(Transform assignerTrans){
		// first update line material if needed
		lr.material = playerLineMaterial;
		// then update control status
		receiverCS.controller = Controller.Hacker;
	}

	void UpdateNodePositions(){
		// update lineNodes
		if(nodeTransforms == null){
			return;
		}
		nodePositions = new Vector3[nodeTransforms.Count];
		for(int i = 0; i < nodeTransforms.Count; ++i){
			nodePositions [i] = nodeTransforms [i].position;
		}
	}

	void SetLineRendererNodes(){
		if(nodePositions == null || lr == null){
			return;
		}
		lr.positionCount = nodePositions.Length;
		lr.SetPositions (nodePositions);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
