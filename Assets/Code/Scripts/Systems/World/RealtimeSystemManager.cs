namespace BobaStop.Systems.World
{
    public abstract class RealtimeSystemManager {
        protected bool isPaused;

        public virtual void Start() {
            isPaused = true;
        }

        public virtual void Update() {
            if (isPaused) return;
        }
        
        public void Pause() {
            isPaused = true;
        }

        public void Unpause() {
            isPaused = false;
        }

        public virtual void Reset() {
            
        }
    }
}
