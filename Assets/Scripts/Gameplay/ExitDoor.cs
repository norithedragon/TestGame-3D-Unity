using System.Collections;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [Header("Door Movement")]
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 4f, 0f);
    [SerializeField] private float openDuration = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField, Range(0f, 2f)] private float openVolume = 1f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen;

    private void Awake()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void OpenDoor()
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;

        PlayOpenSound();
        StartCoroutine(OpenDoorRoutine());
    }

    private void PlayOpenSound()
    {
        if (audioSource == null || openSound == null)
        {
            return;
        }

        audioSource.PlayOneShot(
            openSound,
            openVolume
        );
    }

    private IEnumerator OpenDoorRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < openDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / openDuration
            );

            transform.position = Vector3.Lerp(
                closedPosition,
                openPosition,
                progress
            );

            yield return null;
        }

        transform.position = openPosition;
    }
}