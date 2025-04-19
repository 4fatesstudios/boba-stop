using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.NPCs
{
    [System.Serializable]
    public class NPCDialogueData {
        [SerializeField] public string worldContext; // context for the island itself and its history
        [SerializeField] public string npcLifeContext; // context for the life of this specific NPC
        [SerializeField] public string memoriesContext; // context for new memories created with the player
        [SerializeField] public string personality; // personality of the npc
        // [SerializeField] public string 
    }
}
