using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EngineAudio : MonoBehaviour
{
    public AudioSource runningSound;
    public float runningMaxVolume;
    public float runningMaxPitch;
    public AudioSource reverseSound;
    public float reverseMaxVolume;
    public float reverseMaxPitch;
    public AudioSource idleSound;
    public float idleMaxVolume;
    private float speedRatio;
    private float revLimiter;
    public float LimiterSound = 1f;
    public float LimiterFrequency = 3f;
    public float LimiterEngage = 0.8f;
    public bool isEngineRunning = false;

    [SerializeField] double ratioCounter;

    public AudioSource startingSound;


    private CarMovement carController;
    // Start is called before the first frame update
    void Awake()
    {
        idleSound.volume = 0;
        carController = GetComponent<CarMovement>();
        runningSound.volume = 0;
        reverseSound.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused)
        {
            idleSound.volume = 0;
            reverseSound.volume = 0;
            runningSound.volume = 0;
            return;
        }

        float speedSign = 0;
        if (carController)
        {
            speedSign = Mathf.Sign(carController.GetSpeedRatio());
            speedRatio = Mathf.Abs(carController.GetSpeedRatio());
        }

        if (speedRatio > LimiterEngage)
        {
            revLimiter = (Mathf.Sin(Time.time * LimiterFrequency) + 1f) * LimiterSound * (speedRatio - LimiterEngage);
        }
        if (isEngineRunning)
        {
            idleSound.volume = Mathf.Lerp(0.1f, idleMaxVolume, speedRatio);
            if (speedSign > 0 && Mathf.Abs(speedRatio) > ratioCounter)
            {
                idleSound.volume = 0;
                reverseSound.volume = 0;
                runningSound.volume = Mathf.Lerp(0.1f, runningMaxVolume, speedRatio);
                runningSound.pitch = Mathf.Lerp(runningSound.pitch, Mathf.Lerp(0.1f, runningMaxPitch, speedRatio) + revLimiter, speedRatio);
            }
            else if (speedSign < 0 && Mathf.Abs(speedRatio) > ratioCounter)
            {
                idleSound.volume = 0;
                runningSound.volume = 0;
                reverseSound.volume = Mathf.Lerp(0.2f, reverseMaxVolume, speedRatio);
                reverseSound.pitch = Mathf.Lerp(reverseSound.pitch, Mathf.Lerp(0.2f, reverseMaxPitch, speedRatio) + revLimiter, speedRatio);
            }

            else if (carController.GetSpeedRatio() > -0.1f && carController.GetSpeedRatio() < 0.1f)
            {
                reverseSound.volume = 0;
                runningSound.volume = 0;
                idleSound.volume = Mathf.Lerp(0.1f, idleMaxVolume, speedRatio);
            }
        }
        else
        {
            idleSound.volume = 0;
            runningSound.volume = 0;
            reverseSound.volume = 0;
        }
    }
    public IEnumerator StartEngine()
    {
        startingSound.Play();
        carController.isEngineRunning = 1;
        yield return new WaitForSeconds(0.6f);
        isEngineRunning = true;
        yield return new WaitForSeconds(0.4f);
        carController.isEngineRunning = 2;
    }
}