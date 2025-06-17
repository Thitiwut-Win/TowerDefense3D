using UnityEngine;

public class Crystal : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out BaseEnemy baseEnemy))
        {
            PoolManager.Instance.Despawn(baseEnemy);
        }
    }
}