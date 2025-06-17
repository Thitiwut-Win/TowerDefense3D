using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    private Transform target;
    public float turnSpeed;
    void Update()
    {
        if (target != null)
        {
            Vector3 targetDirection = target.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
    public void SetTarget(Transform transform)
    {
        target = transform;
    }
    public bool hasTarget()
    {
        return target != null;
    }
}