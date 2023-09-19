using UnityEngine;

public class EndParkourDetection : MonoBehaviour
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
            GameLogic.timeChallengeActive = false;
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
