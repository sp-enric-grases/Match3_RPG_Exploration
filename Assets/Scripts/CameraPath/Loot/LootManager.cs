using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SocialPoint.Tools
{
    public class LootManager : MonoBehaviour
    {
        public float autoLootTime = 2;
        public float thresholdTime = 0.3f;
        public bool startCounting = false;

        public List<Loot> lootList = new List<Loot>();

        private float lootCounter;
        private CanvasManager cm;

        void Start()
        {
            cm = GetComponent<CanvasManager>();
            lootCounter = autoLootTime;
        }

        void Update()
        {
            if (cm.numberOfEnemies > 0) return;

            if (startCounting)
            {
                lootCounter -= Time.deltaTime;

                if (lootCounter < 0)
                {
                    startCounting = false;
                    StartCoroutine(AutoLoot());
                }
            }
        }

        public void StartCounting(Loot loot, bool createLoot)
        {
            if (createLoot)
                lootList.Add(loot);
            else
                lootList.Remove(loot);

            startCounting = true;
            lootCounter = autoLootTime;
        }

        IEnumerator AutoLoot()
        {
            yield return new WaitForSeconds(thresholdTime);

            Loot loot = lootList[0];

            Vector3 lootPosition = 1 / GetComponent<RectTransform>().localScale.x * Camera.main.WorldToViewportPoint(loot.gameObject.transform.position);
            Vector2 screenPoint = new Vector2(lootPosition.x * Screen.width, lootPosition.y * Screen.height);

            cm.CreateCanvasLoot(screenPoint, loot.loot);
            Destroy(loot.gameObject);
            lootList.Remove(loot);

            if (lootList.Count > 0)
                StartCoroutine(AutoLoot());
        }
    }
}