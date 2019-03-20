using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocialPoint.Tools
{
    public class Crate : ObjectDetection
    {
        [Header("Private properties")]
        public GameObject brokenCrate;
        public GameObject lootRoot;
        public GameObject loot;
        public Vector2 strength = new Vector2(100, 100);
        
        private List<GameObject> loots = new List<GameObject>();

        void Start()
        {
            numLoot = Random.Range(minLoot, maxLoot + 1);
            countLoot = 0;
        }

        public void CreateLootCrate()
        {
            CreateBrokenCrate();
            CreateLoot();
            gameObject.SetActive(false);
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

        private void CreateLoot()
        {
            for (int i = 0; i < numLoot; i++)
            {
                GameObject newLoot = Instantiate(loot, lootRoot.transform.position, Quaternion.identity);
                newLoot.GetComponent<Loot>().SettingConstraintProperties();
                newLoot.SetActive(true);
                Vector3 dir = newLoot.transform.position - Camera.main.transform.position;

                newLoot.GetComponent<Rigidbody>().AddForce(Vector3.up * strength.y - dir * strength.x);
            }

            Destroy(gameObject);
        }
    }
}