using System.Collections;
using UnityEngine;

public class Projectile : PoolableObject
{
    private Transform target;
    public float speed;
    public float damage;
    public ETeam eTeam;
    protected Rigidbody rb;
    [SerializeField] private Particle particle;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected virtual void Update()
    {
        MoveToTarget();
    }
    private void MoveToTarget()
    {
        if (ValidTarget())
        {
            Vector3 direc = target.position - transform.position;
            direc = direc.normalized;
            rb.linearVelocity = new Vector3(direc.x * speed, direc.y * speed, direc.z * speed);
        }
        else
        {
            DestroySelf();
        }
    }
    public void SetTarget(Transform transform)
    {
        target = transform;
    }
    public void OnTriggerEnter(Collider collider)
    {
        BaseUnit baseUnit = collider.GetComponent<BaseUnit>();
        BaseTower baseTower = collider.GetComponent<BaseTower>();
        if (baseUnit != null && eTeam != baseUnit.stat.eTeam)
        {
            baseUnit.GetHit(damage);
            PoolManager.Instance.Spawn<Particle>(particle.name, transform.position, transform.rotation);
            PoolManager.Instance.Despawn(this);
        }
    }
    private bool ValidTarget()
    {
        return target != null && target.gameObject.activeSelf;
    }
    private IEnumerator StartDestroy()
    {
        yield return new WaitForSeconds(6);
        DestroySelf();
    }
    private void DestroySelf()
    {
        PoolManager.Instance.Despawn(this);
    }
    public override void OnSpawn()
    {
        StartCoroutine(StartDestroy());
    }
    public override void OnDespawn()
    {
        target = null;
    }
    public IEnumerator PostSpawn()
    {
        yield return target != null;
        MoveToTarget();
    }
}