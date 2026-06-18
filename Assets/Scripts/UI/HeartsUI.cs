using UnityEngine;
using UnityEngine.UI;
using ZerosAndOnes.Gameplay;

namespace ZerosAndOnes.UI
{
    /// <summary>
    /// Displays player health as a row of hearts (empty/half/full).
    /// Attach to the HeartsPanel under the gameplay Canvas.
    /// </summary>
    public class HeartsUI : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Image[] heartImages;

        [Header("Heart Sprites")]
        [SerializeField] private Sprite emptyHeartSprite;
        [SerializeField] private Sprite halfHeartSprite;
        [SerializeField] private Sprite fullHeartSprite;

        private void OnEnable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged += UpdateHearts;
                UpdateHearts(playerHealth.CurrentHealth, playerHealth.MaxHealth);
            }
        }

        private void OnDisable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged -= UpdateHearts;
            }
        }

        private void UpdateHearts(int currentHealth, int maxHealth)
        {
            for (int i = 0; i < heartImages.Length; i++)
            {
                int remainingForThisHeart = currentHealth - (i * 2);

                if (remainingForThisHeart >= 2)
                {
                    heartImages[i].sprite = fullHeartSprite;
                }
                else if (remainingForThisHeart == 1)
                {
                    heartImages[i].sprite = halfHeartSprite;
                }
                else
                {
                    heartImages[i].sprite = emptyHeartSprite;
                }
            }
        }
    }
}
