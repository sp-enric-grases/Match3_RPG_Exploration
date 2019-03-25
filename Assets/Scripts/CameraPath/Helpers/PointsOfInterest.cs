using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocialPoint.Tools
{
    public class PointsOfInterest : MonoBehaviour
    {
        public float timeToRelocation;
        public float angleThreshold;
        public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public List<Transform> pointsOfInterest;

        private bool isRecentering;
        private bool haveBeenRecentered;
        private float counterFinalRelocation = 0;
        private bool mouseIsDrag = false;

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                //mouseIsDrag = false;
                GetComponent<CameraRotation>().enabled = true;
            }
            else
            {
                CheckIfRelocate();
            }
            
            Debug.DrawLine(pointsOfInterest[0].position, transform.position, Color.green);

            //if (!mouseIsDrag) CheckIfRelocate();

            //if (isRecentering)
            //{
            //    RecenterCamera(targetDir);
            //}

            //Debug.Log("Is Recentering: " + isRecentering);
            //Debug.Log("Have been recentered: " + haveBeenRecentered);
        }

        private void RecenterCamera(Vector3 target)
        {
            counterFinalRelocation += Time.deltaTime / timeToRelocation;
            Quaternion toRotation = Quaternion.FromToRotation(transform.forward, target);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, curve.Evaluate(counterFinalRelocation));

            //if (counterFinalRelocation >= timeToRelocation)
            //{
            //    counterFinalRelocation = 0;
            //    isRecentering = false;
            //    haveBeenRecentered = true;
            //    GetComponent<CameraRotation>().SetInitRotations(transform.eulerAngles);
            //    GetComponent<CameraRotation>().enabled = true;
            //}
        }

        void OnGUI()
        {
            if (Event.current.type == EventType.MouseDrag) mouseIsDrag = true;
        }

        private void CheckIfRelocate()
        {
            Vector3 targetDir = pointsOfInterest[0].position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);

            if (angle < angleThreshold)
            {

                //RecenterCamera(targetDir);
                GetComponent<CameraRotation>().enabled = false;
                Debug.DrawLine(pointsOfInterest[0].position, transform.position, Color.magenta);
            }
        }
    }
}