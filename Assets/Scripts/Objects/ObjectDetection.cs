﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocialPoint.Tools
{
    public class ObjectDetection : MonoBehaviour
    {
        [Header("Loot")]
        public int minLoot = 3;
        public int maxLoot = 5;
        public GameObject lootRoot;
        public GameObject loot;
        public Vector2 strength = new Vector2(100, 100);
        public float threshold = 0.5f;

        protected int countLoot;
        protected int numLoot;

        protected void CreateLoot()
        {
            for (int i = 0; i < numLoot; i++)
            {
                GameObject newLoot = Instantiate(loot, lootRoot.transform.position, Quaternion.identity);
                newLoot.transform.position += new Vector3(Random.Range(-threshold, threshold), Random.Range(-threshold, threshold), Random.Range(-threshold, threshold));
                newLoot.GetComponent<Loot>().SettingConstraintProperties();
                newLoot.SetActive(true);

                Vector3 dir = newLoot.transform.position - Camera.main.transform.position;
                newLoot.GetComponent<Rigidbody>().AddForce(Vector3.up * strength.y - dir * strength.x);
            }
        }
    }
}