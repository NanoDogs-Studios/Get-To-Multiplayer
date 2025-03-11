using System;
using FishNet.Component.Animating;
using FishNet.Object;
using UnityEngine;

namespace Get_To_Multiplayer
{
    public class CollisionMP : MonoBehaviour
    {

        private void Awake()
        {
            this.TAG = GameObject.FindObjectOfType<Manager>().Tag;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name == "PlayerMultiplayerColl")
            {
                if (this.TAG)
                {
                    NetworkObject[] array = GameObject.FindObjectsOfType<NetworkObject>();
                    foreach (NetworkObject networkObject in array)
                    {
                        if (networkObject.IsOwner)
                        {
                            if (networkObject.transform.Find("Model").Find("model_RIG_UPDATED").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("ToRed"))
                            {
                                other.transform.parent.Find("Model").Find("model_RIG_UPDATED").GetComponent<NetworkAnimator>().SetTrigger("Red");
                                networkObject.transform.Find("Model").Find("model_RIG_UPDATED").GetComponent<NetworkAnimator>().SetTrigger("Normal");
                            }
                            else
                            {
                                Debug.Log("NOT NAME");
                            }
                        }
                    }
                }
                GameObject.Find("Player").transform.Find("PlayerPhysics").GetComponent<Rigidbody>().AddForce(-base.transform.forward * 7f, ForceMode.VelocityChange);
            }
        }
        private bool TAG;
    }
}
