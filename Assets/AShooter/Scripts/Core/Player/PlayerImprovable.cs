using Abstracts;
using UnityEngine;

public class PlayerImprovable : MonoBehaviour
{
    public void ApplyImprove(IImprovement improvement)
    {
        switch (improvement.GetImproveType())
        {
            case ImprovementType.Attackable:

                Debug.Log($"Attack improve - accept! |{improvement.Value}|");
                break;
            case ImprovementType.Movable:
                Debug.Log($"Speed improve - accept! |{improvement.Value}|");
                break;
        }
        
    }

    public void CanselImprove(IImprovement improvement)
    {
        switch (improvement.GetImproveType())
        {
            case ImprovementType.Attackable:

                Debug.Log($"Attack improve - canseled! |{improvement.Value}|");
                break;
            case ImprovementType.Movable:
                Debug.Log($"Speed improve - canseled! |{improvement.Value}|");
                break;
 
        }
        improvement.Dispose();
    }
}
