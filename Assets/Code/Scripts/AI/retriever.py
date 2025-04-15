import pandas as pd
import chromadb
from sentence_transformers import SentenceTransformer

__all__ = ['retriever']

# df = pd.read_csv("./karen_dataset.csv")

chroma_client = chromadb.PersistentClient(path="./chroma_db")  
collection = chroma_client.get_or_create_collection("karen_personality")

embedding_model = SentenceTransformer("all-MiniLM-L6-v2")  # Lightweight model

# def insertPosts():
#     # Iterate through CSV and add to ChromaDB
#     for index, row in df.iterrows():
#         title = row["Title"]  
#         text = row["Text"] 
#         combined_text = f"{title}. {text}" 

#         embedding = embedding_model.encode(combined_text).tolist()
        
#         collection.add(
#             ids=[str(index)],
#             embeddings=[embedding],
#             metadatas=[{"Title": title, "Text": text}])
#     print("karen posts added")

def retriever(query_text="Karen is being rude at a store"):
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


if __name__ == "__main__":
    retriever()

