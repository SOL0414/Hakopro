﻿using UnityEngine;
using System.Collections;

public class ScrollController : MonoBehaviour 
{
	[SerializeField] float minX = 0F;
	[SerializeField] float maxX = 14F;
	[SerializeField] float minY = 0F;
	[SerializeField] float maxY = 0F;

	private float nowX = 0F;
	private float beforeX = 0F;
	private bool useScroll = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButton(0)) 
		{
			Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast( mousePosWorld, Vector2.zero);					
			if( hit.collider != null)
			{
				if( hit.collider.gameObject == this.gameObject && useScroll)
				{
					float X = mousePosWorld.x - beforeX;
					Vector3 pos = Camera.main.transform.position;

					pos.x = Mathf.Clamp( pos.x - X, minX, maxX);
					Camera.main.transform.position = pos;
				}

				mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				beforeX = mousePosWorld.x;
				useScroll = true;
			}	
		}
		if (Input.GetMouseButtonUp (0)) 
		{
			useScroll = false;
		}



	}
}
