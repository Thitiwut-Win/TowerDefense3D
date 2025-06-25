using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHologram : MonoBehaviour
{
    [SerializeField] private BaseTower towerPrefab;
    [SerializeField] private Material invalid;
    [SerializeField] private Material valid;
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
        if (!isValid)
        {
            PopupManager.Instance.Pop("Invalid placement");
            yield return null;
        }
        else if(LevelManager.Instance.GetMoney() < towerPrefab.towerStat.cost)
        {
            PopupManager.Instance.Pop("Not enough money to build");
        }
        else
        {
            TowerBuilderManager.Instance.Build(towerPrefab, pos);
            LevelManager.Instance.IncreaseMoney(-towerPrefab.towerStat.cost);
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