using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{

    public class SeekerOfEnemies  
    {
         
        public List<IEnemy> Enemies = new();


        public void FoundOverlap(Collider[] colliders )
        { 
            
                for (int i = 0; i < colliders.Length; i++)
                {
                    var obj = colliders[i];
               
                    CustomDispatcher.Instance.Invoke(() =>
                    {
                       
                        if (obj.TryGetComponent<IEnemy>(out var enemy))
                        {
                            Debug.LogWarning(obj.gameObject.name);
                            Enemies.Add(enemy);
                        }
                        else
                        {
                             
                            Vector3 objFirst = Vector3.one;
                            GameObject parent = obj.gameObject;

                            while(objFirst != Vector3.zero)
                            {
                                if (parent.transform.parent != null)
                                {
                                 
                                    if (parent.TryGetComponent<IEnemy>(out var enemyFirst))
                                    {
                                        Debug.LogWarning("YES");
                                        Enemies.Add(enemyFirst);
                                    }
                                    else parent = parent.transform.parent.gameObject;

                                }
                                else
                                {
                                    objFirst = Vector3.zero;
                                }
                            }

                            
                        }
                    });
              
            }
            
           

        }

    }
}

public interface IDispatcher
{
    void Invoke(Action fn);
}

public class CustomDispatcher : IDispatcher
{

    public List<Action> pending = new List<Action>();
    private static CustomDispatcher instance;

    public void Invoke(Action fn)
    {
        lock (pending)
        {
            pending.Add(fn);
        }
    }


    public void InvokePending()
    {
        lock (pending)
        {
            foreach (var action in pending)
            {
                action();
            }
            pending.Clear();
        }
    }
    public static CustomDispatcher Instance
    {
        get
        {
            if (instance == null)
            {

                instance = new CustomDispatcher();
            }
            return instance;
        }
    }
}