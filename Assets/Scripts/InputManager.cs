using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public Camera cam;

	public enum manualControlOverride { Mobile, PC }
	public manualControlOverride controlMode;

	public LayerMask clickableObjects;

	public float colorTransitionSpeed = 0.01f;
	public Color flashColor;
	private IEnumerator colorShifter;

	public static GameObject selectedTile;

	private Vector3 touchStart;
	private bool panning = false;

	public float minZoom = 1;
	public float maxZoom = 8;
	public float mobilePinchSensitivity = 0.01f;
	public float PCScrollSensitivity = 1f;
	private bool isMultiTouching;

	public Vector2 cameraBounds;

	private void Start()
	{
		cam = Camera.main;
	}

	void Update()
	{
		if (Input.touchCount == 2)
		{
			isMultiTouching = true;

			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

			float difference = currentMagnitude - prevMagnitude;

			Zoom(difference * mobilePinchSensitivity);
		}

		Zoom(Input.GetAxis("Mouse ScrollWheel") * PCScrollSensitivity);

		if (Input.GetMouseButtonDown(0))
		{
			panning = false;
			touchStart = cam.ScreenToWorldPoint(MousePosition());
		}

		if (Input.GetMouseButton(0))
		{
			Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(MousePosition());

			if ((isMultiTouching && Input.touchCount < 2))
			{
				direction = Vector2.zero;
				isMultiTouching = false;
			}

			cam.transform.position += direction;
			cam.transform.position = new Vector3(
				Mathf.Clamp(cam.transform.position.x, -cameraBounds.x, cameraBounds.x),
				Mathf.Clamp(cam.transform.position.y, -cameraBounds.y, cameraBounds.y),
				-10f);

			if (!panning)
			{
				panning = direction.magnitude > 0.1f;
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			if (!panning)
			{
				Vector2 mousePos = cam.ScreenToWorldPoint(MousePosition());
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

	void Zoom(float increment)
	{
		cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, minZoom, maxZoom);
	}

	Vector2 MousePosition()
	{
		if (controlMode == manualControlOverride.Mobile)
		{
			return Input.GetTouch(0).position;
		}
		else if (controlMode == manualControlOverride.PC)
		{
			return Input.mousePosition;
		}

		#if UNITY_IOS || UNITY_ANDROID
			return Input.GetTouch(0).position;
		#else
			return Input.mousePosition;
		#endif
	}

	IEnumerator ColorShifter(GameObject tile)
	{
		selectedTile = tile;

		SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
		Color originalColor = spriteRenderer.color;
		float transition = 0;

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
