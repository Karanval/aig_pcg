using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum NoiseType { SimplexPerlin, MyPerlin};
public interface INoise
{
    float Evaluate(Vector3 point);
}
