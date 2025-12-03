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
    [SerializeField] private Rigidbody2D rb;
    bool grounded;
    float move;
    public GameObject slashHitbox;

    // ⭐ QUICKSAND VARIABLES ⭐
    private bool inQuickSand = false;
    public float quickSandSpeedMultiplier = 0.05f;

    // ⭐ ATTACK SOUND VARIABLE ⭐
    [Header("Audio")]
    public AudioClip attackSound;

    float baseScaleX;

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

        // ⭐ ATTACK INPUT ⭐
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("attack");

            // Play the sound effect if AudioManager exists
            if (AudioManager.instance != null && attackSound != null)
            {
                AudioManager.instance.PlaySFX(attackSound);
            }
        }

        if (move != 0)
        {
            var s = transform.localScale;
            s.x = baseScaleX * (move < 0 ? -1 : 1);
            transform.localScale = s;
        }
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        animator.SetBool("isRunning", move != 0);

        // ⭐ 1. CHECK SLASH FIRST (critical fix)
        bool isSlashing = animator.GetCurrentAnimatorStateInfo(0).IsName("slash");
        if (isSlashing)
        {
            // keep vertical velocity, freeze horizontal
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // ⭐ 2. APPLY QUICKSAND MOVEMENT REDUCTION
        float currentSpeed = moveSpeed;
        if (inQuickSand)
            currentSpeed *= quickSandSpeedMultiplier;

        // ⭐ 3. FINALLY APPLY MOVEMENT
        rb.linearVelocity = new Vector2(move * currentSpeed, rb.linearVelocity.y);
    }


    // Slash hitbox triggers
    public void EnableHitbox()
    {
        slashHitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        slashHitbox.SetActive(false);
    }

    // ⭐ DETECT ENTERING QUICKSAND ⭐
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Quicksand"))
        {
            inQuickSand = true;

            // ⭐ Make Sand Wraith appear
            SandWraithAppear wraith = FindFirstObjectByType<SandWraithAppear>();
            if (wraith != null)
                wraith.Appear();
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Quicksand"))
        {
            inQuickSand = true;

            // ⭐ Make Sand Wraith appear
            SandWraithAppear wraith = FindFirstObjectByType<SandWraithAppear>();
            if (wraith != null)
                wraith.Appear();
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Quicksand"))
            inQuickSand = false;
    }

}