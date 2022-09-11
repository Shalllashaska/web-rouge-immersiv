using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayGeneration : MonoBehaviour
{
    public Transform doorHolder;

    public ButtonInteract button;

    private GameObject _room1;
    private GameObject _room2;

    private bool init = false;
    
    void Start()
    {
        //Choose random door prefab
        int i = Random.Range(0, doorHolder.childCount);
        GameObject door = doorHolder.GetChild(i).gameObject;
        door.SetActive(true);

        //
        button._doorScript = door.GetComponent<DoorScript>();
        
    }

    
    void Update()
    {
        
    }

    public void Initialize(GameObject room1, GameObject room2){
        _room1 = room1;
        _room2 = room2;

        button._fog1 = _room1.transform.Find("FogOfWar").GetChild(0).GetComponent<FogOfWarScript>();
        button._fog2 = _room2.transform.Find("FogOfWar").GetChild(0).GetComponent<FogOfWarScript>();
    }
}
