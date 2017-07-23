using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMenu : MonoBehaviour
{
	public ObjectView obj;
	public Image color;
	public Text objName;
	public Button reset;
	public Toggle outMorphisms;
	public Toggle inMorphisms;
	public Toggle sphere;
	public Toggle showMarkers;
	public Toggle showName;

	// Use this for initialization
	void Start ()
	{
		SetNullObject (obj);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public void UpdateMesh (bool value)
	{
		if (value) {
			obj.UpdateMesh (1);
		} else {
			obj.UpdateMesh (0);
		}
	}

	public void ToggleShowOutMorphisms (bool value)
	{
		obj.SetShowOutMorphisms (value);
	}

	public void ToggleShowInMorphisms (bool value)
	{
		obj.SetShowInMorphisms (value);
	}

	public void SetShowMarkers (bool value)
	{
		obj.ShowMarkers = value;
	}

	public void SetShowName (bool value)
	{
		obj.ShowName = value;
	}

	public void ResetPosition ()
	{
		obj.ResetPosition ();
	}

	public void SetObject (ObjectView obj)
	{
		GetComponent<CanvasGroup> ().alpha = 1;
		GetComponent<CanvasGroup> ().interactable = true;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		this.obj = obj;
		objName.text = obj.id;
		color.color = obj.color;
		outMorphisms.isOn = obj.showOutMorphisms;
		inMorphisms.isOn = obj.showInMorphisms;
		if (obj.meshType > 0) {
			sphere.isOn = true;
		} else {
			sphere.isOn = false;
		}
		showMarkers.isOn = obj.ShowMarkers;
		showName.isOn = obj.ShowName;
	}
		
	public void SetNullObject (ObjectView obj)
	{
		if (this.obj == obj) {
			GetComponent<CanvasGroup> ().alpha = 0;
			GetComponent<CanvasGroup> ().interactable = false;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
			this.obj = null;
		}
	}
}
