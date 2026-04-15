using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 60f;

    public static event Action<int> OnHealthChanged;

    public int sceneIndex;
    public int pinsCollected = 0;

    private int _health = 3;
    public int health
    {
        get => _health;
        set
        {
            _health = value;
            OnHealthChanged?.Invoke(_health);
        }
    }

    private float verticalAngle = 0f;
    private bool movementEnabled = true;
    private PinInteractable pinInRange;
    private Shield shieldInRange;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if (!movementEnabled)
            return;

        HandleCameraRotation();
        HandleInteractions();
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalAngle -= mouseY;
        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);

        cameraTarget.position = transform.position;
        cameraTarget.rotation = Quaternion.Euler(verticalAngle, cameraTarget.eulerAngles.y + mouseX, 0f);
    }

    private void HandleInteractions()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        if (pinInRange != null && !pinInRange.IsPickedUp())
        {
            pinInRange.PickUp();
            pinsCollected++;
        }

        if (shieldInRange != null)
            shieldInRange.InsertPin();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pin"))
            pinInRange = other.GetComponent<PinInteractable>();

        if (other.CompareTag("Schild"))
            shieldInRange = other.GetComponent<Shield>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pin"))
            pinInRange = null;

        if (other.CompareTag("Schild"))
            shieldInRange = null;
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }

    public void BlockMovement()
    {
        movementEnabled = false;
    }

    public void EnableMovement()
    {
        movementEnabled = true;
    }

    public bool GetMovementState()
    {
        return movementEnabled;
    }

    public Transform GetCameraTarget()
    {
        return cameraTarget;
    }
}