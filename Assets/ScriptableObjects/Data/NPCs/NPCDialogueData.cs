using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.NPCs
{
    [System.Serializable]
    public class NPCDialogueData
    {
        [SerializeField] public string name;
        [SerializeField] public string age;
        [SerializeField] public string role;
        [SerializeField] public string livingCondition;
        [SerializeField] public string personality;
        [SerializeField] public string beliefs;
        [SerializeField] public string speakingStyle;
        [SerializeField] public string knowledgeScope;
        [SerializeField] public string backstory;
        [SerializeField] public string memory;
    }
}
