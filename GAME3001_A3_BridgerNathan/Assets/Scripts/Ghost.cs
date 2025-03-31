using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Ghost : MonoBehaviour
{
    public GameObject ghost;
    public GameObject Player;
    public GameObject PointA;
    public GameObject PointB;

    public TextMeshProUGUI stateText;

    private Rigidbody2D rb;
    private Transform currentPoint;

    [SerializeField] public float moveSpeed;
    [SerializeField] public float rotationSpeed;

    private bool idle = true;
    private bool patrol = false;
    private bool isAttacking = false;

    private SpriteRenderer spriteRenderer;
    private bool isWaiting = false;

    void Start()
    {
        spriteRenderer = ghost.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentPoint = PointB.transform;

        StartCoroutine(StartPatrolAfterIdle(3f));
    }

    void Update()
    {
        Idle();
        Attack();
        Patrol();
        UpdateStateText();
    }

    void Idle()
    {
        if (idle)
        {
            // Determine the direction the sprite is facing based on flipX
            Vector3 rayDirection = ghost.transform.localScale.x < 0 ? Vector3.left : Vector3.right;

            // Cast a ray in the direction the sprite is facing
            RaycastHit2D ray = Physics2D.Raycast(ghost.transform.position, rayDirection, 20f);

            // Visualize the ray in the scene view for debugging
            Debug.DrawRay(ghost.transform.position, rayDirection * 20f, Color.green);

            if (ray.collider != null)
            {
                idle = false;
                isAttacking = true;
            }


        }

    }

    void Patrol()
    {
        if (patrol && !isWaiting)
        {
            // Determine the direction the sprite is facing based on flipX
            Vector3 rayDirection = ghost.transform.localScale.x < 0 ? Vector3.left : Vector3.right;

            // Cast a ray in the direction the sprite is facing
            RaycastHit2D ray = Physics2D.Raycast(ghost.transform.position, rayDirection, 20f);

            // Visualize the ray in the scene view for debugging
            Debug.DrawRay(ghost.transform.position, rayDirection * 20f, Color.green);

            if (ray.collider != null)
            {
                patrol = false;
                isAttacking = true;
            }

            Vector2 point = currentPoint.position - transform.position;

            if (currentPoint == PointB.transform)
            {
                rb.linearVelocity = new Vector2(moveSpeed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(-moveSpeed, 0);
            }

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
            {
                StartCoroutine(IdleForSeconds(3f));

                if (currentPoint == PointB.transform)
                {
                    currentPoint = PointA.transform;
                }
                else
                {
                    currentPoint = PointB.transform;
                }
            }
        }
    }

    IEnumerator StartPatrolAfterIdle(float delay)
    {
        yield return new WaitForSeconds(delay);  // Wait for 3 seconds
        idle = false;  // Exit idle state
        patrol = true;  // Start patrol
    }

    IEnumerator IdleForSeconds(float seconds)
    {
        isWaiting = true;  // Prevent the patrol from continuing during idle
        idle = true;  // Trigger the idle state
        rb.linearVelocity = Vector2.zero;  // Stop movement by setting velocity to 0
        yield return new WaitForSeconds(seconds);  // Wait for 3 seconds

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        idle = false;  // Exit idle state
        patrol = true;  // Resume patrol
        isWaiting = false;  // Allow patrol to continue
    }

    void Attack()
    {
        if (isAttacking)
        {
            idle = false;
            patrol = false;

            Vector3 direction = ghost.transform.position - Player.transform.position;
            direction.Normalize();
            ghost.transform.position -= direction * moveSpeed * Time.fixedDeltaTime;

            Debug.DrawLine(ghost.transform.position, ghost.transform.position - direction * 2.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (idle)
            if (collider.gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene("Victory Scene");
            }

        if (patrol)
            if (collider.gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene("Victory Scene");
            }

        if (isAttacking)
            if (collider.gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene("Defeat Scene");
            }

    }

    void UpdateStateText()
    {
        if (stateText != null)
        {
            if (idle)
            {
                stateText.text = "State: Idle";
            }
            else if (patrol)
            {
                stateText.text = "State: Patrolling";
            }
            else if (isAttacking)
            {
                stateText.text = "State: Attacking";
            }

        }
    }
}
