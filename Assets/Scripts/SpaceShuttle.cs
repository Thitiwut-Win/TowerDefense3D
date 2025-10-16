using UnityEngine;

public class SpaceShuttle : PoolableObject
{
    public float speed = 12;
    private Rigidbody rb;
    private Vector3 target;
    private bool isInPosition = true;
    public delegate void OnGetInPosition(SpaceShuttle spaceShuttle);
    public OnGetInPosition onGetInPosition;
    private int state = 0;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (isInPosition) return;
        if (!hasReachedTarget()) Move();
        else Stop();
    }
    private void Move()
    {
        Vector3 direc = target - transform.position;
        direc = direc.normalized;
        rb.linearVelocity = new Vector3(speed * direc.x, speed * direc.y, speed * direc.z);
        Quaternion nextRotation = Quaternion.LookRotation(direc, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, 3 * Time.deltaTime);
    }
    private void Stop()
    {
        rb.linearVelocity = Vector3.zero;
        isInPosition = true;
        if (state == 2)
        {
            PoolManager.Instance.Despawn(this);
        }
        else if (onGetInPosition != null)
        {
            onGetInPosition(this);
        }
    }
    public void MoveToPosition(Vector3 pos, float y)
    {
        target = pos;
        target.y = y;
        isInPosition = false;
        state++;
    }
    private bool hasReachedTarget()
    {
        return Vector3.Distance(target, transform.position) <= 0.15;
    }
    public override void OnSpawn()
    {
        state = 0;
        isInPosition = true;
    }
    public override void OnDespawn()
    {
        rb.linearVelocity = Vector3.zero;
    }
}