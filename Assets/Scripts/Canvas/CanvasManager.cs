using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SocialPoint.Tools
{
    public class CanvasManager : MonoBehaviour
    {
        const float IPHONEXR_ASPECT_RATIO = 2.1f;
        const float IPAP_ASPECT_RATIO = 1.5f;
        const float iPHONEXR_INIT_POSITION = 90;
        const float SCALE_FACTOR_iPAD = 0.78f;

        public Camera cam;
        public GameObject iPhoneXReference;
        public GameObject container;
        public GameObject loot;
        public GameObject fadeIn;
        public GameObject fadeOut;
        public GameObject endCanvas;
        public Inventory inventory;

        private Animator anim;
        private List<EnemyManager> enemies;
        private bool enemiesAreReady = false;
        private List<GameObject> trigger;
        private Animator door;
        private LootManager lm;
        private bool inventoryIsVisible = false;

        [HideInInspector] public int numberOfEnemies;
        [HideInInspector] public int numberOfLoot;

        private bool mouseHasBeenDrag = false;

        void Start()
        {
            cam.gameObject.GetComponent<CameraEvents>().CameraEventEndForest += ShowEndCanvas;
            cam.gameObject.GetComponent<CameraEvents>().CameraEventDungeon += GoToDungeons;

            lm = GetComponent<LootManager>();
            anim = GetComponent<Animator>();

            SetCanvasFades();
            SetCanvasRatio();
        }

        private void SetCanvasFades()
        {
            fadeIn.SetActive(true);
            fadeOut.SetActive(false);
            endCanvas.SetActive(false);
        }

        private void SetCanvasRatio()
        {
            float aspectRatio = (float)Screen.height / (float)Screen.width;

            if (aspectRatio > IPHONEXR_ASPECT_RATIO)
            {
                iPhoneXReference.SetActive(true);

                Vector3 position = container.GetComponent<RectTransform>().anchoredPosition;
                position.y = iPHONEXR_INIT_POSITION;
                container.GetComponent<RectTransform>().anchoredPosition = position;
            }
            else if (aspectRatio <= IPAP_ASPECT_RATIO)
            {
                container.transform.localScale *= SCALE_FACTOR_iPAD;
                cam.sensorSize = new Vector2(30, cam.sensorSize.y);
                iPhoneXReference.SetActive(false);
            }
            else
            {
                iPhoneXReference.SetActive(false);
            }
        }

        void Update()
        {
            if (numberOfEnemies > 0) return;

            int layerMask = 1 << LayerMask.NameToLayer("InteractableObject");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 25, layerMask) && Input.GetMouseButtonUp(0) && !mouseHasBeenDrag)
            {
                Vector3 lootPosition = 1 / GetComponent<RectTransform>().localScale.x * Input.mousePosition;

                switch (hit.collider.tag)
                {
                    case "Loot":
                        lm.StartCounting(hit.collider.gameObject.GetComponent<Loot>(), false);
                        CreateCanvasLoot(lootPosition, hit.collider.gameObject.GetComponent<Loot>().loot);
                        DestroyImmediate(hit.collider.gameObject);
                        break;
                    case "Chest":
                        hit.collider.GetComponent<Chest>().CreateLootChest();
                        break;
                    case "Crate":
                        hit.collider.GetComponent<Crate>().CreateLootCrate();
                        break;
                }
            }

            if (Input.GetMouseButtonUp(0)) mouseHasBeenDrag = false;
        }

        public void CreateRainCoins(Vector3 pos)
        {
            int numCoins = Random.Range(5, 15);

            for (int i = 0; i < numCoins; i++)
                StartCoroutine(CreateCanvasCoin(pos, 0.15f*i));
        }

        IEnumerator CreateCanvasCoin(Vector3 pos, float time)
        {
            yield return new WaitForSeconds(time);
            CreateCanvasLoot(pos, TypeOfLoot.Coins, true);
        }

        public void CreateCanvasLoot(Vector3 pos, TypeOfLoot typeOfLoot, bool isCoin = false)
        {
            GameObject newLoot = Instantiate(this.loot, transform);
            newLoot.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            newLoot.transform.localScale = Vector3.one;
            newLoot.GetComponent<RectTransform>().anchoredPosition = pos;

            Loot loot = newLoot.GetComponent<Loot>();
            loot.loot = typeOfLoot;
            loot.Init(inventory, inventory.slots.Find(s => s.loot == loot.loot), isCoin);
            loot.LootHasArrivedRequest += TriggerTextAnimation;
        }

        void OnGUI()
        {
            if (Event.current.type == EventType.MouseDrag)
                mouseHasBeenDrag = true;
        }

        public void InventoryVisibility()
        {
            if (!GameState.canShowInventory) return;

            if (!inventoryIsVisible)
                anim.SetTrigger("ShowInventory");
            else
                anim.SetTrigger("HideInventory");

            cam.GetComponent<Animator>().SetBool("ShowInventory", !inventoryIsVisible);
            inventoryIsVisible = !inventoryIsVisible;
        }

        public void InventoryState(bool state, string animation)
        {
            if (inventoryIsVisible == !state)
            {
                anim.SetTrigger(animation);
                inventoryIsVisible = state;
            }

            cam.GetComponent<Animator>().SetBool("ShowInventory", state);
            cam.GetComponent<CameraRotation>().enabled = state;
            GameState.canShowInventory = state;
            GameState.canRotateCamera = state;
        }

        public void ShowTiles()
        {
            anim.SetTrigger("ShowTiles");
            cam.GetComponent<Animator>().SetBool("ShowInventory", true);
        }

        private void TriggerTextAnimation(TypeOfLoot loot)
        {
            inventory.slots.Find(s => s.loot == loot).transform.Find("TXT_LootType").GetComponent<Animator>().SetTrigger("scale");
        }

        public void PopulateEnemies(List<EnemyManager> enemies, List<GameObject> trigger, Animator door)
        {
            List<EnemyManager> enemiesList = new List<EnemyManager>();

            this.enemies = enemies;
            this.trigger = trigger;
            this.door = door;

            numberOfEnemies = 0;

            foreach (var item in enemies.ToList())
                if (item != null) enemiesList.Add(item);

            numberOfEnemies = enemiesList.Count;

            enemiesAreReady = false;

            float TIME_BETWEEN_ENEMIES = 0.25f;

            for (int i = 0; i < numberOfEnemies; i++)
                StartCoroutine(ShowEnemy(enemiesList[i], i * TIME_BETWEEN_ENEMIES, i == numberOfEnemies - 1 ? true : false));
        }

        public void HitEnemy(int enemy)
        {
            if (!enemiesAreReady) return;
            
            if (enemies[enemy] != null)
            {
                if (enemies[enemy].numberOfHits > 0)
                    enemies[enemy].Shake();
                else
                {
                    enemies[enemy].Die();
                    enemies[enemy] = null;
                    numberOfEnemies--;
                }
            }

            if (numberOfEnemies == 0)
            {
                InventoryState(true, "ShowInventory");

                foreach (var item in trigger)
                    item.SetActive(true);

                if (door != null)
                    door.SetBool("Open", true);
            }
        }

        IEnumerator ShowEnemy(EnemyManager enemy, float n, bool allShown)
        {
            yield return new WaitForSeconds(n);
            enemy.gameObject.GetComponent<Animator>().SetBool("init", true);

            if (allShown) enemiesAreReady = true;
        }

        private void ShowEndCanvas()
        {
            endCanvas.SetActive(true);
        }

        private void GoToDungeons()
        {
            fadeOut.SetActive(true);
        }

        public void RestartDemo()
        {
            AudioManager.Instance.ChangeMusicWithFade(AudiosData.MUSIC, 1, 0.7f);
            GameState.startFromBegining = true;
            GameState.canShowInventory = true;
            GameState.canRotateCamera = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Env_Forest");
        }
    }
}