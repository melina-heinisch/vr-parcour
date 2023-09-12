using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverOnFalling : MonoBehaviour
{

    [SerializeField] private GameLogic GameLogic;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched The Game Over Barrier:" + other.name);
        if (other.CompareTag("VRUserBodyPart"))
        {
            GameLogic.isGameOver = true;
        }
    }

}
