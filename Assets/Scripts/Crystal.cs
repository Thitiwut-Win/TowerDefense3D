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
            if (baseEnemy.stat.eTeam == ETeam.ENEMY)
            {
                PopupManager.Instance.Pop("Warning : Enemy is attacking the crystal!", Color.red);
                audioSource.volume = GameManager.Instance.GetVolume() / 300f;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }
    }
}