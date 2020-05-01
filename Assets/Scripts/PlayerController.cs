using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement
    public float ACCELERATION_TIME_VALUE;
    public float accelerationSpeed;
    public float topSpeed;

    private float horizontalMovement;
    private float accelerationTime;

    //Jump
    public LayerMask groundLayer;
    public Transform leftGroundPosition;
    public Transform rightGroundPosition;
    public float jumpForce;
    public float jumpingGravity;
    public float landingGravity;
    public int EXTRA_JUMPS_VALUE;

    private bool isGrounded;
    private int extraJumps;

    // Component
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        accelerationTime = ACCELERATION_TIME_VALUE;
        extraJumps = EXTRA_JUMPS_VALUE;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        if (horizontalMovement > 0 || horizontalMovement < 0)
        {
            accelerationTime -= Time.deltaTime;
        }
        else
        {
            accelerationTime = ACCELERATION_TIME_VALUE;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            JumpPlayer();
        }

        if (isGrounded) 
        {
            rb.gravityScale = jumpingGravity;
            extraJumps = EXTRA_JUMPS_VALUE;
        }

        if (rb.velocity.y > 0)
        {
            rb.gravityScale = jumpingGravity;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = landingGravity;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapArea(leftGroundPosition.position, rightGroundPosition.position, groundLayer);

        MovePlayer();
    }

    void MovePlayer()
    {
        if (accelerationTime > 0)
        {
            rb.velocity = new Vector2(horizontalMovement * accelerationSpeed, rb.velocity.y);
        }
        else if (accelerationTime <= 0)
        {
            rb.velocity = new Vector2(horizontalMovement * topSpeed, rb.velocity.y);
        }
    }

    void JumpPlayer()
    {
        if (extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (extraJumps == 0 && (isGrounded))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(0, 1, 0, 0.5f);
    //    Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y -0.505f),
    //                    new Vector2(1, 0.01f));
    //}
}
