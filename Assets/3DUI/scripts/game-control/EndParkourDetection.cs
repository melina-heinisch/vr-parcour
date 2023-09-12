using UnityEngine;

public class EndParkourDetection : MonoBehaviour
{
    [SerializeField] private GameLogic GameLogic;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched The End Barrier:" + other.name);
        if(other.CompareTag("VRUserBodyPart"))
        {
            Debug.Log("Player finished the race!!");
            GetComponent<AudioSource>().Play();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            transform.Find("Text (TMP)").gameObject.SetActive(false);
            GameLogic.timerActive = false;
            if (GameLogic.timeRemaining > 0)
                GameLogic.isWin = true;
        }
    }

    public void Reset()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        transform.Find("Text (TMP)").gameObject.SetActive(true);
    }

}
