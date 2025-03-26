using AllMiniLmL6V2Sharp;
using Microsoft.ML.OnnxRuntime;
using System.Numerics.Tensors;
using System.Linq;

public class EmbeddingGenerator
{
    public static float[] GetEmbedding(string text)
    {
        string modelPath = "./model/model.onnx";
        using var session = new InferenceSession(modelPath);
        using var embedder = new AllMiniLmL6V2Embedder(modelPath: modelPath);
        return embedder.GenerateEmbedding(text).ToArray();
    }
}