using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartParkourDetection : MonoBehaviour
{

    [SerializeField] private GameLogic GameLogic;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched The Start Barrier:" + other.name);
        if(other.CompareTag("VRUserBodyPart"))
        {
            Debug.Log("Player started the race!!");
            //GetComponent<AudioSource>().Play();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            transform.Find("Text (TMP)").gameObject.SetActive(false);
            GameLogic.timerActive = true;
        }
    }
    public void Reset()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        transform.Find("Text (TMP)").gameObject.SetActive(true);
    }
}
