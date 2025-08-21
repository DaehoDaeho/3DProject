using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    public float volume = 0.8f;
    public bool randomPitch = true;
    public float pitchMin = 0.95f;
    public float pitchMax = 1.05f;

    AudioSource src;

    void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void Play(int index)
    {
        if (clips == null) return;
        if (clips.Length == 0) return;

        if (index < 0 || index >= clips.Length)
        {
            index = Random.Range(0, clips.Length);
        }

        if (randomPitch)
        {
            src.pitch = Random.Range(pitchMin, pitchMax);
        }
        else
        {
            src.pitch = 1f;
        }

        src.PlayOneShot(clips[index], volume);
    }
}
