﻿using UnityEngine;
using System.Collections;

public class ScrollController : MonoBehaviour 
{
	[SerializeField] float minX = 0F;
	[SerializeField] float maxX = 14F;
	[SerializeField] float minY = 0F;
	[SerializeField] float maxY = 0F;
	[SerializeField] float maxScale = 10F;
	[SerializeField] float minScale = 5F;

	private float beforeX = 0F;
	private float beforeY;
	private bool useScroll = false;

	private bool usePC = true;

	private BoxCollider2D collider;
	private float interval = 0.0f;
	private bool isPitched = false;
	const float ZOOM_SPEED = 600.0f;


	void Start()
	{
		#if UNITY_ANDROID
			usePC = false;
		#endif

		collider = this.GetComponent<BoxCollider2D> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if ( !collider.enabled) 
		{
			return;
		}

		MoveCamera ();

		if (Input.GetMouseButtonUp (0)) 
		{
			useScroll = false;
		}

		ScaleProcess ();


	}

	void MoveCamera()
	{
		if (isPitched) 
		{
			return;
		}

		if (Input.GetMouseButton(0)) 
		{
			Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast( mousePosWorld, Vector2.zero);					
			if( hit.collider != null)
			{
				if( hit.collider.gameObject == this.gameObject && useScroll)
				{
					float X = mousePosWorld.x - beforeX;
					float Y = mousePosWorld.y - beforeY;
					Vector3 pos = Camera.main.transform.position;
					
					pos.x = Mathf.Clamp( pos.x - X, minX, maxX);
					pos.y = Mathf.Clamp( pos.y - Y, minY, maxY);
					Camera.main.transform.position = pos;
				}
				
				mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				
				beforeX = mousePosWorld.x;
				beforeY = mousePosWorld.y;
				useScroll = true;
			}	
		}
	}

	void ScaleProcess()
	{
		if (usePC) {
			Camera.main.orthographicSize += Input.GetAxis ("Mouse ScrollWheel");
		} 
		else 
		{
			if (Input.touchCount == 2) {
				if (Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began) {
					interval = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
				}
				float tmpInterval = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
				Camera.main.orthographicSize += (tmpInterval - interval) / ZOOM_SPEED;
				isPitched = true;
			}
			else
			{
				isPitched = false;
			}
		}
		
		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, minScale, maxScale);
	}

	void SetLock()
	{
		collider.enabled = false;
	}

	void LiftLock()
	{
		collider.enabled = true;
	}
}
