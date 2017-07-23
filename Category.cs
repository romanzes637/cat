using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Category : MonoBehaviour
{
	[System.Serializable]
	public class Data
	{
		public List<string> objects;
		public List<int> morphismsFrom;
		public List<int> morphismsTo;
	}

	Data data;

	public MorphismView morphismView;
	public ObjectView objectView;
	public ObjectMenu objectMenu;

	Dictionary<int, string> objects;
	Dictionary<int, Dictionary<int, List<int> > > morphisms;

	Dictionary<int, ObjectView> objectViews;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine (Download ());
//		ReadData ();
//		objects = new Dictionary<int, string> ();
//		morphisms = new Dictionary<int, Dictionary<int, List<int> > > ();
//
//		for (int i = 0; i < data.objects.Count; i++) {
//			objects.Add (i, data.objects [i]);
//		} 
//
////		for (int i = 0; i < data.objects.Count; i++) {
////			if (!morphisms.ContainsKey (data.objects [i])) {
////				for (int j = 0; j < data.morphismsFrom.Count; j++) {
////					if (data.morphismsFrom [j] = i) {
////						if (!morphisms
////						List<int> homoset = new List<int> ();
////
////					}
////
////				}
////				Dictionary <int, int> homoset = new Dictionary<int, int> ();
////			}
////		}
//
//
//		objectViews = new Dictionary<int, ObjectView> ();
//
//
////		morphismViews = new Dictionary<string, MorphismView> ();
////		objects = new List<List<int>> ();
////		for (int i = 0; i < 1; i++) {
////			objects.Add (new List<int> ());
////			objects [i].Add (0);
////			objects [i].Add (1);
////		}
////		objects.Add (new List<int> ());
////		objects.Add (new List<int> ());
//
//		float deltaA = 2 * Mathf.PI / data.objects.Count;
//		float cntA = 0;
//		int nNodesA = (int)Mathf.Clamp (deltaA / (Mathf.PI / 18), 2, float.MaxValue);
//		float r1 = 10;
//		float r2 = 20;
//		for (int i = 0; i < data.objects.Count; i++) {
//			objectView.id = data.objects [i];
//			objectView.a1 = cntA;
//			objectView.a2 = objectView.a1 + deltaA;
//			objectView.r1 = r1;
//			objectView.r2 = r2;
//			objectView.nNodesA = nNodesA;
//			cntA += deltaA;
//			ObjectView ov = Instantiate (objectView, transform);
//			ov.Initialize ();
//			ov.OnSelected += objectMenu.SetObject;
//			ov.OnDeSelected += objectMenu.SetNullObject;
//			objectViews.Add (i, ov);
//			float cntR = r1;
//			int nNodesR = 2;
//
////			for (int j = 0; j < objects [i].Count; j++) {
//////				sectors [id].GetComponent<Renderer> ().enabled = false;
////				float deltaR = (r2 - r1) / objects [i].Count;
////				objectView.r1 = cntR;
////				objectView.r2 = objectView.r1 + deltaR;
////				objectView.nNodesR = nNodesR;
////				cntR += deltaR;
////				string id2 = string.Format ("[ \"{0:}\", \"{1:}\" ]", i, j);
////				objectView.id = id2;
////				ObjectView s2 = Instantiate (objectView, transform);
////				s2.Initialize ();
////				s2.OnSelected += objectMenu.SetObject;
////				s2.OnDeSelected += objectMenu.SetNullObject;
////				objectViews.Add (id2, s2);
////			}
//		}
//			
//		for (int i = 0; i < data.morphismsFrom.Count; i++) {
//			for (int j = 0; j < data.morphismsTo.Count; j++) {
//				int fromId = data.morphismsFrom [i];
//				int toId = data.morphismsTo [j];
//				Debug.Log (string.Format ("Morphism from {0:G} to {1:G}", 
//					objectViews [fromId].id, objectViews [toId].id));
//				MorphismView mv = Instantiate (morphismView, transform);
//				mv.Initialize (
//					objectViews [fromId].GetComponent<Renderer> ().bounds.center,
//					objectViews [toId].GetComponent<Renderer> ().bounds.center,
//					objectViews [fromId].GetComponent<Renderer> ().material.color);
//				objectViews [fromId].OnMoved += mv.UpdateStart;
//				objectViews [toId].OnMoved += mv.UpdateEnd;
//				objectViews [fromId].OnShowOutMorphismsChanged += mv.SetVisibility;
//				objectViews [toId].OnShowInMorphismsChanged += mv.SetVisibility;
//				objectViews [fromId].OnShowMarkersChanged += mv.SetMarkerVisibility;
//				objectViews [toId].OnShowMarkersChanged += mv.SetMarkerVisibility;
//
//			}
//		}
//
//		foreach (KeyValuePair<int, ObjectView> entryFrom in objectViews) {
//			foreach (KeyValuePair<int, ObjectView> entryTo in objectViews) {
//				// Unique object selection
//				if (entryFrom.Key != entryTo.Key) {
//					entryFrom.Value.OnSelected += entryTo.Value.SetSelected;
//				}
//			}
//		}
//
////		foreach (KeyValuePair<string, ObjectView> entryFrom in objectViews) {
////			foreach (KeyValuePair<string, ObjectView> entryTo in objectViews) {
////				MorphismView mv = Instantiate (morphismView, transform);
////				mv.Initialize (mv.start = entryFrom.Value.GetComponent<Renderer> ().bounds.center,
////					entryTo.Value.GetComponent<Renderer> ().bounds.center,
////					entryFrom.Value.GetComponent<Renderer> ().material.color);
////				entryFrom.Value.OnMoved += mv.UpdateStart;
////				entryTo.Value.OnMoved += mv.UpdateEnd;
////				entryFrom.Value.OnShowOutMorphismsChanged += mv.SetVisibility;
////				entryTo.Value.OnShowInMorphismsChanged += mv.SetVisibility;
////				entryFrom.Value.OnShowMarkersChanged += mv.SetMarkerVisibility;
////				entryTo.Value.OnShowMarkersChanged += mv.SetMarkerVisibility;
////				// Unique object selection
////				if (entryFrom.Key != entryTo.Key) {
////					entryFrom.Value.OnSelected += entryTo.Value.SetSelected;
////				}
////			}
////		}
//		objectView.gameObject.SetActive (false);
//		morphismView.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	IEnumerator Download ()
	{
		#if UNITY_EDITOR
			string filepath = "file:///" + Application.dataPath + "/data.json";
		#else
			string filepath = Application.dataPath + "/data.json";
		#endif

		Debug.Log (filepath);
		WWW jsonData = new WWW (filepath);
		yield return jsonData;
		Debug.Log (jsonData.text);
		this.data = JsonUtility.FromJson<Data> (jsonData.text);
		for (int i = 0; i < data.objects.Count; i++) {
			//			Debug.Log (data.objects [i]);
		}
		for (int i = 0; i < data.morphismsFrom.Count; i++) {
			//			Debug.Log (data.morphismsFrom [i]);
		}
		for (int i = 0; i < data.morphismsTo.Count; i++) {
			//			Debug.Log (data.morphismsTo [i]);
		}
		Debug.Log (string.Format ("Objects: {0:G}, Morphisms From: {1:G}, Morphisms To: {2:G}",
			data.objects.Count, data.morphismsFrom.Count, data.morphismsTo.Count));

		objects = new Dictionary<int, string> ();
		morphisms = new Dictionary<int, Dictionary<int, List<int> > > ();

		for (int i = 0; i < data.objects.Count; i++) {
			objects.Add (i, data.objects [i]);
		} 

		//		for (int i = 0; i < data.objects.Count; i++) {
		//			if (!morphisms.ContainsKey (data.objects [i])) {
		//				for (int j = 0; j < data.morphismsFrom.Count; j++) {
		//					if (data.morphismsFrom [j] = i) {
		//						if (!morphisms
		//						List<int> homoset = new List<int> ();
		//
		//					}
		//
		//				}
		//				Dictionary <int, int> homoset = new Dictionary<int, int> ();
		//			}
		//		}


		objectViews = new Dictionary<int, ObjectView> ();


		//		morphismViews = new Dictionary<string, MorphismView> ();
		//		objects = new List<List<int>> ();
		//		for (int i = 0; i < 1; i++) {
		//			objects.Add (new List<int> ());
		//			objects [i].Add (0);
		//			objects [i].Add (1);
		//		}
		//		objects.Add (new List<int> ());
		//		objects.Add (new List<int> ());

		float deltaA = 2 * Mathf.PI / data.objects.Count;
		float cntA = 0;
		int nNodesA = (int)Mathf.Clamp (deltaA / (Mathf.PI / 18), 2, float.MaxValue);
		float r1 = 10;
		float r2 = 20;
		for (int i = 0; i < data.objects.Count; i++) {
			objectView.id = data.objects [i];
			objectView.a1 = cntA;
			objectView.a2 = objectView.a1 + deltaA;
			objectView.r1 = r1;
			objectView.r2 = r2;
			objectView.nNodesA = nNodesA;
			cntA += deltaA;
			ObjectView ov = Instantiate (objectView, transform);
			ov.Initialize ();
			ov.OnSelected += objectMenu.SetObject;
			ov.OnDeSelected += objectMenu.SetNullObject;
			objectViews.Add (i, ov);
			float cntR = r1;
			int nNodesR = 2;

			//			for (int j = 0; j < objects [i].Count; j++) {
			////				sectors [id].GetComponent<Renderer> ().enabled = false;
			//				float deltaR = (r2 - r1) / objects [i].Count;
			//				objectView.r1 = cntR;
			//				objectView.r2 = objectView.r1 + deltaR;
			//				objectView.nNodesR = nNodesR;
			//				cntR += deltaR;
			//				string id2 = string.Format ("[ \"{0:}\", \"{1:}\" ]", i, j);
			//				objectView.id = id2;
			//				ObjectView s2 = Instantiate (objectView, transform);
			//				s2.Initialize ();
			//				s2.OnSelected += objectMenu.SetObject;
			//				s2.OnDeSelected += objectMenu.SetNullObject;
			//				objectViews.Add (id2, s2);
			//			}
		}

		for (int i = 0; i < data.morphismsFrom.Count; i++) {
			int fromId = data.morphismsFrom [i];
			int toId = data.morphismsTo [i];
			Debug.Log (string.Format ("Morphism from {0:G} to {1:G}", 
				objectViews [fromId].id, objectViews [toId].id));
			MorphismView mv = Instantiate (morphismView, transform);
			mv.Initialize (
				objectViews [fromId].GetComponent<Renderer> ().bounds.center,
				objectViews [toId].GetComponent<Renderer> ().bounds.center,
				objectViews [fromId].GetComponent<Renderer> ().material.color);
			objectViews [fromId].OnMoved += mv.UpdateStart;
			objectViews [toId].OnMoved += mv.UpdateEnd;
			objectViews [fromId].OnShowOutMorphismsChanged += mv.SetVisibility;
			objectViews [toId].OnShowInMorphismsChanged += mv.SetVisibility;
			objectViews [fromId].OnShowMarkersChanged += mv.SetMarkerVisibility;
			objectViews [toId].OnShowMarkersChanged += mv.SetMarkerVisibility;
		}

		foreach (KeyValuePair<int, ObjectView> entryFrom in objectViews) {
			foreach (KeyValuePair<int, ObjectView> entryTo in objectViews) {
				// Unique object selection
				if (entryFrom.Key != entryTo.Key) {
					entryFrom.Value.OnSelected += entryTo.Value.SetSelected;
				}
			}
		}

		//		foreach (KeyValuePair<string, ObjectView> entryFrom in objectViews) {
		//			foreach (KeyValuePair<string, ObjectView> entryTo in objectViews) {
		//				MorphismView mv = Instantiate (morphismView, transform);
		//				mv.Initialize (mv.start = entryFrom.Value.GetComponent<Renderer> ().bounds.center,
		//					entryTo.Value.GetComponent<Renderer> ().bounds.center,
		//					entryFrom.Value.GetComponent<Renderer> ().material.color);
		//				entryFrom.Value.OnMoved += mv.UpdateStart;
		//				entryTo.Value.OnMoved += mv.UpdateEnd;
		//				entryFrom.Value.OnShowOutMorphismsChanged += mv.SetVisibility;
		//				entryTo.Value.OnShowInMorphismsChanged += mv.SetVisibility;
		//				entryFrom.Value.OnShowMarkersChanged += mv.SetMarkerVisibility;
		//				entryTo.Value.OnShowMarkersChanged += mv.SetMarkerVisibility;
		//				// Unique object selection
		//				if (entryFrom.Key != entryTo.Key) {
		//					entryFrom.Value.OnSelected += entryTo.Value.SetSelected;
		//				}
		//			}
		//		}
		objectView.gameObject.SetActive (false);
		morphismView.gameObject.SetActive (false);
	}

	void ReadData ()
	{
		string filepath = Application.dataPath + "/data.json";
		if (File.Exists (filepath)) {
			string jsonData = File.ReadAllText (filepath);
			data = JsonUtility.FromJson<Data> (jsonData);
		} else {
			data = new Data ();
		}
		for (int i = 0; i < data.objects.Count; i++) {
//				Debug.Log (data.objects [i]);
		}
		for (int i = 0; i < data.morphismsFrom.Count; i++) {
//				Debug.Log (data.morphismsFrom [i]);
		}
		for (int i = 0; i < data.morphismsTo.Count; i++) {
//				Debug.Log (data.morphismsTo [i]);
		}
		Debug.Log (string.Format ("Objects: {0:G}, Morphisms From: {1:G}, Morphisms To: {2:G}",
			data.objects.Count, data.morphismsFrom.Count, data.morphismsTo.Count));
	}
}
