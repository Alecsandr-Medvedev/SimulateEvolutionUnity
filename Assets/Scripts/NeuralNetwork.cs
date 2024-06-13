using System;
using System.Linq;

class NeuralNetwork
{
    private int[] layers;
    private float[][] neurons;
    private float[][][] weights;
    private Func<float, float>[] activationFunctions;
    private Random random;

    public NeuralNetwork(int[] layers, Func<float, float> activationFunction, float[][][] weights = null)
    {
        this.layers = layers;
        this.activationFunctions = Enumerable.Repeat(activationFunction ?? new Func<float, float>(x => Sigmoid(x)), layers.Length - 1).ToArray();
        this.random = new Random();

        InitializeNeurons();
        InitializeWeights(weights);
    }

    private void InitializeNeurons()
    {
        neurons = new float[layers.Length][];
        for (int i = 0; i < layers.Length; i++)
        {
            neurons[i] = new float[layers[i]];
        }
    }

    private void InitializeWeights(float[][][] weights)
    {
        this.weights = new float[layers.Length - 1][][];
        for (int i = 0; i < layers.Length - 1; i++)
        {
            this.weights[i] = new float[layers[i + 1]][];
            for (int j = 0; j < layers[i + 1]; j++)
            {
                if (weights != null && weights.Length > i && weights[i].Length > j && weights[i][j] != null)
                {
                    this.weights[i][j] = weights[i][j];
                }
                else
                {
                    this.weights[i][j] = Enumerable.Range(0, layers[i]).Select(_ => (float)(random.NextDouble() * 2 - 1)).ToArray();
                }
            }
        }
    }

    public float[] FeedForward(float[] input)
    {
        if (input.Length != layers[0])
        {
            throw new ArgumentException("Input size does not match the input layer size");
        }

        // Set input layer
        for (int i = 0; i < input.Length; i++)
        {
            neurons[0][i] = input[i];
        }

        // Feedforward
        for (int i = 0; i < layers.Length - 1; i++)
        {
            for (int j = 0; j < layers[i + 1]; j++)
            {
                float sum = 0;
                for (int k = 0; k < layers[i]; k++)
                {
                    sum += neurons[i][k] * weights[i][j][k];
                }
                neurons[i + 1][j] = activationFunctions[i](sum);
            }
        }

        return neurons.Last().ToArray();
    }

    private static float Sigmoid(float x)
    {
        return (float) (1 / (1 + Math.Exp(-x)));
    }

    private static float Tanh(float x)
    {
        return (float) Math.Tanh(x);
    }

    public float[][][] GetMutateWeights(float mutationRate = 0.4f)
    {
        Random random = new Random((int)(UnityEngine.Time.time * 100));
        float[][][] mutatedWeights = new float[weights.Length][][];

        for (int i = 0; i < weights.Length; i++)
        {
            mutatedWeights[i] = new float[weights[i].Length][];

            for (int j = 0; j < weights[i].Length; j++)
            {
                mutatedWeights[i][j] = new float[weights[i][j].Length];

                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    if (random.NextDouble() < mutationRate)
                    {
                        mutatedWeights[i][j][k] = (float)(random.NextDouble() * 2 - 1);
                    }
                    else
                    {
                        mutatedWeights[i][j][k] = (float)weights[i][j][k];
                    }
                }
            }
        }

        return mutatedWeights;
    }


}
public static class Activations
{
    public static float Sigmoid(float x)
    {
        return 1.0f / (1.0f + (float)Math.Exp(-x));
    }

    public static float Tanh(float x)
    {
        return (float)Math.Tanh(x);
    }

    public static float ReLU(float x)
    {
        return Math.Max(0, x);
    }
}