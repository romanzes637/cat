using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]
public class ObjectView : MonoBehaviour
{
	public event Action<Vector3> OnMoved;

	public event Action<bool> OnShowOutMorphismsChanged;
	public event Action<bool> OnShowInMorphismsChanged;

	public event Action<ObjectView> OnSelected;
	public event Action<ObjectView> OnDeSelected;

	public event Action<bool> OnShowMarkersChanged;

	public int nNodesR;
	public int nNodesA;
	public int nNodesH;

	public Material mat;
	public Material highlightMat;
	public Color color;

	public float r1;
	public float r2;
	public float a1;
	public float a2;
	public float h1;
	public float h2;

	public string id;

	public float dragTimeCnt;
	public float startDragTime;
	//	public float fastClickTime;
	//	public float fastClickCnt;
	public float startClickTime;

	public bool isSelected;

	public bool showOutMorphisms;
	public bool showInMorphisms;

	bool showMarkers;

	public bool ShowMarkers {
		get {
			return showMarkers;
		}
		set {
			showMarkers = value;
			if (OnShowMarkersChanged != null) {
				OnShowMarkersChanged (showMarkers);
			}
		}
	}

	public bool showName;

	public bool ShowName { get; set; }

	public int meshType;

	// Use this for initialization
	void Start ()
	{
		
	}
		
	// Update is called once per frame
	void Update ()
	{

	}

	void OnGUI ()
	{
		if (ShowName) {
			Vector2 position = Camera.main.WorldToScreenPoint (GetComponent<MeshRenderer> ().bounds.center);
			position.y = Camera.main.pixelHeight - position.y;
			GUI.Label (new Rect (position, new Vector2 (150, 30)), id);
		}
	}

	public void SetShowOutMorphisms (bool value)
	{
		if (showOutMorphisms != value) {
			showOutMorphisms = value;
			if (OnShowOutMorphismsChanged != null) {
				OnShowOutMorphismsChanged (showOutMorphisms);
			}
		}
	}

	public void SetShowInMorphisms (bool value)
	{
		if (showInMorphisms != value) {
			showInMorphisms = value;
			if (OnShowInMorphismsChanged != null) {
				OnShowInMorphismsChanged (showInMorphisms);
			}
		}
	}

	public void SetSelected (bool value)
	{
		if (isSelected != value) {
			isSelected = value;
			if (isSelected) {
				GetComponent<MeshRenderer> ().material = highlightMat;
//				GetComponent<MeshRenderer> ().material.color = color;
				GetComponent<MeshRenderer> ().material.SetColor ("_ColorTint", color);
				GetComponent<MeshRenderer> ().material.SetColor ("_RimColor", color);
				if (OnSelected != null) {
					OnSelected (this);
				}
			} else {
				GetComponent<MeshRenderer> ().material = mat;
				GetComponent<MeshRenderer> ().material.color = color;
				if (OnDeSelected != null) {
					OnDeSelected (this);
				}
			}
		}
	}

	// For unique selection
	public void SetSelected (ObjectView obj)
	{
		if (obj.isSelected && isSelected) {
			SetSelected (false);
		}
	}

	public void ResetPosition ()
	{
		transform.localPosition = Vector3.zero;
		if (OnMoved != null) {
			OnMoved (GetComponent<MeshRenderer> ().bounds.center);
		}
	}

	void OnMouseDown ()
	{
		startClickTime = Time.time;
	}

