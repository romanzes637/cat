using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(LineRenderer))]
public class MorphismView : MonoBehaviour
{
	GameObject marker;
	public float markerSpeed;
	public float markerT;
	public bool showMarker;

	public Color color;

	public Vector3 start;
	public Vector3 end;
	public Vector3 bezier1;
	public Vector3 bezier2;
	public int nPoints;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GetComponent<LineRenderer> ().enabled && showMarker) {
			markerT += Time.deltaTime * markerSpeed;
			if (markerT > 1) {
				markerT = 0;
			}
			marker.transform.position = GetBezierPoint (markerT, start, bezier1, bezier2, end);
		}
	}

	public void Initialize (Vector3 start, Vector3 end, Color color)
	{
		this.start = start;
		this.end = end;
		this.color = color;
		GetComponent<LineRenderer> ().startColor = color;
		GetComponent<LineRenderer> ().endColor = color;
		EvaluateBezier ();
		Draw ();

		// Marker
		marker = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		marker.GetComponent<MeshRenderer> ().material.color = color;
		float lineWidth = GetComponent<LineRenderer> ().startWidth;
		marker.transform.localScale = new Vector3 (5 * lineWidth, 5 * lineWidth, 5 * lineWidth);
		marker.transform.parent = transform;

		SetVisibility (false);
	}

	public void SetMarkerVisibility (bool value)
	{
		showMarker = value;
		if (GetComponent<LineRenderer> ().enabled) {
			marker.GetComponent<MeshRenderer> ().enabled = value;
		}
	}

	public void SetVisibility (bool value)
	{
		GetComponent<LineRenderer> ().enabled = value;
		marker.GetComponent<MeshRenderer> ().enabled = value;
		SetMarkerVisibility (showMarker);
	}

	public void UpdateStart (Vector3 newStart)
	{
		start = newStart;
		Draw ();
	}

	public void UpdateEnd (Vector3 newEnd)
	{
		end = newEnd;
		Draw ();
	}

	void EvaluateBezier ()
	{
		float distance = Vector3.Distance (start, end);
		Vector3 dB1, dB2;
		if (distance > 0) {
			dB1 = Random.Range (3 * distance, 5 * distance) * Random.insideUnitSphere;
			dB2 = Random.Range (3 * distance, 5 * distance) * Random.insideUnitSphere;
		} else {
			dB1 = Random.Range (6 * start.magnitude, 10 * start.magnitude) * Random.insideUnitSphere;
			dB2 = Random.Range (6 * start.magnitude, 10 * start.magnitude) * Random.insideUnitSphere;
		}
		dB1.z = Mathf.Abs (dB1.z);
		dB2.z = Mathf.Abs (dB2.z);
		bezier1 = start + dB1;
		bezier2 = end + dB2;
	}

	void Draw ()
	{
		Vector3[] positions = new Vector3[nPoints + 2];
		positions [0] = start;
		for (int i = 1; i < nPoints + 1; i++) {
			positions [i] = GetBezierPoint ((float)i / (nPoints + 1), start, bezier1, bezier2, end);
		}
		positions [nPoints + 1] = end;
		GetComponent<LineRenderer> ().positionCount = positions.Length;
		GetComponent<LineRenderer> ().SetPositions (positions);
	}

	Vector3 GetBezierPoint (float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float k0 = (1 - t) * (1 - t) * (1 - t);
		float k1 = 3 * (1 - t) * (1 - t) * t;
		float k2 = 3 * (1 - t) * t * t;
		float k3 = t * t * t;
		return k0 * p0 + k1 * p1 + k2 * p2 + k3 * p3;
	}
}
