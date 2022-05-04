using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCam : MonoBehaviour
{
	// Rotate pivot
	public Transform pivot;
	public int rotationCount;
	public int rotModulo;
	public float timeCount = 0.0f;

	public Transform target;
	public float smoothing = 5f;
	Vector3 offset;

	// do zmniejszenia size camery
	public Camera cam1;

	// Input
	public PlayerInput mouseScroll;
	public float currentScroll;

	private void Awake()
	{
		mouseScroll = new PlayerInput();
		mouseScroll.InputControls.Zoom.performed += ctx => currentScroll = -ctx.ReadValue<float>();

		// Rotate event
		mouseScroll.Camera.Rotate.performed += ctx =>
		{
			rotationCount++;
				rotModulo = rotationCount % 4;
		};

	}
	void LateUpdate()
	{
		// Follow
		Vector3 targetCamPos = target.position;
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

		// Zoom
		if (currentScroll > 0)
        {
			cam1.orthographicSize += 0.1f;
		}
		if (currentScroll  < 0)
		{
			cam1.orthographicSize -= 0.1f;
		}
		var currentPos = Quaternion.Euler(30f, 90f * rotModulo, 0f);
		var nextPos = Quaternion.Euler(30f, 90f * (rotModulo+1), 0f);
		pivot.transform.rotation = Quaternion.Slerp(currentPos, nextPos, 0.05f * Time.deltaTime);
	}


    private void OnEnable()
    {
		mouseScroll.Enable();
	}
    private void OnDisable()
    {
        mouseScroll.Disable();
    }
}
