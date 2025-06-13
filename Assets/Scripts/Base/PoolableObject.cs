using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{
    public abstract void OnSpawn();
    public abstract void OnDespawn();
}