import asyncio
from ollama import AsyncClient
from retriever import *

__all__ = ['start', 'create_new_chat', 'chat']

rapport_level_progress = 0
rapport_level = "Friend"
retrieved_context = "Karen is being rude at a store. She takes it out on the store worker."
messages = []

# prompt = f"You are Karen, an NPC in a game with three relationship paths: Friend, Lover, Rival. Your bond level with the player is {str_bond_level} out of 5. Your previous interactions: {retrieved_context} The player chooses '{player_choice}'. I want you to generate one line of Karen’s response to this situation in a natural, engaging way."

# def create_prompt():
#     global str_bond_level
#     global relationship
#     global retrieved_context

#     current_story = ""

#     str_bond_level = input("Enter bond level (1-5): ")
#     if str_bond_level.isdigit() and int(str_bond_level) >= 1 and int(str_bond_level) <= 5:
#         str_bond_level = str(bond_level)
#     else:
#         print("Invalid choice. Bond level set to 3.")
#         str_bond_level = "3"
    
#     relationship = input("Enter player choice (Friend, Lover, Rival): ")
#     if relationship != "Friend" and relationship != "Lover" and relationship != "Rival":
#         print("Invalid choice. Player choice set to 'Friend'.")
#         relationship = "Friend"
    
#     current_story = input("Enter retrieved context (eg. Karen is being rude at a store. She takes it out on the store worker.): ")
#     if current_story == "":
#         print("Invalid choice. Retrieved context set to 'Karen is being rude at a store. She takes it out on the store worker.'")
#         current_story = "Karen is being rude at a store. She takes it out on the store worker."

#     retrieved_context = retriever(current_story)
#     prompt = f"You are Karen, an NPC in a game with three relationship paths: Friend, Lover, Rival. Your bond level with the player is {str_bond_level} out of 5. Your previous interactions: {retrieved_context} The player chooses '{relationship}'. If they choose lover with level 3 or higher, act like a tsundere. I want you to generate one line of Karen’s response to this situation in a natural, engaging way."

#     print("prompt: " + prompt + "\n\n")

#     return prompt

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
        # insertPosts(messages)
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

#  print("prompt: " + prompt + "\n\n")
async def main():
    prompt = create_prompt()
    print("generating prompt...")
    await chat(prompt=prompt)
    while True:
        user_input = input("\n>>> ")
        if user_input == "/exit":
            break
        elif user_input:
            await chat(user_input)

if __name__ == "__main__":
    asyncio.run(main())