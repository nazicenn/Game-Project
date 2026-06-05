using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float laneChangeSpeed = 15f;
    public float jumpForce = 7f;

    private float[] lanePositions = { -2.5f, 0f, 2.5f };
    private int currentLane = 1;
    private Rigidbody rb;
    private bool isGrounded;
    private bool jumpRequested;
    private float groundCheckDistance = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        transform.position = new Vector3(0, 0.3f, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            currentLane = Mathf.Max(0, currentLane - 1);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            currentLane = Mathf.Min(2, currentLane + 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }

        CheckIfGrounded();
    }

    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(
            lanePositions[currentLane],
            rb.position.y,
            0
        );

        rb.MovePosition(Vector3.MoveTowards(
            rb.position,
            targetPos,
            laneChangeSpeed * Time.fixedDeltaTime
        ));

        if (jumpRequested)
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
            jumpRequested = false;
        }
    }

    void CheckIfGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance + 0.3f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
                return;
            }
        }

        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            ScoreManager.Instance.AddCoin();
            Destroy(other.gameObject);
            return;
        }

        if (other.CompareTag("Obstacle"))
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();

            if (obstacle != null && obstacle.obstacleType == ObstacleType.Air)
            {
                GameManager.Instance.GameOver();
            }
            else 
            {
                if (isGrounded)
                {
                    GameManager.Instance.GameOver();
                }
            }
        }
    }
}