using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ControlStatus))]
public class ControlStatusEditor : Editor {

	//SerializedProperty controllerProp;


	private void OnEnable(){
		//controllerProp = serializedObject.FindProperty ("initialController");
	}

	public override void OnInspectorGUI(){
		ControlStatus cs = (ControlStatus)target;

		EditorGUILayout.LabelField ("Controller", cs.controller.ToString ());

		DrawDefaultInspector ();

		this.Repaint ();
	}

}
