using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEditorInternal;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
	public LayerMask clickableObjects;
	public Camera cam;
	public Color flashColor;
	public float colorTransitionSpeed = 0.01f;
	static GameObject selectedTile;
	private IEnumerator colorShifter;
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
					if (selectedTile != null)
					{
						StopCoroutine(colorShifter);
						selectedTile.GetComponent<SpriteRenderer>().color = Color.white;
					}
					colorShifter = ColorShifter(hit.collider.gameObject);
					StartCoroutine(colorShifter);
				}
			}
		}	
	}

	IEnumerator ColorShifter(GameObject tile)
	{
		selectedTile = tile;
		SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
		float transition = 0;
		Color originalColor = spriteRenderer.color;
		while (true)
		{
			spriteRenderer.color = Color.Lerp(originalColor, flashColor, transition);
			transition += colorTransitionSpeed;
			if (transition < 0 || transition > 1)
			{
				colorTransitionSpeed = -colorTransitionSpeed;
			}

			yield return new WaitForEndOfFrame();
		}

	}
}
