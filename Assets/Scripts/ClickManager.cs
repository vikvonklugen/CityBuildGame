using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
	public LayerMask clickableObjects;
	public Camera cam;
	private void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (!Panner.panning)
			{
				Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
				if (hit)
				{
					Debug.Log(hit.transform.name);
				}
			}
		}	
	}
}
