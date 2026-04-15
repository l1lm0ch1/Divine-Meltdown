using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private CinemachinePanTilt panTilt;

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

    private float xRotation = 0f;
    private bool movementEnabled = true;
    private PinInteractable pinInRange;
    private Shield shieldInRange;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        panTilt = cinemachineCamera.GetComponent<CinemachinePanTilt>();
    }

    private void Update()
    {
        if (!movementEnabled)
            return;

        HandleMouseLook();
        HandleInteractions();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -25f, 25f);

        playerBody.Rotate(Vector3.up * mouseX);

        panTilt.TiltAxis.Value = xRotation;
        panTilt.PanAxis.Value = playerBody.eulerAngles.y;
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
}