using UnityEngine;

public class EndParkourDetection : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private GameLogic GameLogic;
    public VRHostSystem VRHostSystem;

    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched The End Barrier:" + other.name);
        if(other.CompareTag("VRUserBodyPart"))
        {
            Debug.Log("Player finished the race!!");
            //GetComponent<AudioSource>().Play();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            transform.Find("Text (TMP)").gameObject.SetActive(false);
            SetUiPosition();
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

    public void SetUiPosition()
    {
        Transform playerHead = VRHostSystem.getXROriginGameObject().transform;
        float distanceFromHead = 1f;
        Vector3 targetPosition = playerHead.position + playerHead.forward * distanceFromHead;
        targetPosition.y = 4;
        ui.transform.position = targetPosition;
        ui.transform.rotation = playerHead.rotation;
    }
}
