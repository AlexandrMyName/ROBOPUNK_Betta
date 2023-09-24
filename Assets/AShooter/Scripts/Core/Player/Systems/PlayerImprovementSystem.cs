using Abstracts;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using User.Presenters;
using Zenject;


namespace Core
{

    public class PlayerImprovementSystem : BaseSystem, IImprovable , IDisposable
    {

        [Inject] private TimerPool _timerPool;
        [Inject] private ImprovablePresenter _improvable;
        private IGameComponents _components;
        private ConcurrentQueue<IImprovement> _timeImprovements;
        private List<IDisposable> _disposables;
        

        public void Apply(IImprovement improvementObject)
        {
            _improvable.ApplyImprove(improvementObject);

            if (improvementObject.GetImproveTime() == ImprovementTime.Temporary)
            {
                SetTimer(improvementObject);
            }
        }


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


        protected override void Awake(IGameComponents components)
        {
            _disposables = new();
            _components = components;

            _improvable.Init(_components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Attackable,
                _components.BaseObject.GetComponent<IPlayer>().ComponentsStore.Movable);

            _timeImprovements = new ConcurrentQueue<IImprovement>();
            _components.BaseObject.GetComponent<Collider>()
                .OnTriggerEnterAsObservable()
                    .Where(x => x.GetComponent<IImprovement>() != null)
                    .Subscribe(
                        collider =>
                        {
                            collider.GetComponent<IImprovement>().Improve(this);
                        }).AddTo(_disposables);

            _components.BaseObject
                .GetComponent<MonoBehaviour>()
                .StartCoroutine(TryCansel());
        }
         

        private void SetTimer(IImprovement improvementObject)
        {
            string timerName = $"Timer of improve : {improvementObject.GetImproveType()} :" +
                $" +{improvementObject.Value} | on {improvementObject.Timer} sec.";

            ITimer timer = _timerPool.CreateCallBackTimer(timerName);
 
                _timerPool.RunTimer(
                    timer,
                    improvementObject.Timer,
                    () => { TimeDown(improvementObject); });
 
        }


        private void TimeDown(IImprovement improvementObject)
        => _timeImprovements.Enqueue(improvementObject);
 

        private IEnumerator TryCansel()
        {
            WaitForSeconds secondOnUpdate = new WaitForSeconds(0.2f);
 
            while (_components.BaseObject != null)
            {
                if (_timeImprovements.TryDequeue(out var improvement))
                {
                    _improvable.CanselImprove(improvement);
                }
                yield return secondOnUpdate;
            }
        }


    }
}