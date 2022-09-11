using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{

    public Color colorForCurrentRoom;
    public Color colorForNeighborsUnchecked;
    public Color colorForCheckedRoom;

    public Camera mapCamera;
    public Animator mapAnimator;
    public float speedOfCamera;

    public float multSize;

    private Dictionary<int, GameObject> _checkedRoom;
    private Dictionary<int, GameObject> _unCheckedRoom;

    private Vector3 _currentPos;

    private GameObject _currentRoom;

    private bool miniMap = true;
    private float _currentSizeOfCamera;
    private float _startSizeOfCamera;

    // Start is called before the first frame update
    void Start()
    {
        _checkedRoom = new Dictionary<int, GameObject>();
        _unCheckedRoom = new Dictionary<int, GameObject>();
        _currentSizeOfCamera = mapCamera.orthographicSize;
        _startSizeOfCamera = _currentSizeOfCamera;
    }

    // Update is called once per frame
    void Update()
    {
        mapCamera.transform.position = Vector3.Lerp(mapCamera.transform.position, new Vector3(_currentPos.x, mapCamera.transform.position.y, _currentPos.z), speedOfCamera * Time.deltaTime);
        mapCamera.orthographicSize = Mathf.Lerp(mapCamera.orthographicSize, _currentSizeOfCamera, speedOfCamera * Time.deltaTime);
    }

    public void AddCheckedRoom(GameObject roomOnMap, int id){
        if(!_checkedRoom.ContainsKey(id)){
            _checkedRoom.Add(id, roomOnMap);
        }

        _currentRoom = roomOnMap;
        _currentPos = _currentRoom.transform.position;
        
        foreach (var item in _checkedRoom){
            if(item.Key != id){
                UpdateColor(item.Value, colorForCheckedRoom);
            }
            else{
                UpdateColor(item.Value, colorForCurrentRoom);
            }
        }
    }

    public void AddNeighborsRoom(GameObject roomOnMap, int id){

        if(!_unCheckedRoom.ContainsKey(id) && !_checkedRoom.ContainsKey(id)){
            _unCheckedRoom.Add(id, roomOnMap);
        }
        else if(!_unCheckedRoom.ContainsKey(id) && _checkedRoom.ContainsKey(id)){
            _unCheckedRoom.Remove(id);
        }
        else if(_unCheckedRoom.ContainsKey(id) && _checkedRoom.ContainsKey(id)){
            _unCheckedRoom.Remove(id);
        }


        foreach(var item in _unCheckedRoom){
            UpdateColor(item.Value, colorForNeighborsUnchecked);
        }
    }

    private void UpdateColor(GameObject room, Color colorToChange){

        Transform model = room.transform.Find("Model");
        Transform border = room.transform.Find("Border");
        
        for(int i = 0; i < model.childCount; i++){
            model.GetChild(i).GetComponent<MeshRenderer>().material.color = colorToChange;
        }

        Color borderColor = border.GetChild(0).GetComponent<MeshRenderer>().material.color;

        for(int i = 0; i < border.childCount; i++){
            border.GetChild(i).GetComponent<MeshRenderer>().material.color = new Color(borderColor.r, borderColor.g, borderColor.b, 1);
        }
    
    }

    public void SwitchMap(){
        miniMap = !miniMap;
        if(miniMap){
            mapAnimator.SetBool("MaxMap", false);
            _currentSizeOfCamera = _startSizeOfCamera;
        }
        else{
            mapAnimator.SetBool("MaxMap", true);
            _currentSizeOfCamera *= multSize;
        }
    }

}
