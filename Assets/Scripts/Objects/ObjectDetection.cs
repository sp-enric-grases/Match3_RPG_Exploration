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
        public LootManager lootManager;
        public Vector2 strength = new Vector2(100, 100);
        public float distancethreshold = 0.5f;

        protected int countLoot;
        protected int numLoot;

        protected void CreateLoot(LootManager lm)
        {
            for (int i = 0; i < numLoot; i++)
            {
                GameObject newLoot = Instantiate(loot, lootRoot.transform.position, Quaternion.identity);
                newLoot.transform.position += new Vector3(Random.Range(-distancethreshold, distancethreshold), Random.Range(-distancethreshold, distancethreshold), Random.Range(-distancethreshold, distancethreshold));
                newLoot.GetComponent<Loot>().SettingConstraintProperties();
                newLoot.SetActive(true);

                Vector3 dir = newLoot.transform.position - Camera.main.transform.position;
                newLoot.GetComponent<Rigidbody>().AddForce(Vector3.up * strength.y - dir * strength.x);

                lootManager.StartCounting(newLoot.GetComponent<Loot>(), true);
            }
        }
    }
}