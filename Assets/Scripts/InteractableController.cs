using TMPro;
using UnityEngine;

public class InteractableController : Singleton<InteractableController>
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private float interactDistance;
    private IInteractable target;
    void Update()
    {
        UpdateCurrentInteractable();
        UpdateInteractionText();
        CheckForInteractionInput();
    }
    private void UpdateCurrentInteractable()
    {
        target = null;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out var hit, interactDistance))
        {
            target = hit.collider?.GetComponent<IInteractable>();
        }
    }
    private void UpdateInteractionText()
    {
        if (target == null)
        {
            interactText.text = string.Empty;
            return;
        }
        interactText.text = target.InteractMessage;
    }
    private void CheckForInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && target != null)
        {
            target.Interact();
        }
    }
}