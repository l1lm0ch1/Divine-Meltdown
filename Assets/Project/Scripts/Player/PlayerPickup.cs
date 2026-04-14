using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private float pickupRange = 2f;
    [SerializeField] private Transform holdPosition;

    private GameObject objectInRange;
    private GameObject heldObject;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        if (heldObject == null)
            TryPickupObject();
        else
            DropObject();
    }

    private void TryPickupObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (Collider collider in colliders)
        {
            if (!collider.gameObject.CompareTag("Pickup"))
                continue;

            objectInRange = collider.gameObject;
            PickupObject();
            break;
        }
    }

    private void PickupObject()
    {
        if (objectInRange == null)
            return;

        heldObject = objectInRange;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        heldObject.transform.position = holdPosition.position;
        heldObject.transform.parent = holdPosition;
    }

    private void DropObject()
    {
        if (heldObject == null)
            return;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        heldObject.transform.parent = null;
        heldObject = null;
    }
}