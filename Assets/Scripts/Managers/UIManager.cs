using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public Image firstHeroImage;
        public TextMeshProUGUI firstHeroName;
        public TextMeshProUGUI firstHeroHealth;
        
        public void SetFirstSelectedHero(Sprite heroSprite, string heroName, string heroHealth)
        {
            firstHeroImage.sprite = heroSprite;
            firstHeroName.text = heroName;
            firstHeroHealth.text = heroHealth;
        }
    }
}