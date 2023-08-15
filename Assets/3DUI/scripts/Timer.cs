using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class Timer : MonoBehaviour
{
    // Made with help of the following video: https://youtu.be/2MSMmPWedyg
    public GameObject timer;
    public GameObject radialProgressBar;
    public bool timerIsActive;

    public void StartTimer(float duration)
    {
        timerIsActive = true;
        timer.SetActive(true);
        radialProgressBar.GetComponent<CircularProgressBar>().ActivateCountdown(duration);

        StartCoroutine(EndTimer(duration));
    }

    IEnumerator EndTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        timerIsActive = false;
        timer.SetActive(false);
    }
}

