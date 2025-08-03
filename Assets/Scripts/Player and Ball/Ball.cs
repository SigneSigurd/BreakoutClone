using UnityEngine;

public class Ball : MonoBehaviour
{
    #region Fields
    [SerializeField] private float ballStartPosition;
    [SerializeField] private Vector2 startVelocity;

    [SerializeField] private AudioClip paddleCollideSoundEffect;
    [SerializeField] private AudioClip wallCollideSoundEffect;
    [SerializeField] private AudioClip damageSoundEffect;

    public int strength; // Amount of damage the ball applies to bricks ( used in Brick.cs )

    private bool ballInPlay = false;
    private Rigidbody2D ball;

    private Transform paddle;
    private PlayerHealthManager playerHealthManager;
    #endregion

    #region Methods

    void Start()
    {
        ball = GetComponent<Rigidbody2D>();

        paddle = GameObject.FindFirstObjectByType<PlayerMovement>().transform;
        playerHealthManager = GameObject.FindFirstObjectByType<PlayerHealthManager>();
    }

    void Update()
    {
        // If ball not in play, set start position
        if(!ballInPlay)
        {
            transform.position = paddle.position + (Vector3.up * ballStartPosition);

            // When shoot button pressed, shoot ball
            if (Input.GetKeyDown("space"))
            {
                // Play sound effect on start
                PlaySoundEffect(paddleCollideSoundEffect);

                ballInPlay = true;
                ball.AddForce(startVelocity, ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When colliding with the player
        if(collision.gameObject.CompareTag("Player"))
        {
            // Play sound effect on hit
            PlaySoundEffect(paddleCollideSoundEffect);

            // Get half width of player collider
            float halfWitdh = collision.collider.bounds.size.x;
            // Calculate which side of the player we collide with (positive = right, negative = left)
            // Use half width to normalize output
            float x = (transform.position.x - collision.transform.position.x) / halfWitdh;

            // Set and normalize new direction
            Vector2 direction = new(4 * x, 1);
            direction = direction.normalized;

            // Set speed to current speed
            float currentSpeed = ball.linearVelocity.magnitude;

            // Apply new direction
            ball.linearVelocity = direction * currentSpeed;
        }

        if(collision.gameObject.CompareTag("Wall"))
        {
            PlaySoundEffect(wallCollideSoundEffect);
        }

        // When colliding with the bottom of the level
        if (collision.gameObject.CompareTag("Death"))
        {
            PlaySoundEffect(damageSoundEffect);
            Death();
        }
    }

    private void PlaySoundEffect(AudioClip soundEffect)
    {
        AudioSource.PlayClipAtPoint(soundEffect, new Vector3(0, 0, 0));
    }

    private void Death()
    {
        // If the ball hits the bottom (death line), reset ball
        transform.position = paddle.position + (Vector3.up * ballStartPosition);
        ball.linearVelocity = Vector2.zero; // Reset velocity.. otherwise it keeps adding speed
        ballInPlay = false;

        playerHealthManager.LooseHeath();
    }
    #endregion
}
