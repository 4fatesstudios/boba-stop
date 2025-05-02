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

# Load the collection
karen_collection = chroma_client.get_or_create_collection("karen_history")
jade_collection = chroma_client.get_or_create_collection("jade_history")
kaden_collection = chroma_client.get_or_create_collection("kaden_history")
aster_collection = chroma_client.get_or_create_collection("aster_history")
common_collection = chroma_client.get_or_create_collection("common_knowledge")

embedding_model = SentenceTransformer("all-MiniLM-L6-v2")  # Lightweight model

def insert_post(character_name, title, text):
    collection = choose_collection(character_name)

    # Check if the title already exists using metadata filter
    results = collection.get(where={"Title": title})

    if results["ids"]:
        existing_id = results["ids"][0]
        existing_metadata = results["metadatas"][0]
        existing_text = existing_metadata.get("Text", "")

        # Avoid adding duplicate text
        if text.strip() in existing_text:
            print(f"Duplicate text already exists for title '{title}', skipping insert.")
            return

        new_text = existing_text + "\n" + text.strip()

        # Update existing document
        collection.update(
            ids=[existing_id],
            embeddings=[embedding_model.encode(title).tolist()],
            metadatas=[{"Title": title, "Text": new_text}]
        )
        print(f"Post with Title '{title}' updated in ChromaDB.")
    else:
        # Insert new document
        id = str(uuid.uuid4())
        embedding = embedding_model.encode(title).tolist()

        collection.add(
            ids=[id],
            embeddings=[embedding],
            metadatas=[{"Title": title, "Text": text.strip()}]
        )
        print(f"Post with ID {id} and Title '{title}' added to ChromaDB.")



def retriever(character_name, query_text="Karen is being rude at a store", n_results=1):
    char_collection = choose_collection(character_name)
    query_embedding = embedding_model.encode(query_text).tolist()

    # Query character-specific collection
    char_results = char_collection.query(
        query_embeddings=[query_embedding],
        n_results=n_results
    )

    # Query common knowledge collection
    common_results = common_collection.query(
        query_embeddings=[query_embedding],
        n_results=n_results
    )

    combined_texts = []

    print(f"\n--- Character: {character_name} Results ---")
    for doc, metadata, dist in zip(char_results["documents"][0], char_results["metadatas"][0], char_results["distances"][0]):
        print(f"Title: {metadata['Title']}")
        print(f"Distance: {dist}")
        print(f"Text: {metadata.get('Text', '')}\n")
        combined_texts.append(metadata.get("Text", ""))

    print(f"\n--- Common Knowledge Results ---")
    for doc, metadata, dist in zip(common_results["documents"][0], common_results["metadatas"][0], common_results["distances"][0]):
        print(f"Title: {metadata['Title']}")
        print(f"Distance: {dist}")
        print(f"Text: {metadata.get('Text', '')}\n")
        combined_texts.append(metadata.get("Text", ""))

    return " ".join(combined_texts)


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
        case "common":
            return common_collection
        case _:
            raise ValueError("Unknown character name.")


def clear_all_collections():
    for collection in [karen_collection, jade_collection, kaden_collection, aster_collection, common_collection]:
        # Retrieve all IDs in the collection
        results = collection.get()
        ids = results.get("ids", [])
        if ids:
            collection.delete(ids=ids)
            print(f"Cleared {len(ids)} items from collection '{collection.name}'")
        else:
            print(f"No items to clear in collection '{collection.name}'")
