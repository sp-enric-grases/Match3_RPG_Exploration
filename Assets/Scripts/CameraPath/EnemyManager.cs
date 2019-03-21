using System.Collections.Generic;
using UnityEngine;

namespace SocialPoint.Tools
{
    public class EnemyManager : MonoBehaviour
    {
        public GameObject lootRoot;
        public GameObject loot;
        public int minLootAmount = 1;
        public int maxLootAmount = 5;
        public int numberOfHits = 3;

        private float threshold = 0.2f;

        private List<GameObject> loots = new List<GameObject>();
        private Animator anim;

        void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void CreateLoot()
        {
            int numberOfLoots = Random.Range(minLootAmount, maxLootAmount + 1);

            for (int i = 0; i < numberOfLoots; i++)
            {
                GameObject newLoot = Instantiate(loot, lootRoot.transform.position, Quaternion.identity, lootRoot.transform);
                newLoot.transform.position += new Vector3(Random.Range(-threshold, threshold), Random.Range(-threshold, threshold), Random.Range(-threshold, threshold));
                loots.Add(newLoot);
                newLoot.GetComponent<Loot>().SettingConstraintProperties();
                newLoot.transform.parent = null;
            }
        }

        public void Shake()
        {
            AudioManager.Instance.PlayEffect(AudiosData.SLASH);
            anim.SetTrigger("shake");
            numberOfHits--;
        }

        public void Die()
        {
            anim.SetTrigger("die");

            foreach (var item in loots)
                item.SetActive(true);

            AudioManager.Instance.PlayEffect(AudiosData.DEATH);
            string grunt = "Death_" + Random.Range(1, 6);
            AudioManager.Instance.PlayEffect(grunt);
        }

        public void Dissapear()
        {
            Destroy(gameObject);
        }
    }
}