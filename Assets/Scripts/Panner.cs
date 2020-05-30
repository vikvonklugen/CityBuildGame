using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panner : MonoBehaviour
{
    Vector3 touchStart;
    public static bool panning = false;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            panning = false;
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
            if (!panning)
            {
                panning = direction.magnitude > 0.1f;
            }
        }
    }
}
