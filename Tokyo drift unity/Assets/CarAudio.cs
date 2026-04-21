using UnityEngine;

public class CarAudio : MonoBehaviour
{
    [Header("Speed Settings")]
    public float minSpeed = 0f;
    public float maxSpeed = 3000f;

    private float currentSpeed;

    [Header("Audio Settings")]
    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;

    private AudioSource carAudio;

    void Start()
    {
        carAudio = GetComponent<AudioSource>();

        // Make sure audio is playing
        if (!carAudio.isPlaying)
        {
            carAudio.loop = true;
            carAudio.Play();
        }
    }

    void Update()
    {
        EngineSound();
        Debug.Log(carAudio.pitch);
    }

    // Called from your movement script
    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    void EngineSound()
    {
        // Convert speed to 0–1 range
        float t = Mathf.InverseLerp(minSpeed, maxSpeed, currentSpeed);

        // Target pitch based on speed
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, t);

        // Smooth transition (prevents jittery sound)
        carAudio.pitch = Mathf.Lerp(carAudio.pitch, targetPitch, Time.deltaTime * 5f);
    }
}