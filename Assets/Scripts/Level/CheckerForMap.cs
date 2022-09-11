using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerForMap : MonoBehaviour
{

    private RoomInit _curRoom;
    private LevelGenerator _levelGenerator;

    private MapScript _mapScript;

    private GameObject roomOnMap;
    // Start is called before the first frame update
    void Start()
    {
        _curRoom = gameObject.GetComponentInParent<RoomInit>();
        _levelGenerator = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        _mapScript = GameObject.Find("MapHolder").GetComponent<MapScript>();
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            PlayerEnter();  
        }
    }

    public void PlayerEnter(){
        Debug.Log("Enter trigger " + gameObject.name);

        List<GameObject> neighbors = new List<GameObject>();
        List<int> neighborsIDs = new List<int>();

        int i =_curRoom.roomId;
        roomOnMap = _levelGenerator.GetCurrentRoomOnMap(i);
        neighbors = _levelGenerator.GetCurrentNeighborsOnMap(i);
        neighborsIDs = _levelGenerator.GetCurrentNeighborsOnMapIds(i);

        

        for(int j= 0; j < neighbors.Count; j++){
            _mapScript.AddNeighborsRoom(neighbors[j], neighborsIDs[j]);
        }

        _mapScript.AddCheckedRoom(roomOnMap, i);
    }
}
