using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverOnFalling : MonoBehaviour
{

    [SerializeField] private GameLogic GameLogic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VRUserBodyPart"))
        {
            GameLogic.isGameOver = true;
        }
    }

}
