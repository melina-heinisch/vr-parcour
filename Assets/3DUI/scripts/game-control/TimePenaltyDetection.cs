using System;
using UnityEngine;

namespace _3DUI.scripts
{
    public class TimePenaltyDetection : MonoBehaviour
    {
        public float penalty = 5f;
        
        [SerializeField] private GameLogic GameLogic;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("VRUserBodyPart"))
            {
                GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<AudioSource>().Play();
                GameLogic.AddTimePenalty(this);
            }
        }

        public void Reset()
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}