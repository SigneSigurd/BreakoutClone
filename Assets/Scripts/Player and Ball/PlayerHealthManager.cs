using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private int hp;
    [SerializeField] private GameObject heartContainer;
    [SerializeField] private GameObject heartPrefab;
    
    private GameManager gameManager;
    #endregion

    #region Methods
    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();

        InstantiateHealth();
    }

    /// <summary>
    /// Instantiates player's health on game load
    /// </summary>
    private void InstantiateHealth()
    {
        for (int i = 0; i < hp; i++)
        {
            CreateHealthObject();
        }
    }

    public void AddHealth(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CreateHealthObject();
        }

        hp += amount;
    }

    public void LooseHeath()
    {
        hp -= 1;

        if (hp >= 0)
        {
            Destroy(heartContainer.transform.GetChild(heartContainer.transform.childCount - 1).gameObject);
        }

        gameManager.CheckIfGameOver(hp);
    }

    public void CreateHealthObject()
    {
        GameObject go;
        go = Instantiate(heartPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        go.transform.SetParent(heartContainer.transform);
    }
    #endregion
}
