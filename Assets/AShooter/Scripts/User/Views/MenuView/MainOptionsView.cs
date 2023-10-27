using Abstracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;


public class MainOptionsView : MonoBehaviour, IOptionsView
{

    [SerializeField] private GameObject _menuViewObject;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    public Button _onBack;

    [SerializeField] AudioMixer _mixer;

    private void Awake()
    {
        _onBack.onClick.AddListener(BackToMenu);

        if (PlayerPrefs.HasKey("Music"))
        {
            _soundSlider.value = PlayerPrefs.GetFloat("Music");

            _musicSlider.value = PlayerPrefs.GetFloat("Sound");

            _mixer.SetFloat("Music", _musicSlider.value);
            _mixer.SetFloat("Sound", _soundSlider.value);
        }
        else
        {
            _mixer.GetFloat("Music", out var musValue);
            _mixer.GetFloat("Sound", out var soundValue);

            _soundSlider.value = soundValue;
            _musicSlider.value = musValue;
        }
        
        
    }


    private void BackToMenu()
    {

        if (_menuViewObject == null) return;
        _menuViewObject.SetActive(true);
        gameObject.SetActive(false);

        PlayerPrefs.SetFloat("Music", _musicSlider.value);
        PlayerPrefs.SetFloat("Sound", _soundSlider.value);
        
    }


    private void Update()
    {
       
        _mixer.SetFloat("Music",_musicSlider.value);
        _mixer.SetFloat("Sound", _soundSlider.value);
    }


    
    private void OnDestroy()
    {
        
        _onBack.onClick.RemoveAllListeners();
        PlayerPrefs.Save();
    }

    public void Show()
    {

        
        gameObject.SetActive(true);
    }

    public void SubscribeButtonBack(UnityAction onBack)
    {
        _onBack.onClick.AddListener(onBack);
    }
    public void Hide()
    {
        if (!this) return;
        gameObject.SetActive(false);
    }

    public bool GetActivityState()
    {
        if(!this) return false;

        return gameObject.activeSelf;
    }
}
