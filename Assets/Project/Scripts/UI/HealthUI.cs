using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private List<Image> heartImages;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    private void OnEnable()
    {
        PlayerController.OnHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        PlayerController.OnHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(int health)
    {
        for (int i = 0; i < heartImages.Count; i++)
            heartImages[i].sprite = i < health ? fullHeartSprite : emptyHeartSprite;
    }
}