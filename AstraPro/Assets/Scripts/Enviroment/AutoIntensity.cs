using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoIntensity : MonoBehaviour {

    public Gradient nightDayColor;

    public float maxIntensity = 3f;
    public float minIntensity = 0f;
    public float minPoint = -0.2f;

    public float maxAmbient = 1f;
    public float minAmbient = 0f;
    public float minAmbientPoint = -0.2f;

    public GameObject lightSwitch;

    public Gradient nightDayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;

    public float dayAtmosphereThickness = 0.4f;
    public float nightAtmosphereThickness = 0.87f;

    public Vector3 dayRotateSpeed;
    public Vector3 nightRotateSpeed;

    float skySpeed = 1;

    float timecount;
    float timecount_2;

    Light mainLight;
    Skybox sky;
    Material skyMat;

    bool day, night;
    int dayCount;

    void Start()
    {

        mainLight = GetComponent<Light>();
        skyMat = RenderSettings.skybox;
        dayCount = 1;
        night = false;
        day = true;
    }

    void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.isPaused) return;

        float tRange = 1 - minPoint;
        float dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minPoint) / tRange);
        float i = ((maxIntensity - minIntensity) * dot) + minIntensity;

        //Debug.Log("Dot is " + dot.ToString() + ", Day is " + dayCount.ToString() + ", Day is " + day.ToString() + ", Night is " + night.ToString());

        if (Score.Instance != null)
        {
            if (dot == 0 && day == true && night == false)
            {
                // Count Days required to clear game
                if (Score.Instance.maxStar == true)
                {
                    dayCount++;
                }

                day = false;
                night = true;

                // Make customer wait for shorter timing as day past
                foreach(var pair in CustomerSpawner.Instance.customerDic)
                {
                    pair.Value.waitTiming -= (pair.Value.waitTiming * 0.2f);
                }

                Debug.Log("Its a new day my dude. Day: " + dayCount.ToString());
            }
            else if (dot > 0.99f && day == false && night == true)
            {
                day = true;
                night = false;
            }
            if (dayCount >= 3)
            {
                Menu_Manager.Instance.WinGame();
            }
        }
        mainLight.intensity = i;

        tRange = 1 - minAmbientPoint;
        dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
        i = ((maxAmbient - minAmbient) * dot) + minAmbient;
        RenderSettings.ambientIntensity = i;

        mainLight.color = nightDayColor.Evaluate(dot);
        RenderSettings.ambientLight = mainLight.color;

        RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

        i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
        skyMat.SetFloat("_AtmosphereThickness", i);

        if (dot > 0)
            transform.Rotate(dayRotateSpeed * Time.deltaTime * skySpeed);
        else
            transform.Rotate(nightRotateSpeed * Time.deltaTime * skySpeed);

        if (Input.GetKeyDown(KeyCode.Q)) skySpeed *= 0.5f;
        if (Input.GetKeyDown(KeyCode.E)) skySpeed *= 2f;

        if (gameObject.GetComponent<Light>().intensity < 0.6)
        {
            lightSwitch.SetActive(true);
        }
        else if (gameObject.GetComponent<Light>().intensity > 0.6)
        {
            lightSwitch.SetActive(false);
        }
    }
}
