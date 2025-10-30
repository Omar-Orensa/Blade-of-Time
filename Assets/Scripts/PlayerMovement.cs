using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 10f;
    public Transform groundCheck;      // empty child at feet
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;
    [SerializeField] private Animator animator;
    [SerializeField]  private Rigidbody2D rb;
    bool grounded;
    float move;

    float baseScaleX;   // <- NEW

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baseScaleX = Mathf.Abs(transform.localScale.x);
    }


    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("yVelocity", rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
        }
        if (move != 0)
        {
            var s = transform.localScale;
            s.x = baseScaleX * (move < 0 ? -1 : 1);       // <- keep size, flip only direction
            transform.localScale = s;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        if(move != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
}
