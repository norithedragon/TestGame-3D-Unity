using UnityEngine;

public class EnergyCellPickup : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float hoverHeight = 0.2f;
    [SerializeField] private float hoverSpeed = 2f;

    private Vector3 startPosition;
    private GameManager gameManager;

    private void Start()
    {
        startPosition = transform.position;
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Update()
    {
        RotatePickup();
        HoverPickup();
    }

    private void RotatePickup()
    {
        transform.Rotate(
            Vector3.up,
            rotationSpeed * Time.deltaTime,
            Space.World
        );
    }

    private void HoverPickup()
    {
        float verticalOffset =
            Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        transform.position =
            startPosition + Vector3.up * verticalOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (gameManager != null)
        {
            gameManager.CollectEnergyCell();
        }
        else
        {
            Debug.LogWarning(
                "GameManager nebyl ve scéně nalezen."
            );
        }

        Destroy(gameObject);
    }
}