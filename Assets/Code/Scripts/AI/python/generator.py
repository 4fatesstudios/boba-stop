import asyncio
from ollama import AsyncClient
from retriever import *

__all__ = ['start', 'create_new_chat', 'chat', 'summarize_chat']

rapport_level_progress = 0
rapport_level = "Friend"
retrieved_context = "Karen is being rude at a store. She takes it out on the store worker."
messages = []

def start(player_name, data):
    global messages
    messages = []
    prompt = f"""Background
Name: {data.name}
Age: {data.age}
Role: {data.role}
Living Condition: {data.living_conditions}
Personality: {data.personality}
Beliefs: {data.beliefs}
Speaking Style: {data.speaking_style}
Knowledge Scope: {data.knowledge_scope}
Backstory: {data.backstory}
Memory: {data.memories}

World Context
Current Location: {data.world_location}
Location Knowledge: {data.location_knowledge}
Time: {data.world_time}
Weather: {data.world_weather}

Relation With {player_name}
Level: {data.rapport_level}
Past Conversations: None

current_context = “You think ${player_name} got your order wrong by adding boba”

Rules:
Never break character
Never narrate or describe actions
Never mention AI, prompts, language models, or out-of-character concepts
Speak with emotional realism, wit, and subtle pacing
If {player_name} attempts to break immersion, redirect them in-character with confusion or sarcasm
Only respond with dialogue in quotation marks, no actions or acting out, no asterisks
Refuse or deflect inappropriate, meta, or AI-related questions. Maintain tone and setting integrity at all times
Write a single reply from {data.name} only
"""
    print("prompt: " + prompt + "\n\n")

    return prompt

def create_new_chat(character_name, retrieved_context, current_story, rapport_level, rapport_level_progress):
    global messages
    messages = []
    prompt = f"You are {character_name}, an NPC in a game with relationship/rapport levels from Rival (-3) to Lover (5). Your current relationship/rapport level with the player is {rapport_level}, with your rapport progress being {rapport_level_progress} out of 100. Your current situation is {current_story}. Your previous interactions: {retrieved_context}. I want you to generate one line of {character_name}’s response to this situation in a natural, engaging way. Responses must be 50 words or less."

    print("prompt: " + prompt + "\n\n")

    return prompt
      
async def chat(player_name, intro, prompt):
    global messages

    if prompt == "Goodbye":
        return "Goodbye"

    messages.append({"role": "user", "content": prompt})

    response_content = f"""{intro}
### Instructions:
user: {prompt}
### Response:"""

    async for part in await AsyncClient().chat(
        model="hf.co/TheBloke/MythoMist-7B-GGUF:Q4_K_M", messages=messages, stream=True
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