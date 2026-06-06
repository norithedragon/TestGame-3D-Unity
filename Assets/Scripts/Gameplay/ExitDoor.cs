using System.Collections;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [Header("Door Movement")]
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 4f, 0f);
    [SerializeField] private float openDuration = 1.5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen;

    private void Awake()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    public void OpenDoor()
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;
        StartCoroutine(OpenDoorRoutine());
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