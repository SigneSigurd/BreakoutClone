using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float fallSpeed;
    [SerializeField] private AudioClip pickingUpPowerUpSoundEffect;

    private PowerUpManager powerUpManager;

    private PowerUpData powerUpData;
    private Rigidbody2D powerUpBody;

    void Start()
    {
        powerUpBody = GetComponent<Rigidbody2D>();
        powerUpManager = GameObject.FindFirstObjectByType<PowerUpManager>();
    }

    /// <summary>
    /// Remove the power-up from the parent (to avoid destruction)
    /// and make it fall towards the bottom of the screen
    /// </summary>
    public void ActivateSpawn()
    {
        transform.parent = null;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        powerUpBody.AddForce(new Vector2(0, -fallSpeed), ForceMode2D.Impulse);
    }

    /// <summary>
    /// Set up the needed data for the power-up
    /// </summary>
    /// <param name="powerUpData"></param>
    public void SetPowerUpData(PowerUpData powerUpData)
    {
        this.powerUpData = powerUpData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(pickingUpPowerUpSoundEffect, new Vector3(0, 0, 0));
            powerUpManager.ActivatePowerUp(powerUpData);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            Destroy(gameObject);
        }
    }
}
