using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class SimpleGenerator : MonoBehaviour
{
    [SerializeField, Range(0.1f,0.5f)] private float _fillPercent;

    [SerializeField] private Transform _planeMax;
    [SerializeField] private Transform _planeMin;

    [SerializeField] private GameObject _cubeObstacle;
    [SerializeField] private GameObject _plane;

    private void Start()
    {
       
        int zEntry = Mathf.FloorToInt( _planeMin.position.z);
        int zMax = Mathf.FloorToInt(_planeMax.position.z);

        int xEntry = Mathf.FloorToInt(_planeMin.position.x);
        int xMax = Mathf.FloorToInt(_planeMax.position.x);


        int width = xEntry + xMax;
        int length = zEntry + zMax;

         
 
        for (int x = xEntry; x < xMax;)
        {
            
            for (int z = zEntry; z < zMax;)
            {
                    if (Random.Range(0f, 10f) < _fillPercent)
                        GameObject.Instantiate(_cubeObstacle,
                            new Vector3(x ,  1, z),
                                Quaternion.identity,this.transform);
                     
                z += Mathf.FloorToInt(_cubeObstacle.transform.localScale.z);
            }
            x += Mathf.FloorToInt(_cubeObstacle.transform.localScale.x);
        }

        _plane.GetComponent<NavMeshSurface>().BuildNavMesh();

        
    }
}
