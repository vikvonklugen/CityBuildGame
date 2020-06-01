using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public Camera cam;

	public enum manualControlOverride { Mobile, PC }
	public manualControlOverride controlMode;

	public LayerMask clickableObjects;

	public float colorTransitionSpeed = 0.02f;
	public Color flashColor = Color.gray;
	private IEnumerator colorShifter;

	public static GameObject selectedTile;

	private Vector3 touchStartPos;
	private Vector3 direction;
	private bool panning = false;
	private bool touchStart;

	public float minZoom = 1;
	public float maxZoom = 8;
	public float mobilePinchSensitivity = 0.01f;
	public float PCScrollSensitivity = 1f;
	[SerializeField]
	private bool isMultiTouching;

	public Vector2 cameraBounds = new Vector2(5f, 5f);


	private void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		// Zoom on mobile with 2 fingers
		if (Input.touchCount == 2)
		{
			isMultiTouching = true;

			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				touchStart = true;
			}

			// Calculate difference between previous distance between 2 fingers and current distance between 2 fingers
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

			float difference = currentMagnitude - prevMagnitude;

			// Apply that difference to zoom function
			Zoom(difference * mobilePinchSensitivity);
		}

		// Zoom on PC with scrollwheel (not working for me, it is for some reason always giving me a value of zero on my scroll wheel input)
		Zoom(Input.GetAxis("Mouse ScrollWheel") * PCScrollSensitivity);

		// Initialise some values on initial mouse press
		if (Input.GetMouseButtonDown(0))
		{
			panning = false;
			touchStartPos = cam.ScreenToWorldPoint(MousePosition());
			direction = Vector3.zero;
		}

		if (Input.GetMouseButton(0))
		{
			// Find difference between pointer position and camera position
			direction = touchStartPos - Camera.main.ScreenToWorldPoint(MousePosition());

			// If the player lifts the finger that touched the screen first before lifting the other finger, the game would snap to the new first finger position. That is why we need to set the direction to zero briefly.
			if ((isMultiTouching && Input.touchCount < 2) || touchStart)
			{
				direction = Vector3.zero;
				touchStartPos = cam.ScreenToWorldPoint(MousePosition());
				isMultiTouching = false;
				touchStart = false;
			}
			else
			{
				// Move the camera
				cam.transform.position += direction;
			}

			// Clamp camera pos to bounded area
			cam.transform.position = new Vector3(
				Mathf.Clamp(cam.transform.position.x, -cameraBounds.x, cameraBounds.x),
				Mathf.Clamp(cam.transform.position.y, -cameraBounds.y, cameraBounds.y),
				-10f);

			// Check if player is panning or clicking
			if (!panning)
			{
				panning = direction.magnitude > 0.1f;
			}
		}

		// Select a tile if player wasn't panning and start color animation
		if (Input.GetMouseButtonUp(0))
		{
			if (!panning)
			{
				Vector2 mousePos = cam.ScreenToWorldPoint(MousePosition());
				RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
				if (hit)
				{
					// Stop previous running selection coroutine and reset their color
					if (selectedTile != null)
					{
						StopCoroutine(colorShifter);
						selectedTile.GetComponent<SpriteRenderer>().color = Color.white;
					}

					// Start new selection coroutine
					colorShifter = ColorShifter(hit.collider.gameObject);
					StartCoroutine(colorShifter);
				}
			}
		}
	}

	// Zoom the camera
	void Zoom(float increment)
	{
		cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, minZoom, maxZoom);
	}

	// Return mouse or finger position based on platform or setting
	Vector2 MousePosition(int finger = 0)
	{
		if (controlMode == manualControlOverride.Mobile)
		{
			return Input.GetTouch(finger).position;
		}
		else if (controlMode == manualControlOverride.PC)
		{
			return Input.mousePosition;
		}

#if UNITY_IOS || UNITY_ANDROID
			return Input.GetTouch(finger).position;
#else
		return Input.mousePosition;
#endif
	}

	// Shifts colors of selected tile
	IEnumerator ColorShifter(GameObject tile)
	{
		selectedTile = tile;

		SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
		Color originalColor = spriteRenderer.color;
		float transition = 0;

		while (true)
		{
			// Change color gradually
			spriteRenderer.color = Color.Lerp(originalColor, flashColor, transition);
			transition += colorTransitionSpeed;

			// Reverse color fade direction having reached min or max value
			if (transition < 0 || transition > 1)
			{
				colorTransitionSpeed = -colorTransitionSpeed;
			}

			yield return new WaitForEndOfFrame();
		}

	}
}
