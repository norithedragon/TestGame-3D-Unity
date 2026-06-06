using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Energy Cells")]
    [SerializeField] private int requiredEnergyCells = 3;

    [Header("UI")]
    [SerializeField] private TMP_Text energyCellText;
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Exit")]
    [SerializeField] private ExitDoor exitDoor;

    private int collectedEnergyCells;
    private bool allEnergyCellsCollected;
    private bool gameFinished;

    private void Start()
    {
        Time.timeScale = 1f;

        collectedEnergyCells = 0;
        allEnergyCellsCollected = false;
        gameFinished = false;

        UpdateEnergyCellUI();
        UpdateObjectiveText("Collect all Energy Cells");

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }
    }

    public void CollectEnergyCell()
    {
        if (gameFinished || allEnergyCellsCollected)
        {
            return;
        }

        collectedEnergyCells++;
        UpdateEnergyCellUI();

        if (collectedEnergyCells >= requiredEnergyCells)
        {
            AllEnergyCellsCollected();
        }
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText == null)
        {
            return;
        }

        healthText.text = $"Health: {currentHealth} / {maxHealth}";
    }

    private void UpdateEnergyCellUI()
    {
        if (energyCellText == null)
        {
            return;
        }

        energyCellText.text =
            $"Energy Cells: {collectedEnergyCells} / {requiredEnergyCells}";
    }

    private void UpdateObjectiveText(string newObjective)
    {
        if (objectiveText == null)
        {
            return;
        }

        objectiveText.text = newObjective;
    }

    private void AllEnergyCellsCollected()
    {
        allEnergyCellsCollected = true;

        UpdateObjectiveText("Reach the escape zone");

        if (exitDoor != null)
        {
            exitDoor.OpenDoor();
        }
    }

    public void WinGame()
    {
        if (gameFinished || !allEnergyCellsCollected)
        {
            return;
        }

        gameFinished = true;

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        EndGame();
    }

    public void LoseGame()
    {
        if (gameFinished)
        {
            return;
        }

        gameFinished = true;

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }

        EndGame();
    }

    private void EndGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
}