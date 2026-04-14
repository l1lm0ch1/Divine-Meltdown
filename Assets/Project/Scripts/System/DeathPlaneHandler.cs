using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPlaneHandler : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float cameraFollowSpeed = 2f;

    private bool isPlayerDead = false;
    private Transform playerTransform;
    private Vector3 initialCameraPosition;
    private Coroutine followCoroutine;

    private void Start()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerDead && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(false);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerDead = true;
        playerTransform = other.transform;
        initialCameraPosition = mainCamera.transform.position;

        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(UIFade.FadeIn(deathPanel));
        followCoroutine = StartCoroutine(FollowPlayer());
    }

    private IEnumerator FollowPlayer()
    {
        while (true)
        {
            mainCamera.transform.position = initialCameraPosition;
            mainCamera.transform.LookAt(playerTransform.position);
            yield return null;
        }
    }

    public void RestartGame()
    {
        if (followCoroutine != null)
            StopCoroutine(followCoroutine);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        if (followCoroutine != null)
            StopCoroutine(followCoroutine);

        SceneManager.LoadScene("StartUpMenu");
    }
}