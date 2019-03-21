using UnityEngine;

namespace SocialPoint.Tools
{
    public class StartCamera : MonoBehaviour
    {
        public float cooldown = 3;
        public AudioClip clip;
        public GameObject arrow;
        public GameObject sideArrows;
        private Quaternion rotation;


        private void Start()
        {
            AudioManager.Instance.ChangeMusicWithFade(clip, 2, 0.3f);

            if (!GameState.startFromBegining)
            { 
                Destroy(this);
                //AudioManager.Instance.ChangeMusicWithFade(clip, 2, 0.7f);
            }

            rotation = transform.rotation;

            if (arrow != null)
                arrow.SetActive(false);
        }

        void Update()
        {
            if (rotation != transform.rotation)
            {
                cooldown -= Time.unscaledDeltaTime;

                if (cooldown < 0)
                {
                    if (sideArrows != null)
                        sideArrows.SetActive(false);

                    if (arrow != null)
                        arrow.SetActive(true);

                    GameState.startFromBegining = false;
                    Destroy(this);
                }
            }
        }
    }
}