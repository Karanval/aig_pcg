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
    Noise noise = new Noise(0);

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this.settings = settings;
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

    public Noise GetNoise()
    {
        return noise;
    }

    public void SetNoise(Noise noise)
    {
        this.noise = noise;
    }
}
