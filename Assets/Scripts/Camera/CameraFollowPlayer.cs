using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
	public Transform target;
	public float smoothing = 5f;
	Vector3 offset;

	// do zmniejszenia size camery
	public Camera cam1;

	void Start()
	{
		offset = transform.position - target.position;
	}

	void LateUpdate()
	{
		Vector3 targetCamPos = target.position + offset;
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

		if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
		{
			cam1.orthographicSize -= 0.1f;
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
		{
			cam1.orthographicSize += 0.1f; 
		}
	}
}
