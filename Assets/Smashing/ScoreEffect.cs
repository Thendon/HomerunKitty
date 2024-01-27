using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEffect : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public float liveTime = 2.0f;
    public List<AudioClip> soundEffects = new List<AudioClip>();
    public AudioSource audioSource = null;
    public Vector3 movementVector = new Vector3();

    float deathTime;

    private void Awake()
    {
        deathTime = Time.time + liveTime;
    }

    public void Init(float amount)
    {
        text.text = Mathf.RoundToInt(amount).ToString();
        AudioClip soundEffect = soundEffects[Random.Range(0, soundEffects.Count)];
        audioSource.PlayOneShot(soundEffect);
    }

    private void Update()
    {
        transform.position += transform.InverseTransformVector(movementVector) * Time.deltaTime;

        if (Time.time > deathTime)
            Destroy(gameObject);
    }
}
