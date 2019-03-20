using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SocialPoint.Tools
{
    [Serializable]
    public class LootSlot
    {
        public Transform transform;
        public TypeOfLoot loot;
        public int amount;
    }

    public class Inventory : MonoBehaviour
    {
        public List<LootSlot> slots;

        void Start()
        {
            InitSlots();
        }

        private void InitSlots()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                Image image = child.GetComponent<Image>();

                LootSlot slot = new LootSlot()
                {
                    transform = child.transform,
                    loot = (TypeOfLoot)Enum.Parse(typeof(TypeOfLoot), image.sprite.name),
                    amount = UnityEngine.Random.Range(0, 4)
                };

                SetImageSlot(child, image, slot);

                slots.Add(slot);
            }
        }

        private static void SetImageSlot(GameObject child, Image image, LootSlot slot)
        {
            child.transform.Find("TXT_LootType").GetComponent<TMP_Text>().text = string.Format("{0}  x{1}", slot.loot.ToString(), slot.amount.ToString());
            image.color = slot.amount > 0 ? new Color(1, 1, 1, 1f) : new Color(1,1,1, 0.3f);
        }

        public void UpdateNumber(LootSlot slot)
        {
            slot.amount++;
            SetImageSlot(slot.transform.gameObject, slot.transform.gameObject.GetComponent<Image>(), slot);
        }
    }
}