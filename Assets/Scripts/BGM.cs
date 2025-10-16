using UnityEngine;

public class BGM : Singleton<BGM>
{
    private AudioSource audioSource;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}