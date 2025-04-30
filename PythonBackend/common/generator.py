import asyncio
from ollama import AsyncClient
from retriever import *

__all__ = ['start', 'create_new_chat', 'chat', 'summarize_chat', 'create_text']

rapport_level_progress = 0
rapport_level = "Friend"
retrieved_context = "Karen is being rude at a store. She takes it out on the store worker."
messages = []

def start(player_name, data, retrieved_context):
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
Player Name: {player_name}

Relation With {player_name}
Level: {data.rapport_level}
Past Conversations: None

current_context = {retrieved_context}

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

def create_new_chat(player_name, data, retrieved_context):
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
Player Name: {player_name}

Relation With {player_name}
Level: {data.rapport_level}
Past Conversations: {retrieved_context}

current_context = See world data. {data.name} just ran into {player_name} there.

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
      
async def chat(player_name, intro, prompt):
    global messages

    messages.append({"role": "user", "content": prompt})

    response_content = f"""{intro}
### Instructions:
1. Respond only with dialogue in character.
2. If the player's message indicates they are saying goodbye, subtly acknowledge it in your reply and include the tag <GOODBYE> at the end (invisible to the player).
user: {prompt}
### Response:"""

    async for part in await AsyncClient().chat(
        model="hf.co/TheBloke/MythoMist-7B-GGUF:Q4_K_M", messages=messages, stream=True
    ):
        chunk = part['message']['content']
        response_content += chunk

    messages.append({"role": "assistant", "content": response_content})

    print(response_content)

    if "<GOODBYE>" in response_content:
        # Optionally remove it before sending to Unity
        clean_response = response_content.replace("<GOODBYE>", "").strip()
        return { "response": clean_response, "goodbye": "true" }
    return { "response": response_content, "goodbye": "false" }

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

def create_text(character_name, title):
    global messages

    text = title + "\n\n"
    for message in messages:
        message_content = message['role'] + message['content']
        text += message_content + "\n"

    return text