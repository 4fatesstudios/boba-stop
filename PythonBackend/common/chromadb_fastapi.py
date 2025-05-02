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
    n: int

class SummaryData(BaseModel):
    n: int

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
    level_context: str
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

@app.post("/summarize_chat/character/{character}")
async def summarize(character: str, data: SummaryData):
    result = await summarize_chat(character)
    # text = create_text(character, result)
    # insert_post(character, result, text, data.n)
    return {"response": result}

@app.post("/insert_post/character/{character}")
async def insert_conversation(character: str, data: Post):
    text = create_text(character, data.title)
    insert_post(character, data.title, text, data.n)
    return {"message": "Posts inserted successfully."}

if __name__ == "__main__":
    uvicorn.run("chromadb_fastapi:app", host="127.0.0.1", port=8000)