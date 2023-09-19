using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartParkourDetection : MonoBehaviour
{

    [SerializeField] private GameLogic GameLogic;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("VRUserBodyPart"))
        {
            GetComponent<AudioSource>().Play();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            transform.Find("Text (TMP)").gameObject.SetActive(false);
            GameLogic.timeChallengeActive = true;
        }
    }
    public void Reset()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        transform.Find("Text (TMP)").gameObject.SetActive(true);
    }
}
