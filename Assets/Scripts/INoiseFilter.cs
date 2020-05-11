using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoiseFilter 
{
    float Evaluate(Vector3 point);
    float Evaluate(float point);
    Noise GetNoise();
    void SetNoise(Noise noise);
    void CreateAndReplaceNoise(int seed);
    int GetSeed();
}
