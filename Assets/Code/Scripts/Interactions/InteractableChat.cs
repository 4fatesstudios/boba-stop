using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BobaStop.NPCs;
using BobaStop.Systems;
using Microsoft.Extensions.AI;
using UnityEngine;

namespace BobaStop.Interactions {
    public class InteractableChat : Interactable {
        [SerializeField] private string chatText;
        [SerializeField] private CompanionData companionData;

        [SerializeField] private OllamaGenerator generator;

        [SerializeField] private string responseText;

        public void Start(){
            this.generator = new OllamaGenerator(companionData);
            generator.Start();
        }
        public async void Interact() {
            Debug.Log(chatText);
            responseText = await generator.Chat(chatText);
        }

        public string GetInteractText() {
            return responseText;
        }
    }
}