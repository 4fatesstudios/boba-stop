from fastapi import FastAPI
from pydantic import BaseModel
from typing import Dict
import uvicorn
from generator import *
from retriever import *

app = FastAPI()

# Request body model for POST /chat
class Prompt(BaseModel):
    text: str

# Request body model for POST /insert_post
class Post(BaseModel):
    title: str
    character_name: str

# Request body model for POST /rate_conversation
class RateConversation(BaseModel):
    character_name: str

# Request body model for POST /start
class CombinedData(BaseModel):
    # character data
    name: str
    age: str
    role: str
    living_condition: str
    personality: str
    beliefs: str
    speaking_style: str
    knowledge_scope: str
    backstory: str
    memory: str
    location_knowledge: str
    rapport_level: str

    # world context
    world_location: str
    world_time: str
    world_weather: str
    level_context: str[2]
    current_context: str

@app.get("/")
async def hello_world():
    return {"message": "Hello, World!"}


@app.post("/first_meeting/character/{character}")
async def first_meeting(character: str, data: CombinedData):
    context = retriever(data.name, f'first meeting with {data.name}')
    intro = start(character, data, context)
    result = await chat(character, intro, "")
    return result


@app.post("/new_conversation/character/{character}")
async def new_conversation(character: str, data: CombinedData):
    context = retriever(data.name, data.memory)
    intro = create_new_chat(
        character,
        data,
        context
    )
    result = await chat(character, intro, "")
    return result


@app.post("/chat/character/{character}")
async def chat_route(character: str, data: Prompt):
    result = await chat(character, '', data.text)
    return result

# change to make post and add character name in create_text
@app.get("/summarize_chat/character/{character}")
async def summarize(character: str):
    result = await summarize_chat(character)
    # text = create_text(character, result)
    # insert_post(character, result, text)
    return {"response": result}

@app.post("/insert_post/character/{character}")
async def insert_conversation(character: str, data: Post):
    text = create_text(character, data.title)
    insert_post(character, data.title, text)
    return {"message": "Posts inserted successfully."}

@app.post("/rate_conversation/player/{player}")
async def rate_conversation(player: str, data: RateConversation):
    text = create_text(player, data.character_name, f"conversation between {player} and {data.character_name}")
    result = await rating(player, data.character_name, text)
    return {"response": result}

if __name__ == "__main__":
    uvicorn.run("chromadb_fastapi:app", host="127.0.0.1", port=8000)