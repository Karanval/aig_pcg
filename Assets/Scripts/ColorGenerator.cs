using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator
{
    ColorSettings settings;
    Texture2D texture;
    Texture2D terrainTexture;
    const int textureResolution = 50;

    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        if (texture == null)
        {
            texture = new Texture2D(textureResolution * 2, 1, TextureFormat.RGBA32, false);
            terrainTexture = new Texture2D(textureResolution * 2, 1, TextureFormat.RGBA32, false);
        }
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    /**
     * Terarin can be updated externaly, so save the terrain texture
     */
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
        terrainTexture.SetPixels(colors);
        terrainTexture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }

    public void UpdateColors(Color[] colors)
    {
        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }

    public void SetTexture(Texture2D texture)
    {
        this.texture = texture;
        this.texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }

    public Texture2D GetTerrainTexture()
    {
        return terrainTexture;
    }
}
