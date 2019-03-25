using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocialPoint.Tools
{
    public delegate void CameraEventsHandler();

    public class CameraEvents : MonoBehaviour
    {
        public event CameraEventsHandler CameraEventEndForest;
        public event CameraEventsHandler CameraEventDungeon;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EndEnvForest"))
                CameraEventEndForest();

            if (other.CompareTag("EndDungeon"))
                CameraEventDungeon();
        }
    }
}