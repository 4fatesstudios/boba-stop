import asyncio
from ollama import AsyncClient
from retriever import *

__all__ = ['start', 'create_new_chat', 'chat', 'summarize_chat']

rapport_level_progress = 0
rapport_level = "Friend"
retrieved_context = "Karen is being rude at a store. She takes it out on the store worker."
messages = []

def start(character_name, start_context):
    global messages
    messages = []
    prompt = f"You are {character_name}, an NPC in a game with relationship/rapport levels from Rival (-3) to Lover (5). You are just meeting the player, so your rapport level is 0 (Neutral) and rapport level progress is 0. Your personality is as follows: {start_context}. I want you to generate one line of {character_name}’s response to this situation in a natural, engaging way."

    print("prompt: " + prompt + "\n\n")

    return prompt

def create_new_chat(character_name, retrieved_context, current_story, rapport_level, rapport_level_progress):
    global messages
    messages = []
    prompt = f"You are {character_name}, an NPC in a game with relationship/rapport levels from Rival (-3) to Lover (5). Your current relationship/rapport level with the player is {rapport_level}, with your rapport progress being {rapport_level_progress} out of 100. Your current situation is {current_story}. Your previous interactions: {retrieved_context}. I want you to generate one line of {character_name}’s response to this situation in a natural, engaging way. Responses must be 50 words or less."

    print("prompt: " + prompt + "\n\n")

    return prompt
      
async def chat(character_name, prompt):
    global messages

    if prompt == "Goodbye":
        return "Goodbye"

    messages.append({"role": "user", "content": prompt})

    response_content = ""

    async for part in await AsyncClient().chat(
        model="llama3.2", messages=messages, stream=True
    ):
        chunk = part['message']['content']
        response_content += chunk

    messages.append({"role": "assistant", "content": response_content})

    print(response_content)

    return response_content

async def summarize_chat(character_name, chat_history):
    prompt = "Summarize the chat history in a single line"
    
    messages.append({"role": "user", "content": prompt})

    response_content = ""

    async for part in await AsyncClient().chat(
        model="llama3.2", messages=messages, stream=True
    ):
        chunk = part['message']['content']
        response_content += chunk

    messages.append({"role": "assistant", "content": response_content})

    print(response_content)

    return response_content