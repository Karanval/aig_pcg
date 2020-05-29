using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class NoiseFactory
{

    public static INoise CreateNoise(NoiseType type, int seed)
    {
        switch (type)
        {
            case NoiseType.SimplexPerlin:
                return new Noise(seed);
            case NoiseType.MyPerlin:
                return new MyPerlin(seed);
        }
        return null;
    }
}
