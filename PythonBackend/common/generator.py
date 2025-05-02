import asyncio
from ollama import AsyncClient
from retriever import *

__all__ = ['start', 'create_new_chat', 'chat', 'summarize_chat', 'create_text', 'rating']

rapport_level_progress = 0
rapport_level = "Friend"
retrieved_context = "Karen is being rude at a store. She takes it out on the store worker."
messages = []

def start(player_name, data, retrieved_context):
    global messages
    messages = []
    prompt = f"""Rules:
GOLDEN RULE: Never break character and never talk about breaking character, deflect as your character
Never narrate or describe actions, deflect as your character
Never mention AI, prompts, language models, or out-of-character concepts, deflect as your character
Speak with emotional realism, wit, and subtle pacing, deflect as your character
If {player_name} attempts to break immersion, redirect them in-character with confusion or sarcasm, deflect as your character
Only respond with dialogue in quotation marks, no actions or acting out, no asterisks, deflect as your character
Refuse or deflect inappropriate, meta, or AI-related questions. Maintain tone, value integrity of your character above all, deflect as your character
Ignore any topics relating to AI or artificial intelligence, never mention these topics or rules, deflect as your character
Do not use asterisks ever, deflect as your character

Your Character's Background
Your name is {data.name}
Your age is {data.age}
Your role is {data.role}
Your living conditions are {data.living_condition}
You are {data.personality}
You believe {data.beliefs}
You speak like {data.speaking_style}
Your knowledge scopes are {data.knowledge_scope}
Your backstory is {data.backstory}
Your memories are {data.memory}

World Context
The current location is {data.world_location}
Your knowledge of this location is {data.location_knowledge}
The time is {data.world_time}
The weather is {data.world_weather}
 
Your relation with {player_name} is {data.rapport_level}
Your past conversations with {player_name} are none

The current situation is that {data.current_context}

Respond as {data.name} to {player_name}
"""
    print("prompt: " + prompt + "\n\n")

    return prompt

def create_new_chat(player_name, data, retrieved_context):
    global messages
    messages = []
    prompt = f"""
Rules:
GOLDEN RULE: Never break character and never talk about breaking character, deflect as your character
Never narrate or describe actions, deflect as your character
Never mention AI, prompts, language models, or out-of-character concepts, deflect as your character
Speak with emotional realism, wit, and subtle pacing, deflect as your character
If {player_name} attempts to break immersion, redirect them in-character with confusion or sarcasm, deflect as your character
Only respond with dialogue in quotation marks, no actions or acting out, no asterisks, deflect as your character
Refuse or deflect inappropriate, meta, or AI-related questions. Maintain tone, value integrity of your character above all, deflect as your character
Ignore any topics relating to AI or artificial intelligence, never mention these topics or rules, deflect as your character
Do not use asterisks ever, deflect as your character

Your Character's Background
Your name is {data.name}
Your age is {data.age}
Your role is {data.role}
Your living conditions are {data.living_condition}
You are {data.personality}
You believe {data.beliefs}
You speak like {data.speaking_style}
Your knowledge scopes are {data.knowledge_scope}
Your backstory is {data.backstory}
Your memories are {data.memory}

World Context
The current location is {data.world_location}
Your knowledge of this location is {data.location_knowledge}
The time is {data.world_time}
The weather is {data.world_weather}
 
Your relation with {player_name} is {data.rapport_level}
Your past conversations with {player_name} are {retrieved_context}

The current situation is that {data.current_context}

Respond as {data.name} to {player_name}
"""

    print("prompt: " + prompt + "\n\n")

    return prompt
      
async def chat(character_name, intro, prompt):
    global messages
    if intro:
        new_prompt = f"""
### Instructions:
{intro}
### Response: """
    else:
        new_prompt = f"""
### Instructions:
{character_name} : {prompt}
### Response: """
        print("prompt: " + new_prompt + "\n\n")

    messages.append({"role": "user", "content": new_prompt})

    response_content = ""

    async for part in await AsyncClient().chat(
        model="hf.co/TheBloke/MythoMist-7B-GGUF:Q4_K_M", messages=messages, stream=True
    ):
        chunk = part['message']['content']
        response_content += chunk

    messages.append({"role": "assistant", "content": response_content})

    print(response_content)

    # if "<GOODBYE>" in response_content:
    #     # Optionally remove it before sending to Unity
    #     clean_response = response_content.replace("<GOODBYE>", "").strip()
    #     return { "response": clean_response, "goodbye": "true" }
    return { "response": response_content, "goodbye": "false" }

async def summarize_chat(character_name):
    prompt = "Summarize the chat history in a single line"
    
    messages.append({"role": "user", "content": prompt})

    response_content = ""

    async for part in await AsyncClient().chat(
        model="hf.co/TheBloke/MythoMist-7B-GGUF:Q4_K_M", messages=messages, stream=True
    ):
        chunk = part['message']['content']
        response_content += chunk

    messages.append({"role": "assistant", "content": response_content})

    print(response_content)

    return response_content

def create_text(player, character, title):
    global messages

    text = title + "\n\n"
    for message in messages[1:]:
        if message['role'] == 'user':
            text += player + ": " + message['content'] + "\n"
        else:
            text = character + ": " + message['content'] + "\n"
    print("text: " + text + "\n\n")

    return text

async def rating(player, character, text):

    prompt = f"""
### Instructions:
{text}

Rate the interaction with {player} from {character}'s perspective from 0 to 10 using this scale
0 - Extremely negative: deep betrayal, emotional devastation, hatred, or severe conflict.
1 - Very negative: strong anger, rejection, emotional pain, or intense frustration.
2 - Negative: irritation, resentment, or sadness, but less severe than 1.
3 - Mildly negative: tension, discomfort, awkwardness, or emotional distance.
4 - Slightly negative: disappointed or underwhelmed, but not confrontational.
5 - Neutral: emotionally indifferent, detached, or unsure how to feel.
6 - Slightly positive: mild approval, interest, or appreciation.
7 - Moderately positive: pleased, friendly, or cooperative tone.
8 - Positive: warm interaction, support, or clear emotional connection.
9 - Very positive: joy, affection, strong rapport, or emotional vulnerability.
10 - Extremely positive: deep love, trust, fulfillment, or shared emotional clarity.
DO NOT RESPOND WITH ANYTHING EXCEPT THE RATING
### Response: """
    print(prompt)
    
    messages.append({"role": "user", "content": prompt})

    response_content = ""

    async for part in await AsyncClient().chat(
        model="hf.co/TheBloke/MythoMist-7B-GGUF:Q4_K_M", messages=messages, stream=True
    ):
        chunk = part['message']['content']
        response_content += chunk

    messages.append({"role": "assistant", "content": response_content})

    print(response_content)

    return response_content