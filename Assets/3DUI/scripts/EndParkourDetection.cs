using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndParkourDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched The End Barrier:" + other.name);
        if(other.CompareTag("VRUserBodyPart"))
        {
            Debug.Log("Player finished the race!!");
            gameObject.SetActive(false);
        }
    }
}
