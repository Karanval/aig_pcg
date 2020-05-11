using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseAnimatorSettings : ScriptableObject
{
    public NoiseAnimator.NoiseAnimatorType noiseAnimatorType;
    public bool enabled;
    public bool myPcIsSlow;
    [Range(0,1)]
    public float weight;
    public float duration = 10f;

    public Gradient color;
}
