using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSettings settings;
    Texture2D texture;
    const int textureResolution = 50;

    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        if (texture == null)
        {
            texture = new Texture2D(textureResolution * 2, 1, TextureFormat.RGBA32, false);
        }
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[texture.width * texture.height];
        for (int i = 0; i < texture.width; i++)
        {
            Color gradientColor;
            if (i < textureResolution)
            {
                gradientColor = settings.oceanColor.Evaluate(i / (textureResolution - 1f));
            } else
            {
                gradientColor = settings.terrainColor.Evaluate((i - textureResolution) / (textureResolution - 1f));
            }
            colors[i] = gradientColor;
        }
        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
