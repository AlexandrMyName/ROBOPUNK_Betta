using Abstracts;
using System;
using UnityEngine;

public class CallBackTimer : MonoBehaviour, ITimer 
{
    private Action _onCompleted;
    private float _currentTime = 0f;
    private float _timeMax;
    private bool isActivate;

    public void AwaiteFor(float timeMax , Action onCompleted)
    {
        _onCompleted = onCompleted;
        _timeMax = timeMax; 
        isActivate = true;
    }

    private void Update()
    {
        if (isActivate)
        {
            _currentTime += Time.deltaTime;

            if(_currentTime >= _timeMax)
            {
                _onCompleted?.Invoke();
                Destroy(gameObject);
            }
        }
    }


}
