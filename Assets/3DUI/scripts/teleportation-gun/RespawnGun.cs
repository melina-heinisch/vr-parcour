using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnGun : MonoBehaviour
{
    private Transform startPosition;
    
    public void SetStartPosition(Transform start)
    {
        startPosition = start;
    }
    
    public void RespawnGunToStartPosition()
    {
        // Reset velocity, so the gun just drops straight down
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.transform.position = startPosition.position;
        gameObject.SetActive(true);
    }
}
