using System.Collections;
using UnityEngine;

public class Particle : PoolableObject
{
    public float countdownTime;
    public float damage;
    public float colliderTime;
    private Collider _collider;
    void Start()
    {
        _collider = GetComponent<Collider>();
    }
    public void OnTriggerEnter(Collider collider)
    {
        BaseUnit baseUnit = collider.GetComponent<BaseUnit>();
        if (baseUnit != null)
        {
            baseUnit.GetHit(damage);
        }
    }
    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(countdownTime);
        PoolManager.Instance.Despawn(this);
    }
    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.01f);
        _collider.enabled = true;
    }
    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(colliderTime);
        _collider.enabled = false;
    }
    public override void OnSpawn()
    {
        StartCoroutine(Countdown());
        StartCoroutine(EnableCollider());
        StartCoroutine(DisableCollider());
    }
    public override void OnDespawn()
    {
        _collider.enabled = false;
    }
}