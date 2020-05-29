using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoiseFilter 
{
    float Evaluate(Vector3 point);
    float Evaluate(float point);
    INoise GetNoise();
    void SetNoise(INoise noise);
    void CreateAndReplaceNoise(int seed, NoiseType type);
    int GetSeed();
}
