using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITextureModifier
{
    Texture2D GenerateTexture(Texture2D original, Gradient gradient, int resolution, float weight);
}
