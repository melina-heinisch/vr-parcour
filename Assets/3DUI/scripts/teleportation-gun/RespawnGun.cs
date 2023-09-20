using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RespawnGun : MonoBehaviour
{
    private Transform startPosition;
    public bool doOnce = false;
    
    public void SetStartPosition(Transform start)
    {
        startPosition = start;
    }
    
    public void RespawnGunToStartPosition()
    {
        if (doOnce)
        {
            doOnce = false;
            gameObject.SetActive(false);
            // Reset velocity, so the gun just drops straight down
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            gameObject.transform.position = startPosition.position;
            gameObject.SetActive(true);
        }
        
    }

    public void enableOnce()
    {
        doOnce = true;
    }
}
