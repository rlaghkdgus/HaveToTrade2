using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AnimalMoving : MonoBehaviour 
{
    [SerializeField] List<GameObject> Animals;
    public GameObject animalPrefab;
    public GameObject curAnimal;
    public float duration;
    public Vector3 spawntrans;
    public Data<bool> isActive = new Data<bool>();

    void Awake()
    {
        isActive.onChange += ActiveAnimal;
        isActive.onChange += OffAnimal;
    }
    private void Start()
    {
        isActive.Value = true;
    }
    private void ActiveAnimal(bool trigger)
    {
        if(trigger)
        {
            SpawnAnimal();
            Debug.Log("A");
        }
    }
    private void OffAnimal(bool trigger)
    {
        if(!trigger)
        {
            if(curAnimal != null)
            {
                Destroy(curAnimal);
                curAnimal = null;
            }
        }
    }
    public void SelectAnimal(TownDB curTown) //현재 타운에 따라 달라지는 동물
    {
        switch(curTown.TownType)
        {
            
            case VillageType.GreStar:
                animalPrefab = Animals[0];
                Debug.Log("B");
                break;
            case VillageType.GoldBen:
                animalPrefab = Animals[1];
                Debug.Log("C");
                break;
            case VillageType.Smokian:
                animalPrefab = Animals[2];
                Debug.Log("D");
                break;
                
        }
    Debug.Log("complete");
    }
    public void SpawnAnimal()
    {
        curAnimal = Instantiate(animalPrefab, spawntrans, Quaternion.identity);
    }
   
}
