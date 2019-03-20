using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SocialPoint.Tools
{
    public class ObjectDetection : MonoBehaviour
    {
        

        [Header("Loot")]
        public int minLoot = 3;
        public int maxLoot = 5;

        protected int countLoot;
        protected int numLoot;
    }
}