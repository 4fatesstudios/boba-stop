using UnityEngine;

namespace SimpleCombat.Components {
    public class Hitbox : MonoBehaviour {
        [SerializeField] private BoxCollider hitbox = null;
        
        public void Start() {
            if (hitbox == null) {
                Debug.LogWarning("SimpleCombat: No box collider on " + gameObject.name);
            }
            
        }
    }
}