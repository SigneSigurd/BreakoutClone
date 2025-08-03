using UnityEngine;

public class Brick : MonoBehaviour
{
    #region Fields
    [SerializeField] private AudioClip breakSoundEffect;
    [SerializeField] private AudioClip hitSoundEffect;

    [SerializeField] private Sprite brick1HP;
    [SerializeField] private Sprite brick2HP;
    [SerializeField] private Sprite brick3HP;
    [SerializeField] private Sprite brick4HP;
    [SerializeField] private Sprite brick5HP;

    private GameManager gameManager;
    private ScoreManager scoreManager;
    private SpriteRenderer spriteRenderer;
    private int hp;
    private bool containsPowerUp;
    #endregion

    #region Methods
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        scoreManager = GameObject.FindFirstObjectByType<ScoreManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When colliding with the ball
        if (collision.gameObject.CompareTag("Ball"))
        {
            hp -= collision.gameObject.GetComponent<Ball>().strength;

            if (hp <= 0)
            {
                Death();
            }
            else
            {
                PlaySoundEffect(hitSoundEffect);
                ChangeSprite();
            }
        }
    }

    /// <summary>
    /// Change the brick's sprite on hp loss
    /// </summary>
    private void ChangeSprite()
    {
        switch (hp)
        {
            case 1:
                spriteRenderer.sprite = brick1HP;
                break;

            case 2:
                spriteRenderer.sprite = brick2HP;
                break;

            case 3:
                spriteRenderer.sprite = brick3HP;
                break;

            case 4:
                spriteRenderer.sprite = brick4HP;
                break;

            case 5:
                spriteRenderer.sprite = brick5HP;
                break;
        }
    }

    /// <summary>
    /// Destroy brick on death
    /// </summary>
    private void Death()
    {
        if(containsPowerUp)
        {
            // Activate the power-up
            transform.GetChild(0).gameObject.GetComponent<PowerUp>().ActivateSpawn();
        }

        PlaySoundEffect(breakSoundEffect);
        Destroy(gameObject);
        scoreManager.AddScore(1);
        gameManager.BrickDestroyed();
    }

    private void PlaySoundEffect(AudioClip soundEffect)
    {
        AudioSource.PlayClipAtPoint(soundEffect, new Vector3(0, 0, 0));
    }

    /// <summary>
    /// Sets the hp for the brick at spawn
    /// </summary>
    /// <param name="health"></param>
    public void SetHealthPoints(int hp)
    {
       this.hp = hp;
    }

    /// <summary>
    /// Set the value that determines if a brick has a power-up or not at spawn
    /// </summary>
    /// <param name="containsPowerUp">true: Has power up, false: No power-up</param>
    public void SetContainsPowerUp(bool containsPowerUp)
    {
        this.containsPowerUp = containsPowerUp;
    }
    #endregion
}
