using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
	public GameObject destPoint;
	
	private NavMeshAgent _agent;
	private RaycastHit _hit;
	private Camera _mainCamera;
	private Animator _animator;
	private GameObject _destPoint;

	public bool isAlive = true;
	
	private static readonly int IsWalk = Animator.StringToHash("isWalk");

	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		_agent.updateRotation = false;
		_animator = GetComponent<Animator>();
		_destPoint = Instantiate(destPoint);
		_mainCamera = Camera.main;
	}

	private void Update()
	{
		if (!isAlive) return;
		
		if (Input.GetKey("mouse 0"))
			PlayerGetTarget();
		PlayerMovementAnimation();
	}
	
	private void PlayerGetTarget()
	{
		if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out _hit))
		{
			if (_hit.transform.CompareTag("Enemy"))
			{
				return;
			}

			if (_hit.transform.CompareTag("Ground"))
			{
				_destPoint.transform.position = new Vector3(_hit.point.x, _hit.point.y, _hit.point.z);
				_agent.destination = _hit.point;
			}
		}
	}
	
	private void PlayerMovementAnimation()
	{
		if (_agent.velocity.magnitude <= 0.1f)
		{
			_animator.SetBool(IsWalk, false);
			_agent.updateRotation = false;
			destPoint.SetActive(false);
		}
		else
		{
			_animator.SetBool(IsWalk, true);
			// _agent.updateRotation = true;
			destPoint.SetActive(true);
		} 
	}
	
}
