using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SocialPoint.Tools
{
    public class CamSlider : MonoBehaviour
    {
        public Slider slider;
        public GameObject reference;
        public float speed = 1;
        public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private GameObject cam;
        private bool recenter;
        private float counterFinalRelocation = 0;

        private void Awake()
        {
            cam = transform.Find("Main Camera").gameObject;
        }
        public void Update()
        {
            Vector3 pos = transform.localPosition;
            pos.x = slider.value;
            transform.localPosition = pos;

            if (recenter)
            {
                cam.GetComponent<CameraRotation>().enabled = false;
                RecenterCamera();
            }
        }

        public void Recenter()
        {
            recenter = true;
        }

        private void RecenterCamera()
        {
            counterFinalRelocation += Time.deltaTime / speed;
            Vector3 direction = reference.transform.position - cam.transform.position;
            Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, toRotation, curve.Evaluate(counterFinalRelocation));

            if (counterFinalRelocation >= speed)
            {
                counterFinalRelocation = 0;
                recenter = false;
                cam.GetComponent<CameraRotation>().SetInitRotations(cam.transform.eulerAngles);
                cam.GetComponent<CameraRotation>().enabled = true;
            }
        }
    }
}