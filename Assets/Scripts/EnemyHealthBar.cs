using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private BaseEnemy baseEnemy;
    [SerializeField] private SpriteRenderer spriteRenderer;
    void Update()
    {
        spriteRenderer.size = new Vector2(baseEnemy.stat.health/baseEnemy.stat.maxHealth, 1);
    }
}