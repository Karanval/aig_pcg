using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text displayText;
    float frameCount = 0.0f;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;  // 4 updates per sec.

    private void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0f / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
        displayText.text = fps.ToString() + " FPS";
    }
}

