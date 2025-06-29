using System.Collections;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    [SerializeField] private BaseEnemy target;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    public float turnSpeed;
    [SerializeField] private BaseTower baseTower;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (hasTarget())
        {
            Vector3 targetDirection = target.transform.position - transform.position;
            targetDirection.y = 0.5f - transform.position.y;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
    public void SetTarget(BaseEnemy baseEnemy)
    {
        target = baseEnemy;
    }
    public bool hasTarget()
    {
        if (target != null && target.IsDead())
        {
            if(baseTower.targetList.Contains(target)) baseTower.targetList.Remove(target);
            target = null;
        }
        return target != null;
    }
    public void Attack(Projectile prefab, float volumeMod = 1)
    {
        if (hasTarget())
        {
            if (prefab != null)
            {
                Projectile projectile = PoolManager.Instance.Spawn<Projectile>(prefab.name, transform.position + transform.forward * 0.5f, Quaternion.identity);
                projectile.SetTarget(target.transform);
                projectile.PostSpawn();
            }
            audioSource.volume = GameManager.Instance.GetVolume()/100f * volumeMod;
            audioSource.clip = audioClip;
            audioSource.pitch = Random.Range(1f, 3f);
            audioSource.Play();
        }
    }
}