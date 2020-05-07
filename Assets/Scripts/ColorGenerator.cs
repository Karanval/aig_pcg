using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSettings settings;
    Texture2D texture;
    const int textureResolution = 50;
    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        if(texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
        {
            // first half for the ocean, other half for terrain
            texture = new Texture2D(textureResolution * 2, settings.biomeColorSettings.biomes.Length,
                TextureFormat.RGBA32, false);
        }
        biomeNoiseFilter = NoiseFilterFactory.CreateNoisefilter(settings.biomeColorSettings.noise);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    // Should return 0 if first biome, 1 for last, and a value in
    // between for any other values
    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        //0 south pole 1 north pole
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColorSettings.noiseOffset) * settings.biomeColorSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColorSettings.biomes.Length;
        float blendRange = settings.biomeColorSettings.blendAmount / 2f + .001f;// in case amount is 0

        for (int i = 0; i < numBiomes; i++)
        {
            float dst = heightPercent - settings.biomeColorSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
            biomeIndex *= (1 - weight);// reset the index first, or dont grow too much
            biomeIndex += i * weight;
        }

        //biomes index in range 0 - 1, or 1 if 0
        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height];
        int colourIndex = 0;
        foreach (var biome in settings.biomeColorSettings.biomes)
        {
            for (int i = 0; i < textureResolution; i++)
            {
                Color gradientColor;
                if (i < textureResolution)
                {
                    gradientColor = settings.oceanColor.Evaluate(i / (textureResolution - 1f));
                } else
                {
                    gradientColor = biome.gradient.Evaluate(i / (textureResolution - 1f));
                }

                Color tintCol = biome.tint;
                colors[colourIndex] = gradientColor * (1 - biome.tintPercent) + tintCol * biome.tintPercent;
                colourIndex++;
            }
        }
        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
