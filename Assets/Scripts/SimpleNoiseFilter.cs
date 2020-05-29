using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    NoiseSettings.SimpleNoiseSettings settings;
    INoise noise;
    int seed = 0;

    public SimpleNoiseFilter (NoiseSettings.SimpleNoiseSettings settings, int seed, NoiseType type)
    {
        this.settings = settings;
        noise = NoiseFactory.CreateNoise(type, seed);

    }

    public void CreateAndReplaceNoise(int seed, NoiseType type)
    {
        INoise newNoise = NoiseFactory.CreateNoise(type, seed);
        this.seed = seed;
        this.noise = newNoise;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;//(noise.Evaluate(point * settings.roughtness + settings.center) + 1) * 0.5f;
        float frequency = settings.baseRoughtness;
        float amplitude = 1;

        for(int i = 0; i < settings.octaves; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= settings.roughtness;
            amplitude *= settings.persistence;
        }

        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strenght;
    }

    public float Evaluate(float point)
    {
        float noiseValue = 0;//(noise.Evaluate(point * settings.roughtness + settings.center) + 1) * 0.5f;
        float frequency = settings.baseRoughtness;
        float amplitude = 1;

        for (int i = 0; i < settings.octaves; i++)
        {
            noiseValue += (point + 1) * 0.5f * amplitude;
            frequency *= settings.roughtness;
            amplitude *= settings.persistence;
        }

        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strenght;
    }

    public INoise GetNoise()
    {
        return noise;
    }

    public int GetSeed()
    {
        return seed;
    }

    public void SetNoise(INoise noise)
    {
        this.noise = noise;
    }
}
