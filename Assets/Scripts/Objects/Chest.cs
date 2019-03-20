using UnityEngine;

namespace SocialPoint.Tools
{
    public class Chest : ObjectDetection
    {
        public CanvasManager cm;
        //public struct ObjectArgs
        //{
        //    public Vector3 fwd;
        //    public RaycastHit hit;
        //}

        //[Header("Private properties")]
        //public float angleThreshold = 3;

        private Animator anim;
        //private bool hasShined = false;

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
            //RaycastHit hit;

            //if (Physics.Raycast(Camera.main.transform.position, fwd, out hit, 75, layerMask) && hit.collider.CompareTag("Chest"))
            //    Shine(new ObjectArgs { fwd = fwd, hit = hit });
            //else
            //    hasShined = false;
        }

        //private void Shine(ObjectArgs args)
        //{
        //    float angle = 180 - Vector3.Angle(args.fwd, (Camera.main.transform.position - args.hit.collider.gameObject.transform.position));

        //    if (angle < angleThreshold)
        //    {
        //        if (!hasShined)
        //        {
        //            hasShined = true;
        //            args.hit.collider.GetComponent<Animator>().SetTrigger("shine");
        //        }
        //    }
        //}

        public void OpenChest()
        {
            if (cm != null) cm.ShowInventary();

            anim.SetBool("openChest", true);
            AudioManager.Instance.PlayEffect(AudiosData.CHEST);
            AudioManager.Instance.PlayEffect(AudiosData.TREASURE);
            countLoot++;
        }

        public void CreateLootChest(CanvasManager cm, Vector3 pos)
        {
            if (countLoot > numLoot) return;

            if (countLoot == 0)
                OpenChest();
            else
            {
                if (countLoot < numLoot)
                {
                    countLoot++;

                    TypeOfLoot loot = GetChestLoot();

                    if (loot == TypeOfLoot.Coins)
                        cm.CreateRainCoins(pos);
                    else
                        cm.CreateCanvasLoot(pos, loot);

                    if (countLoot == numLoot)
                        StopParticles();
                }
            }
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


