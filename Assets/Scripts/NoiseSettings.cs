using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { Simple, Rigid };
    public FilterType filterType;

    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RigidNoiseSettings rigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strenght = 1;
        [Range(1,8)]
        public int octaves = 1;
        public float baseRoughtness = 1;
        public float roughtness = 2;
        public float persistence = 0.5f;
        public Vector3 center;
        public float minValue;

    }

    [System.Serializable]
    public class RigidNoiseSettings: SimpleNoiseSettings
    {
       public float weightMultiplier = 0.8f;
    }
}
