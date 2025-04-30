import chromadb
import uuid
from sentence_transformers import SentenceTransformer

__all__ = ['retriever', 'insert_post']

chroma_client = chromadb.PersistentClient(path="./chroma_db")  

karen_collection = chroma_client.get_or_create_collection("karen_history")
jade_collection = chroma_client.get_or_create_collection("jade_history")
kaden_collection = chroma_client.get_or_create_collection("kaden_history")
aster_collection = chroma_client.get_or_create_collection("aster_history")

embedding_model = SentenceTransformer("all-MiniLM-L6-v2")  # Lightweight model

def insert_post(character_name, title, text):
    collection = choose_collection(character_name)

    id = uuid.uuid4()

    embedding = embedding_model.encode(title).tolist()

    collection.add(
        ids=[str(id)],
        embeddings=[embedding],
        metadatas=[{"Title": title, "Text": text}]
    )
    print(f"Post with ID {id} added to ChromaDB.")

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