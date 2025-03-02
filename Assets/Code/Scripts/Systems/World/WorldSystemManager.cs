using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Systems.World
{
    public abstract class WorldSystemManager {
        public bool isPaused;
        
        public abstract void Start();
        
        public abstract void Update();

        public abstract bool LoadData();

        public virtual void Pause() {
            isPaused = true;
        }

        public virtual void Unpause() {
            isPaused = false;
        }
        
    }
}
