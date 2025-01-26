using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBullet : MonoBehaviour
{

    public ParticleSystem particles;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Bubble" &&  collision.gameObject.tag != "Player")
        {
            Destroy(transform.gameObject);

            if (collision.gameObject.tag == "Enemy")
            {
                EnemyController targetEnemyController = collision.gameObject.GetComponent<EnemyController>();
                targetEnemyController.DamageEnemy(25f);
            }
        }
    }
}
