using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float defaultSensitivity = 200f;
    [SerializeField] private string mainMenuSceneName = "StartUpMenu";
    [SerializeField] private string bossArenaSceneName = "BossArena";

    private bool isPaused = false;

    private void Start()
    {
        volumeSlider.value = AudioListener.volume;
        sensitivitySlider.value = defaultSensitivity;
        pauseMenuUI.SetActive(false);

        volumeSlider.onValueChanged.AddListener(SetVolume);
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetSensitivity(float sensitivity)
    {
        playerController.SetMouseSensitivity(sensitivity);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        isPaused = true;
    }

    public void SaveGame()
    {
        GameManager.Instance.SaveGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RestartBossFight()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(bossArenaSceneName);
    }
}