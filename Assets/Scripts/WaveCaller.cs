using UnityEngine;

public class WaveCaller : Singleton<WaveCaller>, IInteractable
{
    private string _message;
    string IInteractable.InteractMessage { get => _message; }
    private ParticleSystem particle;
    void Start()
    {
        _message = "Press E to call next wave.";
        particle = GetComponentInChildren<ParticleSystem>();
    }
    public void Interact()
    {
        if (LevelManager.Instance.IsSpawning())
        {
            PopupManager.Instance.Pop("Cannot call next wave while the current wave is spawning.");
        }
        else
        {
            LevelManager.Instance.CallWave();
        }
    }
    public void EnableCall()
    {
        _message = "Press E to call next wave.";
        particle.gameObject.SetActive(true);
    }
    public void DisableCall()
    {
        _message = "Cannot call next wave while the current wave is spawning.";
        particle.gameObject.SetActive(false);
    }
}