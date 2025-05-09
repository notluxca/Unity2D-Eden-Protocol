using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public InsectEnemy insectEnemy;
    public NavMeshAgent agent;
    public Rigidbody2D rb;

    private float knockbackTimer = 0f;
    private const float knockbackDuration = 0.2f;

    private bool isAttacking = false;
    private float attackCooldown = 2f;
    private float lastAttackTime = -Mathf.Infinity;

    private float attackRange = 2f;
    private float attackForce = 3f;

    private void Start()
    {
        SetupNavMeshAgentFor2D();
        // insectEnemy = GetComponent<InsectEnemy>();
        // agent = GetComponent<NavMeshAgent>();
        // rb = GetComponent<Rigidbody2D>();
    }

    private void SetupNavMeshAgentFor2D()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = insectEnemy.speed;
    }

    public void Tick(Transform target)
    {
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
            return;
        }

        if (isAttacking) return;

        float distance = Vector2.Distance(insectEnemy.transform.position, target.position);
        if (Time.time >= lastAttackTime + attackCooldown && distance <= attackRange)
        {
            PerformAttack(target);
        }
        else
        {
            MoveTowards(target);
        }
    }

    public void MoveTowards(Transform target)
    {
        Vector2 offset = GetPerlinOffset() * insectEnemy.variationAmount;
        Vector2 targetPosition = (Vector2)target.position + offset;

        agent.isStopped = false;
        agent.SetDestination(targetPosition);

        if (spriteRenderer != null)
            spriteRenderer.flipX = target.position.x < insectEnemy.transform.position.x;
    }

    public void ApplyKnockback(float duration)
    {
        knockbackTimer = duration;
        agent.ResetPath();
    }

    private Vector2 GetPerlinOffset()
    {
        float offsetX = Mathf.PerlinNoise(Time.time * insectEnemy.variationSpeed, 0f) - 0.5f;
        float offsetY = Mathf.PerlinNoise(0f, Time.time * insectEnemy.variationSpeed) - 0.5f;
        return new Vector2(offsetX, offsetY);
    }

    private void PerformAttack(Transform target)
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        agent.isStopped = true;
        rb.linearVelocity = Vector2.zero;

        Vector2 dir = (target.position - insectEnemy.transform.position).normalized;
        rb.AddForce(dir * attackForce, ForceMode2D.Impulse);

        insectEnemy.StartCoroutine(EndAttackRoutine());
    }

    private System.Collections.IEnumerator EndAttackRoutine()
    {
        yield return new WaitForSeconds(0.6f); // tempo de ataque

        rb.linearVelocity = Vector2.zero;
        agent.isStopped = false;
        isAttacking = false;
    }
}
