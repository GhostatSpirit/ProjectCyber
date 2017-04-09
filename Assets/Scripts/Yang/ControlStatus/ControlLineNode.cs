using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLineNode : MonoBehaviour {
	public float nodeSize = 7.5f;

	public bool Equals(LaserNode other) {
		return this.GetInstanceID() == other.GetInstanceID();
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawCube(transform.position, (Vector3.one * GetGizmoSize(transform.position)));
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
