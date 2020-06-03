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

    private bool clockAnimationFinished;

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

            yield return new WaitUntil(() => clockAnimationFinished);
            clockAnimationFinished = false;

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
                population += 5;
            }
        }
    }

    IEnumerator SetClock(float target, float increment)
    {
        float _target = Mathf.Clamp(target + increment, 0, 1);
        float _increment = increment;
        while (clock.fillAmount < _target)
        {
            clock.fillAmount += _increment;
            _increment += increment / 5f;
            yield return new WaitForEndOfFrame();
        }
        while (clock.fillAmount > target)
        {
            clock.fillAmount -= _increment / 5f;
            yield return new WaitForEndOfFrame();
        }
        clockAnimationFinished = true;
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
