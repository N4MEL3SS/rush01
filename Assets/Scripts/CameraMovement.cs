using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public Transform Target;
	
	public float DistanceMin = 10;
	public float DistanceMax = 50;
	public float SensitivityX = 40;
	public float SensitivityY = 120;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	private float _currentDistance;
	private float _xCamera;
	private float _yCamera;
	
	private RaycastHit _hit;
	
	private void Start ()
	{
		var angles = transform.eulerAngles;
		_xCamera = angles.y;
		_yCamera = angles.x;
		_currentDistance = 10f;
	}

	private void LateUpdate()
	{
		if (!Target) return;
		
		if(Input.GetKey("mouse 1"))
		{
			Cursor.visible = false;
			_xCamera += Input.GetAxis("Mouse X") * SensitivityX * _currentDistance * 0.02f;
			_yCamera -= Input.GetAxis("Mouse Y") * SensitivityY * 0.02f;
		}
		else
			Cursor.visible = true;
		
		_currentDistance = Mathf.Clamp(_currentDistance - Input.GetAxis("Mouse ScrollWheel") * 5, DistanceMin, DistanceMax);
		
		if (Physics.Linecast(Target.position, transform.position, out _hit))
		{
			if (!_hit.transform.CompareTag("Enemy"))
				_currentDistance -= _hit.distance;
		}

		var rotation = Quaternion.Euler(ClampAngle(_yCamera, yMinLimit, yMaxLimit), _xCamera, 0);
		var position = rotation * new Vector3(0.0f, 0.0f, - _currentDistance) + Target.position;
		transform.rotation = rotation;
		transform.position = position;
	}
	
	private static float ClampAngle(float angle, float min,float max) 
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
}