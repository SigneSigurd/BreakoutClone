using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    #region Fields
    [SerializeField] private string levelName;
    [SerializeField] private GameObject uiBar;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject brickContainer;
    [SerializeField] private float spaceBetweenBricks;
    [SerializeField] private float spaceFromBottomScreen;

    private GameManager gameManager;
    private PowerUpManager powerUpManager;
    #endregion

    #region Methods
    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
        powerUpManager = GameObject.FindFirstObjectByType<PowerUpManager>();

        GenerateLevel();
    }

    /// <summary>
    /// Generates the level (bricks)
    /// </summary>
    public void GenerateLevel()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("LevelFiles/" + levelName);

        // Windows uses \r for line endings... this means each line before the last line has hidden characters if we don't do this
        // (which makes the brick end up with the default color)
        string[] level = csvFile.text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        Vector3 brickSize = brickPrefab.GetComponent<SpriteRenderer>().bounds.size;
        float uiBarHeight = uiBar.GetComponent<SpriteRenderer>().bounds.size.y;

        // Get camera bounds in world space
        Camera camera = Camera.main;
        Vector3 bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.nearClipPlane));

        // Define the minimum Y position (bottom boundary)
        float minYPosition = bottomLeft.y + spaceFromBottomScreen;

        // Calculate total height needed for all brick rows (For scalling height)
        float totalBricksHeight = level.Length * brickSize.y;
        float availableHeight = topRight.y - uiBarHeight - minYPosition;

        // 1 by default to keep normal scale if no scalling is needed
        float heightScale = 1f;

        // Check if further height scalling is necessary, if so, add new brick scale
        if (totalBricksHeight > availableHeight)
        {
            heightScale = availableHeight / totalBricksHeight;
        }

        float scaledBrickHeight = brickSize.y * heightScale;

        float levelWidth = topRight.x - bottomLeft.x;

        //Set start positions
        float PositionY = topRight.y - uiBarHeight - scaledBrickHeight / 2;
        float startX = bottomLeft.x;

        // Used by GameManager to determine if the player has won the game
        int bricksSpawned = 0;

        // GENERATE BRICKS
        foreach (string s in level)
        {
            string[] splitData = s.Split(",");
            int bricksPerRow = splitData.Length;

            // Calculate the width each brick should be to fill the entire screen width
            float scaledBrickWidth = levelWidth / bricksPerRow;

            for (int i = 0; i < splitData.Length; i++)
            {
                // Trim whitespace (Windows tend to leave line endings... remove those here!)
                string cleanValue = splitData[i].Trim();

                if (cleanValue != "0" && cleanValue != "")
                {
                    GameObject brick = Instantiate(brickPrefab, transform);

                    // Scale the brick
                    Vector3 brickScale = brick.transform.localScale;
                    brickScale.x = (scaledBrickWidth / brickSize.x) * brickScale.x - spaceBetweenBricks;
                    brickScale.y = heightScale * brickScale.y - spaceBetweenBricks;
                    brick.transform.localScale = brickScale;

                    // Position bricks
                    float positionX = startX + (i + 0.5f) * scaledBrickWidth - spaceBetweenBricks;
                    brick.transform.position = new Vector3(positionX, PositionY, 0);
                    brick.transform.SetParent(brickContainer.transform);

                    // Set brick color
                    brick.GetComponent<SpriteRenderer>().color = GetBrickColor(cleanValue.ToLower());

                    // Set brick hp, power-up "status" and instantiate power-up for power-up bricks
                    if (cleanValue.ToLower() == "x")
                    {
                        PowerUpData data = powerUpManager.GetRandomPowerUp();
                        AttachPowerUp(brick, data);
                        brick.GetComponent<Brick>().SetContainsPowerUp(true);
                        brick.GetComponent<Brick>().SetHealthPoints(data.brickHp);
                    }
                    // Set brick hp and power-up "status" for regular bricks
                    else
                    {
                        brick.GetComponent<Brick>().SetHealthPoints(int.Parse(cleanValue));
                        brick.GetComponent<Brick>().SetContainsPowerUp(false);
                    }

                    bricksSpawned += 1;
                }
            }

            PositionY -= scaledBrickHeight + spaceBetweenBricks; // Move down for next row using scaled height
        }

        gameManager.SetRemainingBricks(bricksSpawned);
    }

    /// <summary>
    /// Changes the color of the brick depending on the value from the CSV-file (AKA the brick's hp)
    /// </summary>
    /// <param name="brickType">Value from CSV-file</param>
    /// <returns></returns>
    private Color GetBrickColor(string brickType)
    {
        Color color;

        switch (brickType)
        {
            case "1":
                color = new Color32(137, 251, 97, 255); // green
                break;

            case "2":
                color = new Color32(108, 234, 213, 255); // blue
                break;

            case "3":
                color = new Color32(166, 92, 237, 255); // purple
                break;

            case "4":
                color = new Color32(248, 218, 79, 255); // yellow
                break;

            case "5":
                color = new Color32(238, 148, 63, 255); // orange
                break;

            case "6":
                color = new Color32(222, 58, 56, 255); // red
                break;

            case "x":
                color = new Color32(225, 106, 178, 255); // light pink
                break;

            default:
                color = new Color32(0, 0, 0, 255); // black
                break;
        }

        return color;
    }

    private void AttachPowerUp(GameObject brick, PowerUpData powerUp)
    {
        GameObject powerUp_go = Instantiate(powerUp.prefab, brick.transform);

        PowerUp pu = powerUp_go.GetComponent<PowerUp>();
        pu.SetPowerUpData(powerUp);
    }
    #endregion
}
