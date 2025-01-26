using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody2D enemyRB;
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    public float enemyMoveSpeed = 20f;
    public float enemyDamage = 25f;
    public float enemyAgroRange = 15f;
    public float enemyDeAgroRange = 15f;
    public Sprite deathSprite;

    public List<GameObject> nodes = new List<GameObject>();

    public bool endlessMode = true;

    public Healthbar healthbar;

    Vector3 enemyStartPosition;
    Vector3 currentTargetPosition;

    int currentNode = 0;
    
    enum EnemyState
    {
        Roaming,
        Chasing
    }

    EnemyState currentEnemyState;
    private void Start()
    {
        enemyStartPosition = transform.position;
        
        if (endlessMode)
        {
            currentEnemyState = EnemyState.Chasing;
        }
        else
        {
            currentEnemyState = EnemyState.Roaming;
            NextNode();
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform)
        {
            switch (currentEnemyState)
            {
                default:

                case EnemyState.Roaming:

                    if (Vector3.Distance(transform.position, currentTargetPosition) < 1f)
                    {
                        NextNode();
                    }
                    else if (Vector3.Distance(transform.position, playerTransform.position) < enemyAgroRange)
                    {
                        currentEnemyState = EnemyState.Chasing;
                    }
                    else
                    {
                        MoveEnemy(currentTargetPosition);
                    }

                    break;

                case EnemyState.Chasing:

                    if (Vector3.Distance(transform.position, playerTransform.position) > enemyDeAgroRange && !endlessMode)
                    {
                        currentEnemyState = EnemyState.Roaming;
                    }

                    MoveEnemy(playerTransform.position);

                    break;

            }
        }
       
    }

    void GetRandomPosition()
    {
        currentTargetPosition = transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
    }

    void NextNode()
    {
        currentNode++;

        if (currentNode > nodes.Count - 1)
            currentNode = 0;

        currentTargetPosition = nodes[currentNode].transform.position;

    }

    public void DamageEnemy(float damage)
    {
        currentHealth -= damage;
        healthbar.UpdateHealthBar(currentHealth,maxHealth);

        if (currentHealth <= 0)
        {
            GameManager.Instance.EnemyKilled(transform.gameObject);

            transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InOutQuad);
            transform.DORotate(new Vector3(0, 0, 540), 0.25f, RotateMode.FastBeyond360).SetEase(Ease.InCirc);


            Destroy(healthbar.gameObject,0.25f);
            Destroy(this.gameObject,0.25f);
        }
    }

    void MoveEnemy(Vector3 targetPos)
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * enemyMoveSpeed);
        enemyRB.MovePosition(newPosition);

        var aimDirection = (targetPos - transform.position).normalized;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, aimAngle);
    }
}