	void OnMouseDrag ()
	{
		dragTimeCnt += Time.deltaTime;
		if (dragTimeCnt > startDragTime) {
			Vector3 screenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Vector3.Distance (transform.parent.position, Camera.main.transform.position));
			Vector3 newPosition = Camera.main.ScreenToWorldPoint (screenPoint);
//			newPosition.z = 0;
			transform.position = newPosition;
			if (OnMoved != null) {
				OnMoved (GetComponent<MeshRenderer> ().bounds.center);
			}
		}
	}

	void OnMouseUp ()
	{
// TODO Double click?
//		if (deltaTime < fastClickTime) {
//			fastClickCnt += 1;
//			if (fastClickCnt > 1) {
//				transform.localPosition = Vector3.zero;
//				if (OnObjectMoved != null) {
//					OnObjectMoved (GetComponent<Renderer> ().bounds.center);
//				}
//				fastClickCnt = 0;
//			}
//		} else if(deltaTime > startDragTime {
		if (Time.time - startClickTime < startDragTime) {
			SetSelected (!isSelected);
		}
		dragTimeCnt = 0;
	}

	public void Initialize ()
	{
		color = UnityEngine.Random.ColorHSV (0, 1, 1, 1, 1, 1, 1, 1);
		GetComponent<MeshRenderer> ().material = mat;
		GetComponent<MeshRenderer> ().material.color = color;
		UpdateMesh ();
	}

	public void UpdateMesh (int type = 0)
	{
		meshType = type;

		if (type == 0) {
			List<Vector3> vertices = new List<Vector3> ();
			List<int> triangles = new List<int> ();
			float stepR = (r2 - r1) / (nNodesR - 1);
			float stepA = (a2 - a1) / (nNodesA - 1);
			float stepH = (h2 - h1) / (nNodesH - 1);
			int startVertex = 0;

			// R1
			// Verts
			startVertex = vertices.Count;
			for (int j = 0; j < nNodesH; j++) {
				float h = h1 + j * stepH;
				for (int i = 0; i < nNodesA; i++) {
					float a = a1 + i * stepA;
					vertices.Add (new Vector3 (r1 * Mathf.Cos (a), r1 * Mathf.Sin (a), h));
				}
			}
			// Tris
			for (int j = 0; j < nNodesH - 1; j++) {
				for (int i = 0; i < nNodesA - 1; i++) {
					triangles.Add (startVertex + i + j * nNodesA);
					triangles.Add (startVertex + i + nNodesA + j * nNodesA);
					triangles.Add (startVertex + i + nNodesA + 1 + j * nNodesA);
					triangles.Add (startVertex + i + j * nNodesA);
					triangles.Add (startVertex + i + nNodesA + 1 + j * nNodesA);
					triangles.Add (startVertex + i + 1 + j * nNodesA);
				}
			}

			// R2
			// Verts
			startVertex = vertices.Count;
			for (int j = 0; j < nNodesH; j++) {
				float h = h1 + j * stepH;
				for (int i = 0; i < nNodesA; i++) {
					float a = a1 + i * stepA;
					vertices.Add (new Vector3 (r2 * Mathf.Cos (a), r2 * Mathf.Sin (a), h));
				}
			}
			// Tris
			for (int j = 0; j < nNodesH - 1; j++) {
				for (int i = 0; i < nNodesA - 1; i++) {
					triangles.Add (startVertex + i + j * nNodesA);
					triangles.Add (startVertex + i + nNodesA + 1 + j * nNodesA);
					triangles.Add (startVertex + i + nNodesA + j * nNodesA);
					triangles.Add (startVertex + i + j * nNodesA);
					triangles.Add (startVertex + i + 1 + j * nNodesA);
					triangles.Add (startVertex + i + nNodesA + 1 + j * nNodesA);
				}
			}

			// A1
			// Verts
			startVertex = vertices.Count;
			for (int j = 0; j < nNodesH; j++) {
				float h = h1 + j * stepH;
				for (int i = 0; i < nNodesR; i++) {
					float r = r1 + i * stepR;
					vertices.Add (new Vector3 (r * Mathf.Cos (a1), r * Mathf.Sin (a1), h));
				}
			}
			// Tris
			for (int j = 0; j < nNodesH - 1; j++) {
				for (int i = 0; i < nNodesR - 1; i++) {
					triangles.Add (startVertex + i + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + 1 + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + j * nNodesR);
					triangles.Add (startVertex + i + j * nNodesR);
					triangles.Add (startVertex + i + 1 + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + 1 + j * nNodesR);
				}
			}

			// A2
			// Verts
			startVertex = vertices.Count;
			for (int j = 0; j < nNodesH; j++) {
				float h = h1 + j * stepH;
				for (int i = 0; i < nNodesR; i++) {
					float r = r1 + i * stepR;
					vertices.Add (new Vector3 (r * Mathf.Cos (a2), r * Mathf.Sin (a2), h));
				}
			}
			// Tris
			for (int j = 0; j < nNodesH - 1; j++) {
				for (int i = 0; i < nNodesR - 1; i++) {
					triangles.Add (startVertex + i + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + 1 + j * nNodesR);
					triangles.Add (startVertex + i + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + 1 + j * nNodesR);
					triangles.Add (startVertex + i + 1 + j * nNodesR);
				}
			}

			// H1
			// Verts
			startVertex = vertices.Count;
			for (int j = 0; j < nNodesA; j++) {
				float a = a1 + j * stepA;
				for (int i = 0; i < nNodesR; i++) {
					float r = r1 + i * stepR;
					vertices.Add (new Vector3 (r * Mathf.Cos (a), r * Mathf.Sin (a), h1));
				}
			}
			// Tris
			for (int j = 0; j < nNodesA - 1; j++) {
				for (int i = 0; i < nNodesR - 1; i++) {
					triangles.Add (startVertex + i + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + 1 + j * nNodesR);
					triangles.Add (startVertex + i + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + 1 + j * nNodesR);
					triangles.Add (startVertex + i + 1 + j * nNodesR);
				}
			}

			// H2
			// Verts
			startVertex = vertices.Count;
			for (int j = 0; j < nNodesA; j++) {
				float a = a1 + j * stepA;
				for (int i = 0; i < nNodesR; i++) {
					float r = r1 + i * stepR;
					vertices.Add (new Vector3 (r * Mathf.Cos (a), r * Mathf.Sin (a), h2));
				}
			}
			// Tris
			for (int j = 0; j < nNodesA - 1; j++) {
				for (int i = 0; i < nNodesR - 1; i++) {
					triangles.Add (startVertex + i + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + 1 + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + j * nNodesR);
					triangles.Add (startVertex + i + j * nNodesR);
					triangles.Add (startVertex + i + 1 + j * nNodesR);
					triangles.Add (startVertex + i + nNodesR + 1 + j * nNodesR);
				}
			}
//		Debug.Log (string.Format ("nVerts = {0:G}", vertices.Count));
//		Debug.Log (string.Format ("nTris = {0:G}", triangles.Count / 3));
			Mesh mesh = new Mesh ();
			mesh.vertices = vertices.ToArray ();
			mesh.triangles = triangles.ToArray ();
			mesh.RecalculateNormals ();
			mesh.RecalculateBounds ();
			GetComponent<MeshFilter> ().mesh = mesh;
			GetComponent<MeshCollider> ().sharedMesh = mesh;
			transform.localScale = Vector3.one;
		} else {
			GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			Mesh mesh = sphere.GetComponent<MeshFilter> ().sharedMesh;
			transform.position = GetComponent<MeshRenderer> ().bounds.center;
			transform.localScale = new Vector3 (r2 - r1, r2 - r1, r2 - r1);
			GetComponent<MeshFilter> ().mesh = mesh;
			GetComponent<MeshCollider> ().sharedMesh = mesh;
			GameObject.Destroy (sphere);
		}
		if (OnMoved != null) {
			OnMoved (GetComponent<MeshRenderer> ().bounds.center);
		}
	}
}
