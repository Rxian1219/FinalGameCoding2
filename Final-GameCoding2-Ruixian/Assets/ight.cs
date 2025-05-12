using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ight : MonoBehaviour
{
    public Light directionalLight; 

    public float currentTime = 0f; 

    private Color color1 = new Color(1f, 0.968f, 0.91f);     // 255,247,232
    private Color color2 = new Color(1f, 0.776f, 0.357f);     // 255,198,91
    private Color color3 = new Color(0.263f, 0.035f, 1f);     // 67,9,255

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime <= 180f)
        {
            directionalLight.color = color1;
        }
        else if (currentTime <= 270f)
        {
            directionalLight.color = color2;
        }
        else if (currentTime <= 450f)
        {
            directionalLight.color = color3;
        }
    }
}
