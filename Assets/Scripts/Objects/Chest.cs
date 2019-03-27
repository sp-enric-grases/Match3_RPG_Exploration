using UnityEngine;

namespace SocialPoint.Tools
{
    public class Chest : ObjectDetection
    {
        public CanvasManager cm;
        private Animator anim;
        private bool chestIsOpen = false;

        void Start()
        {
            anim = GetComponent<Animator>();
            numLoot = Random.Range(minLoot, maxLoot + 1);
            countLoot = 0;
        }

        private void FixedUpdate()
        {
            int layerMask = 1 << LayerMask.NameToLayer("InteractableObject");
            Vector3 physicsCameraCompenstion = Vector3.down * 0.2f;
            Vector3 fwd = Camera.main.transform.TransformDirection(Vector3.forward + physicsCameraCompenstion);
        }

        public void OpenChest()
        {
            if (cm != null) cm.InventoryState(true, "ShowInventory");

            anim.SetBool("openChest", true);
            AudioManager.Instance.PlayEffect(AudiosData.CHEST);
        }

        public void CreateLootChest()
        {
            if (chestIsOpen) return;

            chestIsOpen = true;
            OpenChest();
            CreateLoot(lootManager);
            Destroy(GetComponent<SphereCollider>());
        }

        public void StopParticles()
        {
            anim.SetBool("empty", true);
            countLoot++;
        }

        private TypeOfLoot GetChestLoot()
        {
            System.Array values = System.Enum.GetValues(typeof(TypeOfChestLoot));
            return (TypeOfLoot)(TypeOfChestLoot)values.GetValue(Random.Range(1, values.Length));
        }
    }
}


