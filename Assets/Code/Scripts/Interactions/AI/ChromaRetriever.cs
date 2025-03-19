using System;
using System.Collections.Generic;
using ChromaDB.Client;
using System.Threading.Tasks;
using UnityEngine;


public class ChromaDBRetriever
{
    public async Task<List<List<ChromaDB.Client.Models.ChromaCollectionQueryEntry>>> RetrieveData(ChromaCollectionClient chromaClient, string input)
    {
        float[] embedding = EmbeddingGenerator.GetEmbedding(input);
        List<ReadOnlyMemory<float>> embeddingsList = new List<ReadOnlyMemory<float>>
        {
            new ReadOnlyMemory<float>(embedding)
        };

        var queryData = await chromaClient.Query(
            embeddingsList, 
            include: ChromaQueryInclude.Metadatas | ChromaQueryInclude.Distances,
            nResults: 2
        );

        foreach (var item in queryData)
        {
            foreach (var entry in item)
            {
                Debug.Log($"ID: {entry.Id} | Distance: {entry.Distance}");
                
                // Extract and print metadata
                if (entry.Metadata != null)
                {
                    foreach (var meta in entry.Metadata)
                    {
                        Debug.Log($"{meta.Key}: {meta.Value}");
                    }
                }
            }
        }

        return queryData;
    }
}