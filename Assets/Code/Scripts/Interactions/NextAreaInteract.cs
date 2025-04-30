using BobaStop.Data.Level;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop.Interactions {
    public class NextAreaInteract : Interactable {
        [SerializeField] LevelProperties nextArea;
        
        public override void Interact() {
            GameManager.Instance.levelManagerHelper.SwitchScene(nextArea);
        }
        
        
    }
}
