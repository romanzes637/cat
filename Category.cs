using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Category : MonoBehaviour
{
	public MorphismView morphismView;
	public ObjectView objectView;
	public ObjectMenu objectMenu;

	Dictionary<string, ObjectView> objectViews;
	List<List<int>> objects;

	//	Dictionary<string, MorphismView> morphismViews;
	//	Dictionary<string, Dictionary<string, List<string> > > morphisms;

	// Use this for initialization
	void Start ()
	{
		objectViews = new Dictionary<string, ObjectView> ();
//		morphismViews = new Dictionary<string, MorphismView> ();
		objects = new List<List<int>> ();
		for (int i = 0; i < 1; i++) {
			objects.Add (new List<int> ());
			objects [i].Add (0);		
			objects [i].Add (1);
		}
		objects.Add (new List<int> ());
		objects.Add (new List<int> ());

		float deltaA = 2 * Mathf.PI / objects.Count;
		float cntA = 0;
		int nNodesA = (int)Mathf.Clamp (deltaA / (Mathf.PI / 18), 2, float.MaxValue);
		float r1 = 10;
		float r2 = 20;

		for (int i = 0; i < objects.Count; i++) {
			objectView.a1 = cntA;
			objectView.a2 = objectView.a1 + deltaA;
			objectView.r1 = r1;
			objectView.r2 = r2;
			objectView.nNodesA = nNodesA;
			cntA += deltaA;
			string id = string.Format ("[ \"{0:}\" ]", i);
			objectView.id = id;
			ObjectView s = Instantiate (objectView, transform);
			s.Initialize ();
			s.OnSelected += objectMenu.SetObject;
			s.OnDeSelected += objectMenu.SetNullObject;
			objectViews.Add (id, s);

			float cntR = r1;
			int nNodesR = 2;
			for (int j = 0; j < objects [i].Count; j++) {
//				sectors [id].GetComponent<Renderer> ().enabled = false;
				float deltaR = (r2 - r1) / objects [i].Count;
				objectView.r1 = cntR;
				objectView.r2 = objectView.r1 + deltaR;
				objectView.nNodesR = nNodesR;
				cntR += deltaR;
				string id2 = string.Format ("[ \"{0:}\", \"{1:}\" ]", i, j);
				objectView.id = id2;
				ObjectView s2 = Instantiate (objectView, transform);
				s2.Initialize ();
				s2.OnSelected += objectMenu.SetObject;
				s2.OnDeSelected += objectMenu.SetNullObject;
				objectViews.Add (id2, s2);
			}
		}
			
		foreach (KeyValuePair<string, ObjectView> entryFrom in objectViews) {
			foreach (KeyValuePair<string, ObjectView> entryTo in objectViews) {
				MorphismView mv = Instantiate (morphismView, transform);
				mv.Initialize (mv.start = entryFrom.Value.GetComponent<Renderer> ().bounds.center,
					entryTo.Value.GetComponent<Renderer> ().bounds.center,
					entryFrom.Value.GetComponent<Renderer> ().material.color);
				entryFrom.Value.OnMoved += mv.UpdateStart;
				entryTo.Value.OnMoved += mv.UpdateEnd;
				entryFrom.Value.OnShowOutMorphismsChanged += mv.SetVisibility;
				entryTo.Value.OnShowInMorphismsChanged += mv.SetVisibility;
				entryFrom.Value.OnShowMarkersChanged += mv.SetMarkerVisibility;
				entryTo.Value.OnShowMarkersChanged += mv.SetMarkerVisibility;
				// Unique object selection
				if (entryFrom.Key != entryTo.Key) {
					entryFrom.Value.OnSelected += entryTo.Value.SetSelected;
				}
			}
		}
		objectView.gameObject.SetActive (false);
		morphismView.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
