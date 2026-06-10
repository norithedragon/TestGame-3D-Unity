using UnityEngine;

public class EnergyCellPickup : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float hoverHeight = 0.2f;
    [SerializeField] private float hoverSpeed = 2f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem pickupEffect;
    [SerializeField] private AudioClip pickupSound;
    

    private Vector3 startPosition;
    private GameManager gameManager;

    private void Start()
    {
        startPosition = transform.position;
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Update()
    {
        transform.Rotate(
            Vector3.up,
            rotationSpeed * Time.deltaTime,
            Space.World
        );

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

        if (pickupEffect != null)
        {
            Instantiate(
                pickupEffect,
                transform.position,
                Quaternion.identity
            );
        }

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(
                pickupSound,
                transform.position
            );
        }

        Destroy(gameObject);
    }
}