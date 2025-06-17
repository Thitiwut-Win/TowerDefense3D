using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHologram : MonoBehaviour
{
    [SerializeField] private BaseTower towerPrefab;
    [SerializeField] private Material invalid;
    [SerializeField] private Material valid;
    [SerializeField] private LayerMask layerMask;
    private bool isValid = true;
    private List<Collider> invalidList = new List<Collider>();
    public delegate void OnComplete(bool val);
    public static event OnComplete onComplete;
    void Start()
    {
        ApplyFeedback();
    }
    void Update()
    {
        // print(invalidList.Count);
        if (!isValid && invalidList.Count == 0)
        {
            isValid = true;
            ApplyFeedback();
        }
        else if (isValid && invalidList.Count != 0)
        {
            isValid = false;
            ApplyFeedback();
        }
    }
    public void OnTriggerEnter(Collider collider)
    {
        // print(collider.gameObject.name);
        if (collider.gameObject.layer != LayerMask.NameToLayer("Ground")) invalidList.Add(collider);
    }
    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer != LayerMask.NameToLayer("Ground")) invalidList.Remove(collider);
    }
    private void ApplyFeedback()
    {
        Material material = isValid ? valid : invalid;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = material;
        }
    }
    public IEnumerator Build(Vector3 pos)
    {
        if (!isValid) yield return null;
        else
        {
            yield return Instantiate(towerPrefab, pos, Quaternion.identity);
            if (onComplete != null)
            {
                onComplete(true);
            }
            Cancel();
        }

    }
    public void Cancel()
    {
        Destroy(gameObject);
    }
}