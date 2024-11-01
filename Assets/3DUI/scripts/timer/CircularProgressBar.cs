using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularProgressBar : MonoBehaviour
{
    // Made with help of the following video: https://youtu.be/2MSMmPWedyg
    
    private bool isActive = false;

    private float indicatorTimer;

    private float maxIndicatorTimer;

    private Image radialProgressBar;

    private void Awake()
    {
        radialProgressBar = GetComponent<Image>();
    }

    public void ActivateCountdown(float countdownTime)
    {
        isActive = true;
        maxIndicatorTimer = countdownTime;
        indicatorTimer = maxIndicatorTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            indicatorTimer -= Time.deltaTime;
            radialProgressBar.fillAmount = (indicatorTimer / maxIndicatorTimer);
            
            if(indicatorTimer <= 0)
                isActive = false;
        }
        
    }
}
