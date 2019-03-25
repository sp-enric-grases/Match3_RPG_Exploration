using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocialPoint.Tools
{
    public class SetInitScene : MonoBehaviour
    {
        public GameObject path1;
        public GameObject path2;

        void Awake()
        {
            if (!GameState.startFromBegining)
            {
                path1.SetActive(false);
                path2.SetActive(true);
                path2.GetComponent<FlyThroughPath>().cameraStartsFlying = true;
                path2.transform.Find("Container").gameObject.SetActive(false);
            }
        }

        private void SetInitValues(bool state)
        {

        }
    }
}
