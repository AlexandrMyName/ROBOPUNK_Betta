using Abstracts;
using TMPro;
using UnityEngine;


namespace User
{

    public class MP3PlayerView : MonoBehaviour, IMP3PlayerView
    {
        [SerializeField] private TMP_Text _textUI;

        public bool Ticker {  get; set; }

        float _currentPositionX;
        float _defaultPositionX;
        float _speed = 20;


        private void Awake()
        {
            _textUI.text = "Press P to play...";
            _defaultPositionX = _textUI.transform.position.x;
            _currentPositionX = _defaultPositionX;
            Ticker = false;
        }


        void Update()
        {
            if (Ticker)
            {
                _currentPositionX += Time.deltaTime * _speed;
                _textUI.transform.position = new Vector3(_currentPositionX, transform.position.y, transform.position.z);

                if (_currentPositionX > 1000)
                    _currentPositionX = _defaultPositionX;
            }
            else
            {
                _textUI.transform.position = new Vector3(_defaultPositionX, transform.position.y, transform.position.z);
                _currentPositionX = _defaultPositionX;
            }
        }


        public void ChangeText(string text)
        {
            _textUI.text = text;
        }


        public void Show() => gameObject.SetActive(true);


        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public bool GetActivityState()
        {
            return gameObject.activeSelf;
        }


    }
}
