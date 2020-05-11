using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureModifier : ITextureModifier
{
    public Texture2D GenerateTexture(Texture2D original, Gradient gradient, int resolution, float weight)
    {
        Texture2D texture = new Texture2D(original.width, resolution, TextureFormat.RGBA32, false);
        Color[] colors = new Color[original.width * resolution];
        int index = 0;
        for (int j = 0; j < texture.height; j++)
        {
            for (int i = 0; i < original.width; i++)
            {
                Color planetPixel = original.GetPixel(i, 0);
                if (i > original.width / 2)
                {
                    colors[index] = gradient.Evaluate((float)j / texture.height);

                    colors[index] = Color.Lerp(planetPixel, colors[index], weight);
                }
                else
                {
                    colors[index] = original.GetPixel(i, 0);
                }
                index++;
            }
        }
        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }
}
