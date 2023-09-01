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
        this.gameObject.transform.position = startPosition.position;
    }
}
