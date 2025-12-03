using UnityEngine;

public class SimpleEnemyPatrol : MonoBehaviour
{
    public float speed = 1.8f;
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
       
        if (!enabled) return;

        Vector2 target = movingToB ? bPos : aPos;
        Vector2 next = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        if (sr) sr.flipX = (target.x - rb.position.x) > 0f;

        if (Vector2.Distance(rb.position, target) <= 0.05f)
            movingToB = !movingToB;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            movingToB = !movingToB;
            StartCoroutine(PauseAfterHit());
        }
    }

    private System.Collections.IEnumerator PauseAfterHit()
    {
        float oldSpeed = speed;
        speed = 0f;
        yield return new WaitForSeconds(0.5f);
        speed = oldSpeed;
    }
}
