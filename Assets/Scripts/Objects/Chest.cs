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
            if (cm != null) cm.ShowInventary();

            anim.SetBool("openChest", true);
            AudioManager.Instance.PlayEffect(AudiosData.CHEST);
            //AudioManager.Instance.PlayEffect(AudiosData.TREASURE);
            //countLoot++;
        }

        //public void CreateLootChest(CanvasManager cm, Vector3 pos)
        //{
        //    if (countLoot > numLoot) return;

        //    if (countLoot == 0)
        //        OpenChest();
        //    else
        //    {
        //        if (countLoot < numLoot)
        //        {
        //            countLoot++;

        //            TypeOfLoot loot = GetChestLoot();

        //            if (loot == TypeOfLoot.Coins)
        //                cm.CreateRainCoins(pos);
        //            else
        //                cm.CreateCanvasLoot(pos, loot);

        //            if (countLoot == numLoot)
        //                StopParticles();
        //        }
        //    }
        //}

        public void CreateLootChest()
        {
            if (chestIsOpen) return;

            chestIsOpen = true;
            OpenChest();
            CreateLoot();
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


