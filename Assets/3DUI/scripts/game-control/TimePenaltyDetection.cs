using System;
using UnityEngine;
using UnityEngine.XR;

namespace _3DUI.scripts
{
    public class TimePenaltyDetection : MonoBehaviour
    {
        public float penalty = 5f;
        public VRHostSystem VRHostSystem;
        
        [SerializeField] private GameLogic GameLogic;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("VRUserBodyPart"))
            {
                GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<AudioSource>().Play();
                GameLogic.AddTimePenalty(this);
                GenerateVibrations();
            }
        }

        public void Reset()
        {
            GetComponent<BoxCollider>().enabled = true;
        }
        
        public void GenerateVibrations()
        {
            if (VRHostSystem == null)
            {
                VRHostSystem = GameObject.FindGameObjectWithTag("VRHostSystemDevices").GetComponent<VRHostSystem>();
            }

            if (VRHostSystem != null)
            {
                uint channel = 0;
                float amplitude = 0.5f;
                float duration = 0.25f;
                if (VRHostSystem.GetRightHandDevice().TryGetHapticCapabilities(out var capabilitiesRight))
                {
                    if (capabilitiesRight.supportsImpulse)
                        VRHostSystem.GetRightHandDevice().SendHapticImpulse(channel, amplitude, duration);
                }
                if (VRHostSystem.GetRightHandDevice().TryGetHapticCapabilities(out var capabilitiesLeft))
                {
                    if (capabilitiesLeft.supportsImpulse)
                        VRHostSystem.GetLeftHandDevice().SendHapticImpulse(channel, amplitude, duration);
                }
            }
        }
    }
}