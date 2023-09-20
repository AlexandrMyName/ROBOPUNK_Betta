using Abstracts;
using UnityEngine;
using User;


public class Chest : MonoBehaviour , IChest
{
    [SerializeField] private ChestDataConfig chestConfig;




    public object GetRandomItem()
    {
        int index = Random.Range(0, chestConfig.COUNT_POSSIBLE_OBJECTS - 1);

        GetComponent<SphereCollider>().enabled = false;

        switch (index)
        {
            case 0:
                if (chestConfig.WeaponsPossibleGeneration == null) return null;
                return chestConfig.WeaponsPossibleGeneration[Random.Range(0, chestConfig.WeaponsPossibleGeneration.Count - 1)];
                
            case 1:
                if (chestConfig.ImprovableItemsPossibleGeneration == null) return null;
                return chestConfig.ImprovableItemsPossibleGeneration[Random.Range(0, chestConfig.WeaponsPossibleGeneration.Count - 1)];

            case 2:
                if (chestConfig.MettaCoinsPossibleGeneration == null) return null;
                return chestConfig.MettaCoinsPossibleGeneration[Random.Range(0, chestConfig.WeaponsPossibleGeneration.Count - 1)];

            case 3:
                if (chestConfig.HealthPossibleGeneration == null) return null;
                return chestConfig.HealthPossibleGeneration[Random.Range(0, chestConfig.WeaponsPossibleGeneration.Count - 1)];

            default: 
                
                return null;
        }
    }

    
}
