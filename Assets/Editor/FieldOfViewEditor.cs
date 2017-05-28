using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( FieldOfView ) )]
public class FieldOfViewEditor : Editor
{
	public float arrowSize = 1;

	SerializedObject fovScript;
	SerializedProperty radiusProp;

	void OnEnable(){
		fovScript = new SerializedObject (target);
		radiusProp = fovScript.FindProperty ("radius");
	}

	void OnSceneGUI( )
	{
		serializedObject.Update ();

		FieldOfView t = target as FieldOfView;

		Handles.color = Color.blue;
		Handles.Label( t.transform.position + Vector3.up * 2,
			t.transform.position.ToString( ) + "\nRadius: " +
			t.radius.ToString( ) );

		Handles.BeginGUI( );
		GUILayout.BeginArea( new Rect( Screen.width - 100, Screen.height - 80, 90, 50 ) );

		if( GUILayout.Button( "Reset Area" ) )
			t.radius = 5;

		GUILayout.EndArea( );
		Handles.EndGUI( );


		// rotate the start vector by t.angle / 2.0f degrees
		float delta = t.angle / 2.0f;
		Vector3 start = t.facing;
		start = Quaternion.AngleAxis (-delta, Vector3.forward) * start;


		Handles.color = new Color( 1, 1, 1, 0.2f );
		Handles.DrawSolidArc( t.transform.position, t.transform.forward, start,
			t.angle, t.radius );


		Handles.color = Color.white;
		t.radius = Handles.ScaleValueHandle( t.radius,
			t.transform.position + start * t.radius,
			Quaternion.FromToRotation(t.transform.forward, start), 1, Handles.ConeHandleCap, 1 );
			//t.transform.rotation, 1, Handles.ConeCap, 1 );

		radiusProp.floatValue = t.radius;
		fovScript.ApplyModifiedProperties ();


		if (GUI.changed)
			EditorUtility.SetDirty(target);

	}
}