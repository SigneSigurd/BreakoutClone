using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    private float movement;
    private Vector2 screenBounds;
    private float playerHalfWidth;

    void Start()
    {
        // Gets the size of the game window
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        // Makes sure the player paddle stops moving once the edge of the sprite reaches the edge of the screen
        playerHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    void Update()
    {
        // Move the player on input
        movement = Input.GetAxis("Horizontal");
        transform.position += movement * speed * Time.deltaTime * Vector3.right;

        // Ensure player stays within screen bounds
        float clamped_x = Mathf.Clamp(transform.position.x, -screenBounds.x + playerHalfWidth, screenBounds.x - playerHalfWidth);
        Vector2 pos = transform.position;
        pos.x = clamped_x;
        transform.position = pos;
    }
}

