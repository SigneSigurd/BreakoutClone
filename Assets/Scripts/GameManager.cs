using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject gameOverPrefab;
    [SerializeField] private GameObject winPrefab;
    [SerializeField] private AudioClip winSoundClip;
    [SerializeField] private AudioClip gameOverSoundClip;

    private Ball ball;
    private PlayerMovement paddle;
    private SceneChange sceneChange;
    private int remainingBricks;
    #endregion

    #region Methods
    void Start()
    {
        ball = GameObject.FindFirstObjectByType<Ball>();
        paddle = GameObject.FindFirstObjectByType<PlayerMovement>();
        sceneChange = GameObject.FindFirstObjectByType<SceneChange>();
    }

    public void CheckIfGameOver(int health)
    {
        if(health < 0)
        {
            EndGame(gameOverPrefab, gameOverSoundClip, 2);
        }
    }

    private void CheckWin()
    {
        if(remainingBricks == 0)
        {
            EndGame(winPrefab, winSoundClip, 1);
        }
    }

    private void EndGame(GameObject prefab, AudioClip audio, int playLoopTimes)
    {
        // Make sure the player can no longer play the game
        paddle.enabled = false;
        ball.enabled = false;
        ball.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;

        // Instantiate Game Over object
        GameObject go;
        Vector2 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.6f));
        go = Instantiate(prefab, spawnPoint, Quaternion.identity);

        // Get length of Game Over animation to determine delay before swithcing back to main menu
        Animator animator = go.GetComponent<Animator>();
        float animation_timer = 0f;

        foreach (AnimationClip ac in animator.runtimeAnimatorController.animationClips)
        {
            if (ac.name.ToLower().Contains("loop"))
            {
                // If loop animation, play it twice
                animation_timer += ac.length * playLoopTimes;
            }
            else
            {
                animation_timer += ac.length;
            }
        }

        AudioSource.PlayClipAtPoint(audio, new Vector3(0, 0, 0));

        StartCoroutine(BackToMenu(animation_timer - 1f));
    }

    private IEnumerator BackToMenu(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        sceneChange.ChangeScene("Menu");
    }

    /// <summary>
    /// Closes the game. Called from the "Exit" button on the menu scene
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    public void BrickDestroyed()
    {
        remainingBricks -= 1;
        CheckWin();
    }

    /// <summary>
    /// Set amount of bricks at game load
    /// </summary>
    /// <param name="totalBricks"></param>
    public void SetRemainingBricks(int totalBricks)
    {
        remainingBricks = totalBricks;
    }
    #endregion
}
