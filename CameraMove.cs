using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public Transform target;
	public float zoomSpeed;
	public float rotateSpeed;

	// Use this for initialization
	void Start ()
	{
		transform.LookAt (target);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButton (1)) {
			transform.RotateAround (target.position, Vector3.forward, Input.GetAxis ("Mouse X") * rotateSpeed);
			transform.RotateAround (target.position, transform.TransformDirection (Vector3.right), Input.GetAxis ("Mouse Y") * -rotateSpeed);
		}
		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			if (transform.position != target.position) {
				Vector3 newPosition = Vector3.MoveTowards (transform.position, target.position, Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed);
				transform.position = newPosition;
			} else {
				if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
					Vector3 newPosition = transform.TransformDirection (Vector3.forward) * Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
					transform.position = newPosition;
				}
			}
		}
	}
}
