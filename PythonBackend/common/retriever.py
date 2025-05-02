import os
import chromadb
import uuid
from sentence_transformers import SentenceTransformer

__all__ = ['retriever', 'insert_post']

def get_common_path():
    if getattr(sys, 'frozen', False):
        # Running from PyInstaller EXE: go up from dist/chromadb_fastapi/ to common/
        base_dir = os.path.abspath(os.path.join(os.path.dirname(sys.executable), "..", "..", "..", "common"))
    else:
        # Running from source: assume script is in common/
        base_dir = os.path.dirname(os.path.abspath(__file__))
    return base_dir

import sys  # must be imported before get_common_path()

COMMON_DIR = get_common_path()
CHROMA_PATH = os.path.join(COMMON_DIR, "chroma_db")

chroma_client = chromadb.PersistentClient(path=CHROMA_PATH)

karen_collection = chroma_client.get_or_create_collection("karen_history")
jade_collection = chroma_client.get_or_create_collection("jade_history")
kaden_collection = chroma_client.get_or_create_collection("kaden_history")
aster_collection = chroma_client.get_or_create_collection("aster_history")

embedding_model = SentenceTransformer("all-MiniLM-L6-v2")  # Lightweight model

def insert_post(character_name, title, text, id=0):
    collection = choose_collection(character_name)

    embedding = embedding_model.encode(title).tolist()

    collection.add(
        ids=[str(id)],
        embeddings=[embedding],
        metadatas=[{"Title": title, "Text": text}]
    )
    print(f"Post with ID {id} added to {character_name}'s collection.")

def retriever(character_name, query_text="Karen is being rude at a store"):
    collection = choose_collection(character_name)
    query_embedding = embedding_model.encode(query_text).tolist()

    results = collection.query(
        query_embeddings=[query_embedding],
        n_results=2
    )

    # Display results
    for doc, metadata in zip(results["documents"][0], results["metadatas"][0]):
        print(f'Title: {metadata["Title"]}')
        if "Text" in metadata:
            print(f'Response: {metadata["Text"]}\n')

    retrieved_texts = [metadata["Text"] for metadata in results["metadatas"][0] if "Text" in metadata]
    return " ".join(retrieved_texts)

def choose_collection(character_name):
    match character_name:
        case "Karen":
            return karen_collection
        case "Jade":
            return jade_collection
        case "Kaden":
            return kaden_collection
        case "Aster":
            return aster_collection
        case _:
            raise ValueError("Unknown character name.")
        
if __name__ == "__main__":
    chroma_client.delete_collection("karen_history")
    karen_collection = chroma_client.get_or_create_collection("karen_history")
    chroma_client.delete_collection("jade_history")
    jade_collection = chroma_client.get_or_create_collection("jade_history")
    chroma_client.delete_collection("kaden_history")
    kaden_collection = chroma_client.get_or_create_collection("kaden_history")
    chroma_client.delete_collection("aster_history")
    aster_collection = chroma_client.get_or_create_collection("aster_history")
    insert_post("Karen", "first meeting with Karen", "Karen just got her drink at the player’s boba shop. Karen think the player got her order wrong by adding boba")
    insert_post("Jade", "first meeting with Jade", "Jade is meeting the player for the first time at the player’s boba shop. Jade is ordering a boba drink before she heads back to work.")
    insert_post("Kaden", "first meeting with Kaden", "Kaden and his friends are playing a game of frisbee in the town square. He’ll see the player and wants to invite them to join in.")
    insert_post("Aster", "first meeting with Aster", "The Player is rushing home and bumps into Aster in the town square. Aster is annoyed, but amused by the player acting flustered.")