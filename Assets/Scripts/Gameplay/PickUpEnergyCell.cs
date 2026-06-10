using UnityEngine;

public class EnergyCellPickup : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float hoverHeight = 0.2f;
    [SerializeField] private float hoverSpeed = 2f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem pickupEffect;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField, Range(0f, 2f)] private float pickupVolume = 1f;

    private Vector3 startPosition;
    private GameManager gameManager;
    private bool wasCollected;

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
        if (wasCollected)
        {
            return;
        }

        PlayerEnergyGlow energyGlow =
            other.GetComponentInParent<PlayerEnergyGlow>();

        if (energyGlow == null)
        {
            return;
        }

        wasCollected = true;

        if (gameManager != null)
        {
            gameManager.CollectEnergyCell();
        }

        energyGlow.PlayEnergyGlow();

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
                transform.position,
                pickupVolume
            );
        }

        Destroy(gameObject);
    }
}