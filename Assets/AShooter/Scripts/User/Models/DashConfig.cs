using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(DashConfig), menuName = "Config/" + nameof(DashConfig), order = 0)]
    public class DashConfig : ScriptableObject
    {

        [Range(.5f, 2f)] public float ShieldTime;
        [Range(2f, 5f)]  public float RegenerationTime;
        [Range(25f, 100f)] public float Force;
 
    }
}
