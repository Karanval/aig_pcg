using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseAnimator : MonoBehaviour
{
    public enum NoiseAnimatorType {All, Ocean, Terrain};
    [HideInInspector]
    public bool settingsFoldout;
    public NoiseSettings noiseSettings;
    public NoiseAnimatorSettings settings;

    Planet planet;
    Noise perlin;
    Noise oldPerlin;

    INoiseFilter noiseFilter;
    readonly int textureResolution = 50;
    bool used = false;
    private float passedTime = 0;
    private ITextureModifier textureModifier;

    public void OnSettingsUpdated()
    {
        textureModifier = TextureModifierFactory.CreateTextureModifier(settings.noiseAnimatorType);
        GenerateTexture();
        if (!settings.enabled)
        {
            planet.colorGenerator.SetTexture(planet.colorGenerator.GetTerrainTexture());
        }
    }

    public virtual void GenerateTexture()
    {
        if(!settings.enabled)
        {
            used = false;
            return;
        }
        Texture2D planetTexture = planet.colorGenerator.GetTerrainTexture();
        if (planetTexture != null)
        {
            print(textureModifier == null?"NUUULS ASF":"not null");

            Texture2D texture = textureModifier.GenerateTexture(planetTexture, settings.color, textureResolution, settings.weight);
            planet.colorGenerator.SetTexture(texture);
            used = true;
        }
    }

    public float Evaluate(Vector3 point)
    {
        float to = perlin.Evaluate(point);
        float from = oldPerlin.Evaluate(point);

        float evaluation = Mathf.Lerp(from, to, passedTime / settings.duration);
        evaluation = noiseFilter.Evaluate(evaluation);

        return evaluation;
    }


    private void Start()
    {
        planet = this.GetComponentInParent<Planet>();
        noiseFilter = NoiseFilterFactory.CreateNoisefilter(noiseSettings);
        GenerateTexture();
        perlin = new Noise(UnityEngine.Random.Range(0, int.MaxValue));
        oldPerlin = new Noise(UnityEngine.Random.Range(0, int.MaxValue));
        textureModifier = TextureModifierFactory.CreateTextureModifier(settings.noiseAnimatorType);
    }

    private void Update()
    {
        if (used && settings.enabled)
        {
            passedTime += Time.deltaTime;
            if (passedTime > settings.duration)
            {
                oldPerlin = perlin;
                perlin = new Noise(UnityEngine.Random.Range(0, int.MaxValue));
                passedTime = 0;
            }
            if (settings.myPcIsSlow)
            {
                if (Time.frameCount % 5 == 0)
                {
                    planet.UpdateUVsByExternal(this);
                }
            }  
            else
            {
                planet.UpdateUVsByExternal(this);
            }
        }
    }
}
