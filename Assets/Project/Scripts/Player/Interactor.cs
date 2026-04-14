using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactorSource;
    [SerializeField] private float interactRange = 2f;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        Ray ray = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactable))
                interactable.Interact();
        }
    }
}