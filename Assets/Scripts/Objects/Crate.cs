using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocialPoint.Tools
{
    public class Crate : ObjectDetection
    {
        [Header("Private properties")]
        public GameObject brokenCrate;
        
        private List<GameObject> loots = new List<GameObject>();

        void Start()
        {
            numLoot = Random.Range(minLoot, maxLoot + 1);
            countLoot = 0;
        }

        public void CreateLootCrate()
        {
            CreateBrokenCrate();
            CreateLoot(lootManager);
            Destroy(gameObject);
        }

        public void CreateBrokenCrate()
        {
            GameObject newCrate = Instantiate(brokenCrate, transform);
            newCrate.transform.localPosition = Vector3.zero;
            newCrate.transform.localRotation = Quaternion.identity;
            newCrate.transform.localScale = Vector3.one;
            newCrate.transform.parent = null;

            AudioManager.Instance.PlayEffect(AudiosData.CRATE);
        }
    }
}