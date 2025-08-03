using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    #region Fields
    [SerializeField] public List<PowerUpData> allPowerUps;
    [SerializeField] private GameObject ballGameObject;

    private Ball ball;
    private SpriteRenderer ballSpriteRenderer;

    private PlayerHealthManager playerHealthManager;

    private bool strengthBuffInUse;
    #endregion

    #region Methods
    void Start()
    {
        ball = ballGameObject.GetComponent<Ball>();
        ballSpriteRenderer = ballGameObject.GetComponent<SpriteRenderer>();

        playerHealthManager = GameObject.FindFirstObjectByType<PlayerHealthManager>();

        strengthBuffInUse = false;
    }

    public void ActivatePowerUp(PowerUpData powerUp)
    {
        switch (powerUp.type.ToLower())
        {
            case "strength":
                StartCoroutine(Strength(powerUp.duration, powerUp.effectAmount));
                break;
            case "addhealth":
                AddPlayerHealth(powerUp.effectAmount);
                break;
        }
    }

    /// <summary>
    /// Gets a random power-up from the list of power-ups
    /// </summary>
    /// <returns></returns>
    public PowerUpData GetRandomPowerUp()
    {
        int random = Random.Range(0, allPowerUps.Count);

        return allPowerUps[random];
    }

    #region Power-up methods
    /// <summary>
    /// Strength power-up
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator Strength(float duration, int effectAmount)
    {
        // If already in use, wait..
        if(strengthBuffInUse)
        {
            yield return new WaitUntil(() => strengthBuffInUse == false);
        }

        strengthBuffInUse = true;
        int oldStrength = ball.strength;
        Color oldColor = ballSpriteRenderer.color;

        ball.strength = effectAmount;
        ballSpriteRenderer.color = new Color32(190, 14, 14, 255); // red

        yield return new WaitForSeconds(duration);

        ball.strength = oldStrength;
        ballSpriteRenderer.color = oldColor;
        strengthBuffInUse = false;
    }

    /// <summary>
    /// Add hp to player
    /// </summary>
    /// <param name="amount">Amount of hp to add</param>
    public void AddPlayerHealth(int amount)
    {
        playerHealthManager.AddHealth(amount);
    }
    #endregion
    #endregion
}