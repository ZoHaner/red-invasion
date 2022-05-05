using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Bullets
{
    public class BulletsCollisionHandler
    {
        public BulletsCollisionHandler()
        {
        }
        
        private Dictionary<string, Action> objectTagsHandlers;
        public void OnBulletCollided(BulletView bulletView, GameObject collidedObject)
        {
            // get object tag
            // do corresponding action
        }
    }
}