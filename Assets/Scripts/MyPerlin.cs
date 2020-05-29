using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPerlin : INoise
{
    // create random array 1024

    // Edges midpoints of cube
    // Gradients that the corners can take
    //readonly Vector3[] gradients = new []
    //{
    //    new Vector3 (1, 1, 0), new Vector3 (-1, 1, 0), new Vector3 (1, -1, 0),
    //    new Vector3 (-1, -1, 0), new Vector3 (1, 0, 1), new Vector3 (-1, 0, 1),
    //    new Vector3 (1, 0, -1), new Vector3 (-1, 0, -1), new Vector3 (0, 1, 1),
    //    new Vector3 (0, -1, 1), new Vector3 (0, 1, -1), new Vector3 (0, -1, -1)
    //};

    private readonly int[] preGeneratedNumbers;
    private readonly XorShiftRandom random;
    private readonly int step;
    int[] perm = new int [512];
    int[] permutation = { 151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249,
         14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180 };


    public MyPerlin(int seed = 0, int step = 1)
    {
        preGeneratedNumbers = new int[1024];
        random = new XorShiftRandom(seed);
        this.step = step;
        //for (int i = 0; i < 256; i++)
        //    perm[256 + i] = perm[i] = permutation[i];
        GenerateNumbers();
    }
    bool firstTiem = true;
    public float Evaluate(Vector3 point)
    {
        // find cube corners
        Vector3[] corners = new Vector3[8];
        int x1 = (int)(point.x - (point.x % step));
        int x2 = x1 + step;
        int y1 = (int)(point.x - (point.x % step));
        int y2 = x1 + step;
        int z1 = (int)(point.x - (point.x % step));
        int z2 = x1 + step;

        corners[0] = new Vector3(x1, y2, z2);
        corners[1] = new Vector3(x2, y2, z2);
        corners[2] = new Vector3(x2, y1, z2);
        corners[3] = new Vector3(x1, y1, z2);
        corners[4] = new Vector3(x1, y2, z1);
        corners[5] = new Vector3(x2, y2, z1);
        corners[6] = new Vector3(x2, y1, z1);
        corners[7] = new Vector3(x1, y1, z1);


        // map vector3 to position in random array -
        // map corners to number in pre generated numbers
        // hash the array values to the gradients

        int[] randomByPoint = { 
            preGeneratedNumbers[(int) ( (point.x * 1000) + (point.y * 1000) + (point.z * 1000) ) % 1024],
            preGeneratedNumbers[(int) ( (point.x * 1000) + (point.y * 1000) + (point.z * 1000) ) % 1024],
            preGeneratedNumbers[(int) ( (point.x * 1000) + (point.y * 1000) + (point.z * 1000) ) % 1024]
        };

        int[] indexes =
        {
            (int) ( (corners[0].x * 1000) *randomByPoint[0] + (corners[0].y * 1000) *randomByPoint[1] + (corners[0].z * 1000)*randomByPoint[2] ) % 1024,
            (int) ( (corners[1].x * 1000) *randomByPoint[0] + (corners[1].y * 1000) *randomByPoint[1] + (corners[1].z * 1000)*randomByPoint[2] ) % 1024 ,
            (int) ( (corners[2].x * 1000) *randomByPoint[0] + (corners[2].y * 1000) *randomByPoint[1] + (corners[2].z * 1000)*randomByPoint[2] ) % 1024 ,
            (int) ( (corners[3].x * 1000) *randomByPoint[0] + (corners[3].y * 1000) *randomByPoint[1] + (corners[3].z * 1000)*randomByPoint[2] ) % 1024 ,
            (int) ( (corners[4].x * 1000) *randomByPoint[0] + (corners[4].y * 1000) *randomByPoint[1] + (corners[4].z * 1000)*randomByPoint[2] ) % 1024 ,
            (int) ( (corners[5].x * 1000) *randomByPoint[0] + (corners[5].y * 1000) *randomByPoint[1] + (corners[5].z * 1000)*randomByPoint[2] ) % 1024 ,
            (int) ( (corners[6].x * 1000) *randomByPoint[0] + (corners[6].y * 1000) *randomByPoint[1] + (corners[6].z * 1000)*randomByPoint[2] ) % 1024 ,
            (int) ( (corners[7].x * 1000) *randomByPoint[0] + (corners[7].y * 1000) *randomByPoint[2] + (corners[7].z * 1000)*randomByPoint[2] ) % 1024
        };
        if (firstTiem)
        {
            Debug.Log("Indexes: " + indexes[0]);
            Debug.Log("Indexes: " + indexes[1]);
            Debug.Log("Indexes: " + indexes[2]);
            Debug.Log("Indexes: " + indexes[3]);
            Debug.Log("Indexes: " + indexes[4]);
            Debug.Log("Indexes: " + indexes[5]);
            Debug.Log("Indexes: " + indexes[6]);
            Debug.Log("Indexes: " + indexes[7]);
        }

        //Vector3[] cornerGradients = new []
        //{
        //    gradients[  preGeneratedNumbers[indexes[0]] % 12],
        //    gradients[  preGeneratedNumbers[indexes[1]] % 12],
        //    gradients[  preGeneratedNumbers[indexes[2]] % 12],
        //    gradients[  preGeneratedNumbers[indexes[3]] % 12],
        //    gradients[  preGeneratedNumbers[indexes[4]] % 12],
        //    gradients[  preGeneratedNumbers[indexes[5]] % 12],
        //    gradients[  preGeneratedNumbers[indexes[6]] % 12],
        //    gradients[  preGeneratedNumbers[indexes[7]] % 12]
        //};

        int ix, iy, iz, gx, gy, gz;
        //int a0, b0, aa, ab, ba, bb;

        //int aa0, ab0, ba0, bb0;
        //int aa1, ab1, ba1, bb1;
        float x = point.x, y = point.y, z = point.z;

        ix = (int)x; x -= ix;
        iy = (int)y; y -= iy;
        iz = (int)z; z -= iz;

        //gx = ix & 0xFF;
        //gy = iy & 0xFF;
        //gz = iz & 0xFF;

        //a0 = gy + perm[gx];
        //b0 = gy + perm[gx + 1];
        //aa = gz + perm[a0];
        //ab = gz + perm[a0 + 1];
        //ba = gz + perm[b0];
        //bb = gz + perm[b0 + 1];

        //aa0 = perm[aa]; aa1 = perm[aa + 1];
        //ab0 = perm[ab]; ab1 = perm[ab + 1];
        //ba0 = perm[ba]; ba1 = perm[ba + 1];
        //bb0 = perm[bb]; bb1 = perm[bb + 1];

        //float[] grads =
        //{
        //    Grad(bb1, point.x-1, point.y-1, point.z-1),
        //    Grad(ab1, point.x  , point.y-1, point.z-1),
        //    Grad(ba1, point.x-1, point.y  , point.z-1),
        //    Grad(aa1, point.x  , point.y  , point.z-1),
        //    Grad(bb0, point.x-1, point.y-1, point.z),
        //    Grad(ab0, point.x  , point.y-1, point.z),
        //    Grad(ba0, point.x-1, point.y  , point.z),
        //    Grad(aa0, point.x  , point.y  , point.z),
        //};
        float[] grads =
        {
            Grad(preGeneratedNumbers[indexes[0]], point.x-1, point.y-1, point.z-1),
            Grad(preGeneratedNumbers[indexes[1]], point.x  , point.y-1, point.z-1),
            Grad(preGeneratedNumbers[indexes[2]], point.x-1, point.y  , point.z-1),
            Grad(preGeneratedNumbers[indexes[3]], point.x  , point.y  , point.z-1),
            Grad(preGeneratedNumbers[indexes[4]], point.x-1, point.y-1, point.z),
            Grad(preGeneratedNumbers[indexes[5]], point.x  , point.y-1, point.z),
            Grad(preGeneratedNumbers[indexes[6]], point.x-1, point.y  , point.z),
            Grad(preGeneratedNumbers[indexes[7]], point.x  , point.y  , point.z),
        };
        //// find distance from point to corners
        //float[] distances =
        //{
        //    Vector3.Distance(point, corners[0]),
        //    Vector3.Distance(point, corners[1]),
        //    Vector3.Distance(point, corners[2]),
        //    Vector3.Distance(point, corners[3]),
        //    Vector3.Distance(point, corners[4]),
        //    Vector3.Distance(point, corners[5]),
        //    Vector3.Distance(point, corners[6]),
        //    Vector3.Distance(point, corners[7])
        //};
        // interpolate the gradients to the point

        //float interpolation = Mathf.Lerp(
        //    preGeneratedNumbers[(int)((point.x * 1000) + (point.y * 1000) + (point.z * 1000)) % 1024], 
        //    preGeneratedNumbers[indexes[0]], distances[0]);

        float u = Fade(x);
        float v = Fade(y);
        float w = Fade(z);
        float inter1 = Mathf.Lerp(v, Mathf.Lerp(u, grads[7], grads[6]), Mathf.Lerp(u, grads[5], grads[4]));
        float inter2 = Mathf.Lerp(v, Mathf.Lerp(u, grads[3], grads[2]), Mathf.Lerp(u, grads[1], grads[0]));
        firstTiem = false;

        return Mathf.Lerp(w, inter1, inter2);
    }

    private float Fade(float val)
    {
        return val * val * val * (val * (val * 6.0f - 15.0f) + 10.0f);
    }

    private float Grad(int hash, float x, float y, float z)
    {
        //float u = (h < 8) ? x : y;
        //float v = (h < 4) ? y : ((h == 12 || h == 14) ? x : z);
        //return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);

        switch (hash & 0xF)
        {
            case 0x0: return x + y;
            case 0x1: return -x + y;
            case 0x2: return x - y;
            case 0x3: return -x - y;
            case 0x4: return x + z;
            case 0x5: return -x + z;
            case 0x6: return x - z;
            case 0x7: return -x - z;
            case 0x8: return y + z;
            case 0x9: return -y + z;
            case 0xA: return y - z;
            case 0xB: return -y - z;
            case 0xC: return y + x;
            case 0xD: return -y + z;
            case 0xE: return y - x;
            case 0xF: return -y - z;
            default: return 0; // never happens
        }
    }

    private void GenerateNumbers()
    {
        for(int i = 0; i < preGeneratedNumbers.Length; i++)
        {
            preGeneratedNumbers[i] = random.Next();
        }
    }

    // Deterministic generator
    public class XorShiftRandom
    {
        private int x, y;
        public XorShiftRandom(int seed)
        {
            x = seed << 1;
            y = seed >> 1;
        }

        public int Next()
        {
            int _x, _y, res;
            _x = y;
            x ^= x << 23;
            _y = x ^ y ^ (x >> 17) ^ (y >> 26);
            
            res = _y + y;
            x = _x;
            y = _y;

            return res;
        }
    }
}
