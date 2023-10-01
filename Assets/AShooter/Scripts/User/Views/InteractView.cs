using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AShooter.Scripts.User.Views
{
    
    public class InteractView: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}