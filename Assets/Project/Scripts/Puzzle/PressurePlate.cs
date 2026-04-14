using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public static event System.Action OnButtonPushed;

    public bool isPushedDown = false;

    private Vector3 originalPosition;
    private bool playerInRange = false;

    [SerializeField] private float moveDistance = 0.05f;
    [SerializeField] private float moveDuration = 0.5f;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (!isPushedDown && playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PushDown();
            OnButtonPushed?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    public void PushDown()
    {
        isPushedDown = true;
        StartCoroutine(MoveDown());
    }

    public void Reset()
    {
        if (!isPushedDown)
            return;

        isPushedDown = false;
        StartCoroutine(MoveUp());
    }

    private IEnumerator MoveDown()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, -moveDistance, 0);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }

    private IEnumerator MoveUp()
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }
}