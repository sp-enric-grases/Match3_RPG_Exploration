using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocialPoint.Tools
{
    public class PointsOfInterest : MonoBehaviour
    {
        public float timeToRelocation = 1;
        public float angleThreshold = 10;
        public float maxDistance = 25;
        public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public GameObject pointsOfInterest;

        private List<Transform> points = new List<Transform>();
        private Vector3 direction;
        private Quaternion toRotation;
        private float counterFinalRelocation = 0;
        private bool recenter;

        private void Start()
        {
            for (int i = 0; i < pointsOfInterest.transform.childCount; i++)
            {
                if (pointsOfInterest.transform.GetChild(i).gameObject.activeSelf)
                    points.Add(pointsOfInterest.transform.GetChild(i));
            }
        }

        void Update()
        {
            if (!GameState.canRotateCamera) return;

            if (Input.GetMouseButton(0))
                ResetParameters();
            else if (recenter)
                RecenterCamera();
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].gameObject.activeSelf)
                        CheckIfRelocate(points[i].position);
                }
            }
        }

        private void CheckIfRelocate(Vector3 pos)
        {
            Vector3 targetDir = pos - transform.position;
            float distance = (pos - transform.position).magnitude;
            float angle = Vector3.Angle(targetDir, transform.forward);

            if (angle < angleThreshold && distance < maxDistance)
            {
                recenter = true;
                direction = pos - transform.position;
                toRotation = Quaternion.FromToRotation(Vector3.forward, direction);
                Vector3 rot = toRotation.eulerAngles;
                rot.z = 0;
                toRotation = Quaternion.Euler(rot);


                GetComponent<CameraRotation>().enabled = false;
            }
        }

        private void RecenterCamera()
        {
            counterFinalRelocation += Time.deltaTime / timeToRelocation;
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, curve.Evaluate(counterFinalRelocation));

            if (counterFinalRelocation >= 1)
                ResetParameters();
        }

        private void ResetParameters()
        {
            counterFinalRelocation = 0;
            recenter = false;
            GetComponent<CameraRotation>().SetInitRotations(transform.eulerAngles);
            GetComponent<CameraRotation>().enabled = true;
        }
    }
}