using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float bubbleForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform aim;
    [SerializeField] private Transform bubbleSpawnPoint;
    [SerializeField] private GameObject bubble;
    [SerializeField] private Slider playerHealthbar;

    public float playerHealth = 100f;
    public float playerMaxHealth = 100f;
    public float knockbackForceOnPlayer = 100f;
    public float knockbackOnPlayerDuration = 1f;

    public AudioSource sfx;

    private float moveX;
    private float moveY;

    private Vector2 movementVector;
    private Vector3 mousePosition;
    private Vector3 prevPosition;

    private bool damageDebounce = false;
    private bool canShoot = true;

    private WaitForSeconds damageDebounceDelay = new WaitForSeconds(0.25f);
    private WaitForSeconds bubbleShootDelay = new WaitForSeconds(0.3f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GatherInputs();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Gather all of the player inputs
    void GatherInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        movementVector = new Vector2(moveX, moveY).normalized;

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && canShoot && GameManager.Instance.paused == false)
        {
            ShootBubble();
        }
    }
    
    // Move the player
    void MovePlayer()
    {
        // Setting velocity of the rigidbody
        rb.velocity = new Vector2 (movementVector.x * moveSpeed, movementVector.y * moveSpeed);

        // Aim the claw at the mouse
        var aimDirection = (mousePosition - transform.position).normalized;
        float aimAngle = Mathf.Atan2(aimDirection.y,aimDirection.x) * Mathf.Rad2Deg;
        aim.transform.eulerAngles = new Vector3(0,0, aimAngle);

        // Rotate body
        var playerEulers = transform.eulerAngles;
        playerEulers.z = Mathf.LerpAngle(playerEulers.z, aimAngle, rotationSpeed * Time.deltaTime);

        transform.eulerAngles = playerEulers;
        //transform.eulerAngles = new Vector3(0, 0, aimAngle);
    }

    void ShootBubble()
    {
        sfx.Play();
        canShoot = false;
        GameObject bubbleClone = Instantiate(bubble, bubbleSpawnPoint.position, bubbleSpawnPoint.rotation);

        Rigidbody2D bubbleCloneRB = bubbleClone.GetComponent<Rigidbody2D>();

        bubbleCloneRB.AddForce(bubbleSpawnPoint.up * bubbleForce, ForceMode2D.Impulse);
        Destroy(bubbleClone, 5f);

        StartCoroutine(BubbleDelay());
    }

    void DamagePlayer(float damageAmount)
    {
        playerHealth -= damageAmount;
        UpdateHealthBar();

        if (playerHealth <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.LevelOver(false);
        }
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!damageDebounce)
            {
                damageDebounce = true;
                EnemyController targetEnemyController = collision.gameObject.GetComponent<EnemyController>();

                DamagePlayer(targetEnemyController.enemyDamage);

                StartCoroutine(damageDebounceResetter());
                StartCoroutine(Knockback(collision.gameObject.transform));
            }
        }else if (collision.gameObject.tag == "Ink")
        {
            if (!damageDebounce)
            {
                damageDebounce = true;

                DamagePlayer(5);

                StartCoroutine(damageDebounceResetter());
                StartCoroutine(Knockback(collision.gameObject.transform));
            }
        }
    }

    public void UpdateHealthBar()
    {
        playerHealthbar.value = playerHealth / playerMaxHealth;
    }

    IEnumerator Knockback(Transform enemyTransform)
    {
        float timer = 0;

        while (knockbackOnPlayerDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 direction = (transform.position - enemyTransform.position).normalized;
            rb.AddForce(direction * knockbackForceOnPlayer);
        }

        yield return null;
    }

    IEnumerator damageDebounceResetter()
    {
        yield return damageDebounceDelay;
        damageDebounce = false;
    }

    IEnumerator BubbleDelay()
    {
        yield return bubbleShootDelay;
        canShoot = true;
    }

}
