using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Get_To_Multiplayer
{
    public class Billboard : MonoBehaviour
    {
        public void LateUpdate()
        {
            if (Camera.main != null)
            {
                this.mainCamera = Camera.main;
                base.transform.LookAt(this.mainCamera.transform);
                base.transform.Rotate(0f, 180f, 0f);
            }
        }

        private Camera mainCamera;
    }
}
