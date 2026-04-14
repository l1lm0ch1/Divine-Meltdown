using UnityEngine;

public class PinInteractable : MonoBehaviour
{
    private bool isPickedUp = false;
    private bool playerInRange = false;

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

    private void Update()
    {
        if (!isPickedUp && playerInRange && Input.GetKeyDown(KeyCode.E))
            PickUp();
    }

    public void PickUp()
    {
        isPickedUp = true;
        gameObject.SetActive(false);
        UIManager.Instance.UpdatePinCount();

        Shield nearestShield = FindNearestShield();
        if (nearestShield != null)
            nearestShield.AddPin(this);
    }

    private Shield FindNearestShield()
    {
        Shield[] shields = FindObjectsByType<Shield>(FindObjectsSortMode.None);
        Shield nearestShield = null;
        float minDistance = float.MaxValue;

        foreach (Shield shield in shields)
        {
            float distance = Vector3.Distance(transform.position, shield.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestShield = shield;
            }
        }

        return nearestShield;
    }

    public bool IsPickedUp()
    {
        return isPickedUp;
    }
}