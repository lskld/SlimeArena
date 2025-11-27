using UnityEngine;

public class OnPlayerDeath : MonoBehaviour
{
    [SerializeField] private GameObject GameUI;
    [SerializeField] private GameObject GameOverUI;

    bool isPlayerAlive = true;

    void Start()
    {
        GameOverUI.SetActive(false);
        GameUI.SetActive(true);
    }

    public void HandlePlayerDeath()
    {
        if (isPlayerAlive)
        {
            GameUI.SetActive(false);
            GameOverUI.SetActive(true);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.SetActive(false);
        }
    }
}
