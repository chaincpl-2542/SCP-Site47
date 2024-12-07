using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    private const float MIN_MOUSE_SPEED = 100f;
    private const float MAX_MOUSE_SPEED = 800f;
    public GameObject mainMenu;
    public GameObject pauseMenuUI; // Reference to the pause menu UI GameObject
    public GameObject settingsMenuUI; // Reference to the settings menu UI GameObject
    public Slider mouseSensitivitySlider; // Slider to adjust mouse sensitivity
    public TextMeshProUGUI sliderValueText; // Text to display the slider value
    public SimpleFirstPersonController playerController; // Reference to the player's controller script

    private bool isPaused = false;

    void Start()
    {
        // Ensure the menus are initially hidden
        mainMenu.SetActive(false);
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);

        if (mouseSensitivitySlider != null)
        {
            mouseSensitivitySlider.minValue = MIN_MOUSE_SPEED;
            mouseSensitivitySlider.maxValue = MAX_MOUSE_SPEED;
        }
    }

    void Update()
    {
        // Toggle the pause menu when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (LogManager.Instance != null && LogManager.Instance.IsLogOpen())
            {
                LogManager.Instance.CloseLog();
                return;
            }

            // Toggle the pause menu
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        mainMenu.SetActive(false);
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    public void PauseGame()
    {
        mainMenu.SetActive(true);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);

        if (mouseSensitivitySlider != null && playerController != null)
        {
            mouseSensitivitySlider.value = playerController.mouseSensitivity;
            mouseSensitivitySlider.onValueChanged.AddListener(AdjustMouseSensitivity);

            // Update the slider value text
            UpdateSliderValueText(mouseSensitivitySlider.value);
        }

        if (playerController != null && mouseSensitivitySlider != null && sliderValueText != null)
        {
            mouseSensitivitySlider.value = playerController.mouseSensitivity;
            UpdateSliderValueText(playerController.mouseSensitivity);
        }
    }

    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Resume game time before changing scenes
        SceneManager.LoadScene(0); // Replace "MainMenu" with your actual main menu scene name
    }

    private void AdjustMouseSensitivity(float newSensitivity)
    {
        if (playerController != null)
        {
            playerController.mouseSensitivity = newSensitivity;
        }

        // Update the slider value text
        UpdateSliderValueText(newSensitivity);
    }

    private void UpdateSliderValueText(float value)
    {
        if (sliderValueText != null)
        {
            sliderValueText.text = $"Sensitivity: {value:F1}"; // Show value with 1 decimal
        }
    }
}
