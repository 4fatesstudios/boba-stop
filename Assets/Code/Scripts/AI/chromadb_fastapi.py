from fastapi import FastAPI
from pydantic import BaseModel
import uvicorn
from generator import *
from retriever import *

app = FastAPI()

class Prompt(BaseModel):
    text: str

# Request body model for POST /new_conversation
class ConversationData(BaseModel):
    rapport_level: int
    rapport_level_progress: int
    current_story: str


@app.get("/")
async def hello_world():
    return {"message": "Hello, World!"}


@app.get("/first_meeting/character/{character_name}")
async def first_meeting(character_name: str):
    context = retriever(f'personaity of {character_name}')
    prompt = start(character_name, context)
    result = await chat(character_name, prompt)
    return {"response": result}


@app.post("/new_conversation/character/{character_name}")
async def new_conversation(character_name: str, data: ConversationData):
    context = retriever(data.current_story)
    prompt = create_new_chat(
        character_name,
        context,
        data.current_story,
        str(data.rapport_level_progress),
        str(data.rapport_level)
    )
    result = await chat(character_name, prompt)
    return {"response": result}


@app.post("/chat/character/{character_name}")
async def chat_route(character_name: str, data: Prompt):
    result = await chat(character_name, data.text)
    return {"response": result}

if __name__ == "__main__":
    uvicorn.run("chromadb_fastapi:app", host="127.0.0.1", port=8000, reload=True)