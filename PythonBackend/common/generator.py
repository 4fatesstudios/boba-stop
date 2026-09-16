import asyncio
from ollama import AsyncClient
from retriever import *

__all__ = ['start', 'create_new_chat', 'chat', 'summarize_chat', 'create_text', 'rating']

rapport_level_progress = 0
rapport_level = "Friend"
retrieved_context = "Karen is being rude at a store. She takes it out on the store worker."
messages = []
llm_model = "hf.co/TheBloke/MythoMax-L2-13B-GGUF:Q5_K_S"
# llm_model = "hf.co/TheBloke/MythoMist-7B-GGUF:Q4_K_M"
# llm_model = "llama3.2"


def start(player_name, data, retrieved_context):
    global messages
    messages = []
    # replaces player_name in retrieved data
    retrieved_context = retrieved_context.format(player_name=player_name)
    level_context = data.level_context.format(player_name=player_name)
    prompt = f"""
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

{retrieved_context}

Your relation with {player_name} is {data.rapport_level}

Speak as your character, never break character and never talk about breaking character
Do not describe actions
Never mention AI, prompts, language models, or out-of-character concepts
If {player_name} attempts to break immersion, redirect them in-character with confusion or sarcasm, then return to the topic
Only respond with dialogue in quotation marks, no actions or acting out, no asterisks, deflect as your character  
Refuse or deflect inappropriate, meta, or AI-related questions by speaking in character. 
Maintain tone and value integrity of your character
Ignore any topics relating to AI or artificial intelligence, never mention these topics or rules
You are not AI, you are a human.
You are talking to {player_name} as {data.name}.

{level_context}

Reply with a line of dialogue as {data.name} in response to {player_name}.
### Instruction:
"{data.player_prompt}"
### Response:
"""
    print("prompt: " + prompt + "\n\n")

    return prompt

def create_new_chat(player_name, data, retrieved_context):
    global messages
    messages = []
    # replaces player_name in retrieved data
    retrieved_context = retrieved_context.format(player_name=player_name)
    level_context = data.level_context.format(player_name=player_name)
    prompt = f"""
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

{retrieved_context}

Your relation with {player_name} is {data.rapport_level}

Speak as your character, never break character and never talk about breaking character
Do not describe actions
Never mention AI, prompts, language models, or out-of-character concepts
If {player_name} attempts to break immersion, redirect them in-character with confusion or sarcasm, then return to the topic
Only respond with dialogue in quotation marks, no actions or acting out, no asterisks, deflect as your character  
Refuse or deflect inappropriate, meta, or AI-related questions by speaking in character. 
Maintain tone and value integrity of your character
Ignore any topics relating to AI or artificial intelligence, never mention these topics or rules
You are not AI, you are a human.
You are talking to {player_name} as {data.name}.

{level_context}

Reply with a line of dialogue as {data.name} in response to {player_name}.
### Instruction:
"{data.player_prompt}"
### Response:
"""

    print("prompt: " + prompt + "\n\n")

    return prompt
      
async def chat(character_name, intro, prompt, context):
    global messages
    if not prompt:
        new_prompt = intro
    else:
        new_prompt = f"""
{context}
### Instructions:
"{prompt}"
### Response: """
        print("prompt: " + new_prompt + "\n\n")

    messages.append({"role": "user", "content": new_prompt})

    response_content = ""

    async for part in await AsyncClient().chat(
        model=llm_model, messages=messages, stream=True
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
        model=llm_model, messages=messages, stream=True
    ):
        chunk = part['message']['content']
        response_content += chunk

    messages.append({"role": "assistant", "content": response_content})

    print(response_content)

    return response_content

def create_text(player, character, title):
    global messages

    text = title + "\n\n"

    for message in messages:
        if message['role'] == 'user':
            content = message['content']
            if "### Instructions:" in content:
                instruction = content.split("### Instructions:", 1)[1].split("### Response:", 1)[0]
                cleaned = instruction.replace("\n", "").strip()
                text += f"{player}: {cleaned}\n"
        elif message['role'] == 'assistant':
            text += f"{character}: {message['content']}\n"

    print("text: " + text + "\n\n")

    # Write the dialogue to a file
    with open("dialogue_logs.txt", "a") as file:
        file.write(text + "\n")  # Append the dialogue

    return text

async def rating(player, character, text):

    prompt = f"""
{text}

This is a rating scale for interactions.
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
### Instructions:
Rate the interaction with {player} from {character}'s perspective from 0 to 10 using this scale
Only respond with the number rating, no explanation
### Response: """
    print(prompt)
    
    messages.append({"role": "user", "content": prompt})

    response_content = ""

    async for part in await AsyncClient().chat(
        model=llm_model, messages=messages, stream=True
    ):
        chunk = part['message']['content']
        response_content += chunk

    messages.append({"role": "assistant", "content": response_content})

    print(response_content)

    return response_content