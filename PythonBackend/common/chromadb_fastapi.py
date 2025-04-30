from fastapi import FastAPI
from pydantic import BaseModel
from typing import Dict
import uvicorn
from generator import *
from retriever import *

app = FastAPI()

class Prompt(BaseModel):
    text: str

class Post(BaseModel):
    title: str
    text: str

# Request body model for POST /new_conversation
class ConversationData(BaseModel):
    rapport_level: int
    rapport_level_progress: int
    current_story: str


@app.get("/")
async def hello_world():
    return {"message": "Hello, World!"}


@app.get("/first_meeting/character/{character}")
async def first_meeting(character: str):
    context = retriever(character, f'personaity of {character}')
    prompt = start(character, context)
    result = await chat(character, prompt)
    return {"response": result}


@app.post("/new_conversation/character/{character}")
async def new_conversation(character: str, data: ConversationData):
    context = retriever(data.current_story)
    prompt = create_new_chat(
        character,
        context,
        data.current_story,
        str(data.rapport_level_progress),
        str(data.rapport_level)
    )
    result = await chat(character, prompt)
    return {"response": result}


@app.post("/chat/character/{character}")
async def chat_route(character: str, data: Prompt):
    result = await chat(character, data.text)
    return {"response": result}

@app.post("/summarize_chat/character/{character}")
async def summarize(character: str, data: Prompt):
    result = await summarize_chat(character, data.text)
    return {"response": result}

@app.post("/insert_post/character/{character}")
async def insert_conversation(character: str, data: Post):
    result = insert_post(character, data.title, data.text)
    return {"message": "Posts inserted successfully."}

if __name__ == "__main__":
    uvicorn.run("chromadb_fastapi:app", host="127.0.0.1", port=8000)