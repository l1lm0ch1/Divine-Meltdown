using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private int collectedPinCount = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdatePinCount()
    {
        collectedPinCount++;
    }

    public void UpdateInsertedPinCount(int insertedCount)
    {
    }
}