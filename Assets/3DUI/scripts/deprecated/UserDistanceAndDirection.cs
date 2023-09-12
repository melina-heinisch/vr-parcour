using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserDistanceAndDirection : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject mainPlayerCamera;
    public float distanceToMainPlayerCamera;
    public Vector3 normalisedHereToMainPlayerCamera;
    public Vector3 hereToMainPlayerCamera;
    private Text[] texts;
    public float fontSizeAngularHeigthToKeepInDegree = 3.0f;



    void Start()
    {
        mainPlayerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        Debug.Log("Player Camera Found : " + mainPlayerCamera + "Position: " + mainPlayerCamera.transform.position + "rotation" + mainPlayerCamera.transform.rotation);
        getTextComponents();
    }



    // Update is called once per frame
    void Update()
    {
        computeDistanceDirectionToCamera();
        adaptFontSizeToWishAngularHeight();
        //logTextAngularHeight();
    }

    private void adaptFontSizeToWishAngularHeight()
    {
        foreach (Text t in texts)
        {
            float fontsizeToHaveWishedAngularHeight = Mathf.Tan(fontSizeAngularHeigthToKeepInDegree * Mathf.Deg2Rad) * distanceToMainPlayerCamera;
            t.fontSize = Mathf.RoundToInt(fontsizeToHaveWishedAngularHeight);

           Debug.Log("fontsizeToHaveWishedAngularHeight " + fontsizeToHaveWishedAngularHeight);
           Debug.Log(" Mathf.RoundToInt(fontsizeToHaveWishedAngularHeight) " + Mathf.RoundToInt(fontsizeToHaveWishedAngularHeight));
        }
    }

    private void getTextComponents()
    {
        texts = this.GetComponentsInChildren<Text>();
        logTextAngularHeight();
    }

    private void logTextAngularHeight()
    {
        foreach (Text t in texts)
        {
            float measuredDngularHeight = Mathf.Rad2Deg * Mathf.Atan(t.fontSize / distanceToMainPlayerCamera);

            Debug.Log("text found " + t + "  " + t.fontSize + "angular height " + measuredDngularHeight);
        }
    }

    private void computeDistanceDirectionToCamera()
    {
        hereToMainPlayerCamera = mainPlayerCamera.transform.position - transform.position;
        distanceToMainPlayerCamera = hereToMainPlayerCamera.magnitude;
        normalisedHereToMainPlayerCamera = hereToMainPlayerCamera.normalized;
        //Debug.Log("Distance To Camera " + distanceToMainPlayerCamera + "\n vector to camera" + hereToMainPlayerCamera + "\n Normalised" + normalisedHereToMainPlayerCamera);
    }
}
