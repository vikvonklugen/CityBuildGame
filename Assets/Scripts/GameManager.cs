using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int food;
    public static int materials;
    public static int luxuries;
    public static int population;

    public Image clock;
    public float clockFillAmount;
    public GameObject eventPanel;
    public bool eventProcessed;

    void Start()
    {
        StartCoroutine(TickSystem());
    }

    public void ProcessEvent()
    {
        eventProcessed = true;
        clockFillAmount = 0f;
        StartCoroutine(ResetClock());
    }

    IEnumerator TickSystem()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(7.5f);

            clockFillAmount += 0.25f;
            StartCoroutine(SetClock(clockFillAmount, 0.01f));

            if (clockFillAmount == 1)
            {
                //Do event stuff
                eventPanel.SetActive(true);

                while (eventProcessed == false)
                {
                    yield return new WaitForEndOfFrame();
                }
                eventProcessed = false;
            }
            else if (clockFillAmount == 0.5f)
            {
                //Grow population
            }
        }
    }

    IEnumerator SetClock(float target, float increment)
    {
        float _target = Mathf.Clamp(target + increment, 0, 1);
        while (clock.fillAmount < _target)
        {
            clock.fillAmount += increment;
            yield return new WaitForEndOfFrame();
        }
        while (clock.fillAmount > target)
        {
            clock.fillAmount -= increment / 5f;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ResetClock()
    {
        while (clock.fillAmount > 0)
        {
            clock.fillAmount -= 0.05f;
            yield return new WaitForEndOfFrame();
        }

    }
}
