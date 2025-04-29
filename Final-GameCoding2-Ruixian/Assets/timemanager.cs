using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timemanager : MonoBehaviour
{

    public Material daySkybox;
    public Material sunsetSkybox;
    public Material nightSkybox;

    public ParticleSystem rainParticle;

    private float timer = 0f;
    private float totalTimer = 0f;

    // 时间节点（秒）
    public float dayDuration = 180f;       // 白天 3分钟
    public float sunsetDuration = 90f;     // 夕阳 1.5分钟
    public float nightDuration = 180f;     // 夜晚 3分钟

    private enum TimePhase { Day, Sunset, Night }
    private TimePhase currentPhase = TimePhase.Day;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = daySkybox;
    }

    // Update is called once per frame
    void Update()
    {
        // timer = day noon and night time goes, total timer = the game's time goes
        timer += Time.deltaTime;

        totalTimer += Time.deltaTime;

        switch (currentPhase)
        {
            case TimePhase.Day:
                if (timer >= dayDuration)
                {
                    timer = 0f;
                    currentPhase = TimePhase.Sunset;
                    RenderSettings.skybox = sunsetSkybox;
                }
                break;

            case TimePhase.Sunset:
                if (timer >= sunsetDuration)
                {
                    timer = 0f;
                    currentPhase = TimePhase.Night;
                    RenderSettings.skybox = nightSkybox;
                }
                break;

            case TimePhase.Night:
                // 夜晚结束以后你想做什么？重启？结束游戏？暂时可以留空
                break;
        }

        // 控制雨粒子特效
        bool isRaining =
            (totalTimer >= 120f && totalTimer < 180f) ||
            (totalTimer >= 300f && totalTimer < 360f);

        if (isRaining && !rainParticle.isPlaying)
        {
            rainParticle.Play();
        }
        else if (!isRaining && rainParticle.isPlaying)
        {
            rainParticle.Stop();
        }

    }
}
