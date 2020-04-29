using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public float strenght = 1;
    [Range(1,8)]
    public int numberOfLayers = 1;
    public float baseRoughtness = 1;
    public float roughtness = 2;
    public float persistence = 0.5f;
    public Vector3 center;
    public float minValue;
}
