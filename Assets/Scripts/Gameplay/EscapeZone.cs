using UnityEngine;

public class EscapeZone : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (gameManager != null)
        {
            gameManager.WinGame();
        }
    }
}