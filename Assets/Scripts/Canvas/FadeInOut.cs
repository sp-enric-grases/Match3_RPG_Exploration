using UnityEngine;
using UnityEngine.UI;

namespace SocialPoint.Tools
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeInOut : MonoBehaviour
    {
        public enum TypeOfFade { In, Out}

        public TypeOfFade typeOfFade;
        public Color color = Color.black;
        public float fadeDuration = 1;
        public string sceneToLoad = "";

        private CanvasGroup canvas;

        void Awake()
        {
            GetComponent<Image>().color = color;
            canvas = GetComponent<CanvasGroup>();

            canvas.alpha = typeOfFade == TypeOfFade.In ? 1 : 0;
        }

        void Update()
        {
            if (typeOfFade == TypeOfFade.In)
                FadeIn();
            else
                FadeOut();
        }

        private void FadeIn()
        {
            canvas.alpha -= Time.deltaTime / fadeDuration;

            if (canvas.alpha <= 0)
                Destroy(gameObject);
        }

        private void FadeOut()
        {
            canvas.alpha += Time.deltaTime / fadeDuration;

            if (canvas.alpha >= 1)
            {
                if (!string.IsNullOrEmpty(sceneToLoad))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
                }
                else
                    Destroy(this);
            }
        }

    }
}