using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace SocialPoint.Tools
{
    public delegate void LootHasArrived(TypeOfLoot typeOfLoot);

    public enum TypeOfLoot
    {
        None    = 0,
        Chalice = 1,
        Coins   = 2,
        Dagger  = 3,
        Stone   = 4,
        Feather = 5,
        Gems    = 6,
        Iron    = 7,
        Health  = 8,
        Herbs   = 9,
        Magic   = 10,
        Potion  = 11,
        Wood    = 12,
        Coin    = 100
    }

    public enum TypeOfChestLoot
    {
        None = 0,
        Coins = 2,
        Iron = 7,
        Stone = 4,
        Wood = 12
    }

    public class Loot : MonoBehaviour
    {
        public event LootHasArrived LootHasArrivedRequest;

        public TypeOfLoot loot = TypeOfLoot.None;
        [Space(5)]
        public float time = 5;
        public AnimationCurve pos = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve scl = AnimationCurve.EaseInOut(0, 1, 1, 1);
        
        private Vector3 initPosition;
        private Vector3 destination;
        private bool move;
        private float counter = 0;
        private Inventory inventory;
        private LootSlot slot;
        private bool textAnimationIsRequested = false;
        private GameObject blob;

        public void Awake()
        {
            if (GetComponent<SpriteRenderer>())
            {
                Array values = Enum.GetValues(typeof(TypeOfLoot));
                loot = (TypeOfLoot)values.GetValue(UnityEngine.Random.Range(1, values.Length-1));
                gameObject.name = loot.ToString();

                SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                sprite.sprite = Resources.Load<Sprite>("Loot/" + loot.ToString());
                sprite.flipX = true;

                blob = transform.Find("Blob").gameObject;
                transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                CreateBlob();
            }
        }

        private void CreateBlob()
        {
            blob.transform.parent = null;
            blob.transform.localScale = Vector3.one;
            blob.transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);
            blob.GetComponent<PositionConstraint>().constraintActive = true;
        }

        public void SettingConstraintProperties()
        {
            ConstraintSource constraintSource = new ConstraintSource() { sourceTransform = Camera.main.transform, weight = 1 };
            LookAtConstraint rotConst = GetComponent<LookAtConstraint>();
            rotConst.constraintActive = true;
            rotConst.AddSource(constraintSource);
            rotConst.rotationOffset = Vector3.zero;
            rotConst.rotationAtRest = Vector3.zero;
        }

        public void Init(Inventory inventory, LootSlot slot, bool isCoin)
        {
            string typeOfLoot = isCoin ? "Coin" : slot.loot.ToString();
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Loot/" + typeOfLoot);

            this.inventory = inventory;
            this.slot = slot;
            this.destination = slot.transform.position;
            initPosition = transform.position;
            move = true;

            PlaySound(slot.loot);
        }

        private void PlaySound(TypeOfLoot loot)
        {
            string clip = string.Empty;

            switch (loot)
            {
                case TypeOfLoot.Coins: clip = AudiosData.COIN; break;
                case TypeOfLoot.Health: clip = AudiosData.HEALTH; break;
                case TypeOfLoot.Iron: clip = AudiosData.IRON; break;
                case TypeOfLoot.Potion: clip = AudiosData.POTION; break;
                case TypeOfLoot.Stone: clip = AudiosData.STONE; break;

                default: clip = AudiosData.GENERIC; break;
            }

            AudioManager.Instance.PlayEffect(clip, 0.3f);
        }

        private void Update()
        {
            if (!move) return;

            counter += Time.deltaTime / time;
            transform.position = initPosition + ((destination-initPosition) * pos.Evaluate(counter));
            transform.localScale = new Vector3(scl.Evaluate(counter), scl.Evaluate(counter), scl.Evaluate(counter));

            if (counter > 0.75 && !textAnimationIsRequested)
            {
                textAnimationIsRequested = true;
                LootHasArrivedRequest(loot);
            }

            if (counter > 1)
            {
                inventory.UpdateNumber(slot);

                Destroy(blob);
                Destroy(gameObject);
            }
        }
    }
}