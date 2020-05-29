using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Create much rich terrain features
 * sin wave-abs with sharp peaks by substractin -1 and then raise it to a power 
 */
public class RigidNoiseFilter : INoiseFilter
{
    NoiseSettings.RigidNoiseSettings settings;
    INoise noise;
    int seed = 0 ;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings, int seed, NoiseType type)
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
        float weight = 1;

        for (int i = 0; i < settings.octaves; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.center));
            v *= v;
            // reagions that start low down undetail, and higher up have more details
            v *= Mathf.Clamp(weight * settings.weightMultiplier, 0, 1);
            weight = v;

            noiseValue += v * amplitude;
            frequency *= settings.roughtness;
            amplitude *= settings.persistence;
        }

        // no longer clamp so we can get ocean depth //Mathf.Max(0, 
        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strenght;
    }

    /*
    * Used when evaluation of noise is made in a diffeernt place.
    */
    public float Evaluate(float point)
    {
        float noiseValue = 0;//(noise.Evaluate(point * settings.roughtness + settings.center) + 1) * 0.5f;
        float frequency = settings.baseRoughtness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.octaves; i++)
        {
            point *= point;
            // reagions that start low down undetail, and higher up have more details
            point *= Mathf.Clamp(weight * settings.weightMultiplier, 0, 1);
            weight = point;

            noiseValue += point * amplitude;
            frequency *= settings.roughtness;
            amplitude *= settings.persistence;
        }

        // no longer clamp so we can get ocean depth //Mathf.Max(0, 
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
