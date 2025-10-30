using UnityEngine;

public class SimpleEnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public Transform pointA, pointB;
    private Vector2 aPos, bPos;
    private bool movingToB = true;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        aPos = pointA.position;
        bPos = pointB.position;
    }

    void FixedUpdate()
    {
        Vector2 target = movingToB ? bPos : aPos;
        Vector2 next = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        // flip toward movement
        if (sr) sr.flipX = (target.x - rb.position.x) > 0f;

        // reached target → reverse
        if (Vector2.Distance(rb.position, target) <= 0.05f)
            movingToB = !movingToB;
    }

    // turn and pause briefly when colliding with player
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            movingToB = !movingToB;  // reverse direction
            StartCoroutine(PauseAfterHit());
        }
    }

    private System.Collections.IEnumerator PauseAfterHit()
    {
        float oldSpeed = speed;
        speed = 0f;
        yield return new WaitForSeconds(0.5f);  // short pause so player can move away
        speed = oldSpeed;
    }
}
