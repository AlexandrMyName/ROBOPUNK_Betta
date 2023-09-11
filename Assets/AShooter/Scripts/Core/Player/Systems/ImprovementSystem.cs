using Abstracts;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ImprovementSystem : BaseSystem, IImprovable
    {
        private IGameComponents _components;
        private ConcurrentQueue<IImprovement> _timeImprovements;
        private List<IDisposable> _disposables;
        [Inject] private TimerPool _timerPool;

        public void Apply(IImprovement improvementObject)
        {
            if (improvementObject.GetImproveTime() == ImprovementTime.Temporary)
            {
                string timerName = $"Timer of improve : {improvementObject.GetImproveType()} :" +
                    $" +{improvementObject.Value} | on {improvementObject.Timer} sec.";

                Debug.Log(timerName);

                ITimer timer = _timerPool.CreateCallBackTimer(timerName);
 
                Task.Factory.StartNew(() => {

                      _timerPool.RunTimer(
                          timer, 
                          improvementObject.Timer,
                          () => { TimeDown(improvementObject); } );

                });
            }
            else
            {

            }
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
                    Debug.Log($"Canseled {improvement.Value} | {improvement.GetType()}");
                }
                yield return secondOnUpdate;
            }
        }
 

        protected override void Awake(IGameComponents components)
        {
            _disposables = new();
            _components = components;
            _timeImprovements = new ConcurrentQueue<IImprovement>();
            _components.BaseObject.GetComponent<Collider>()
                .OnTriggerEnterAsObservable()
                    .Where(x => x.GetComponent<IImprovement>() != null)
                    .Subscribe(
                        collider =>
                        {
                            collider.GetComponent<IImprovement>().Improve(this);
                        }).AddTo(_disposables);
            _components.BaseObject.GetComponent<MonoBehaviour>().StartCoroutine(TryCansel());
        }


        protected override void OnDestroy()
        => _disposables.ForEach(disposable => disposable.Dispose());
         

    }
}