namespace BobaStop.Systems.World
{
    public abstract class RealtimeSystemManager {
        protected bool isPaused;
        
        public abstract void Start();
        
        public abstract void Update();
        
        public virtual void Pause() {
            isPaused = true;
        }

        public virtual void Unpause() {
            isPaused = false;
        }

        public virtual void Reset() {
            
        }
    }
}
