using UnityEngine;

public class Crystal : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out BaseEnemy baseEnemy))
        {
            LevelManager.Instance.DecreaseLives(baseEnemy.stat.lives);
            baseEnemy.Die();
            audioSource.volume = LevelManager.Instance.GetVolume()/200f;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}