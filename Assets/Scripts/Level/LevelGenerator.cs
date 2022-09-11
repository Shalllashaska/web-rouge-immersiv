using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    class Room{
        public GameObject currentRoom;
        public GameObject currentRoomOnMap;
        List<Room> roomNeighbors = new List<Room>();

        public int roomId;

        public Room(GameObject curRoom, int id){
            this.currentRoom = curRoom;
            this.roomId = id;
        }

        public void AddNeighbor(Room neighRoom){
            this.roomNeighbors.Add(neighRoom);
        }

        public List<Room> GetNeighbors(){
            return this.roomNeighbors;
        }

        public void AddRoomOnMap(GameObject rm){
            currentRoomOnMap = rm;
        }

    }

    public GameObject[] roomPrefabs;
    [Range(0, 50)]
    public int roomMount = 5;
    public float timer = 2;
    public GameObject startRoom;
    public GameObject doorPrefab;
    public Transform mapHolder;
    public LayerMask roomLayer;




    private List<Room> _spawnedRooms;
    private List<Transform> _spawnPoints;
    private List<Transform> _spawnPointsForDoors;
    private List<Transform> _deletedPoints;
    private int _currentCountOfRooms;
    private float _currentTimer;
    private bool _doorsSpawned = false;


    // Start is called before the first frame update
    void Start()
    {
        _currentCountOfRooms = 1;
        _spawnPointsForDoors = new List<Transform>();
        _spawnedRooms = new List<Room>();
        _spawnPoints = new List<Transform>();
        _deletedPoints = new List<Transform>();

        Room newRoom = new Room(startRoom, _currentCountOfRooms);
        
        startRoom.name += "number " + _currentCountOfRooms;
        startRoom.GetComponent<RoomInit>().roomId = _currentCountOfRooms;

        _spawnedRooms.Add(newRoom);
        

        GameObject mapRoom = startRoom.transform.Find("MapRoom").gameObject;
        mapRoom.SetActive(true);

        GameObject rm = GameObject.Instantiate(mapRoom, mapHolder.position, mapHolder.rotation, startRoom.transform);
        _spawnedRooms[_currentCountOfRooms - 1].AddRoomOnMap(rm);
        mapRoom.SetActive(false);
    }

    void Update()
    {
        if (_currentCountOfRooms >= roomMount)
        {
            if (!_doorsSpawned)
            {
                SpawnDoors();
                FindPointsForAdditionalDoors();
                _doorsSpawned = true;
                startRoom.transform.Find("CheckerForMap").GetComponent<CheckerForMap>().PlayerEnter();
            }
            return;
        }

        if (_currentTimer <= 0)
        {
            SpawnRoom();
        }
        else
        {
            _currentTimer -= Time.deltaTime;
        }
    }

    private void SpawnRoom()
    {
        GameObject roomForSpawn =
            roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        GameObject checker = roomForSpawn.transform.Find("Checker").gameObject;

        Transform spawnPoint =
            _spawnPoints[Random.Range(0, _spawnPoints.Count)];

        if (CanSpawnRoom(spawnPoint, checker))
        {
            GameObject room =
                GameObject
                    .Instantiate(roomForSpawn,
                    spawnPoint.position,
                    spawnPoint.rotation,
                    spawnPoint.parent.parent);

            
            _currentCountOfRooms++;

            Room newRoom = new Room(room, _currentCountOfRooms);
            room.name += "number " + _currentCountOfRooms;
            room.GetComponent<RoomInit>().roomId = _currentCountOfRooms;
            _spawnedRooms.Add (newRoom);

            _spawnPointsForDoors.Add (spawnPoint);

            GameObject mapRoom = room.transform.Find("MapRoom").gameObject;

            

            SpawnRoomForMap(mapRoom, spawnPoint);
            mapRoom.SetActive(false);
        }

        CheckAllSpawnPoints();
        _currentTimer = timer;
    }

    private void SpawnRoomForMap(GameObject mapRoom, Transform spawnPoint){

        GameObject room = GameObject.Instantiate(mapRoom, spawnPoint.position + mapHolder.position, spawnPoint.rotation, spawnPoint.parent.parent);
        _spawnedRooms[_currentCountOfRooms - 1].AddRoomOnMap(room);
    }

    private bool CanSpawnRoom(Transform spawnPoint, GameObject checker)
    {
        Transform checkHandler =
            GameObject
                .Instantiate(checker, spawnPoint.position, spawnPoint.rotation)
                .transform;
        Transform check = checkHandler.GetChild(0);

        Collider[] colls =
            Physics
                .OverlapBox(check.position,
                check.localScale / 2,
                spawnPoint.rotation,
                roomLayer);

        Destroy(checkHandler.gameObject);
        if (colls.Length > 0)
        {
            return false;
        }

        return true;
    }

    private void CheckAllSpawnPoints()
    {
        List<Transform> listToDelete = new List<Transform>();

        foreach (Transform point in _spawnPoints)
        {
            if (
                Physics
                    .Raycast(point.position - point.forward * 0.1f,
                    point.forward,
                    0.3f,
                    roomLayer)
            )
            {
                listToDelete.Add (point);
                _deletedPoints.Add (point);
            }
        }

        foreach (Transform point in listToDelete)
        {
            _spawnPoints.Remove (point);
        }
    }

    private void FindPointsForAdditionalDoors()
    {
        List<Transform> posToPlaceDoors = new List<Transform>();
        List<Vector3> posToPlaceVecDoors = new List<Vector3>();

        foreach (Transform point in _spawnPointsForDoors)
        {
            _deletedPoints.Remove (point);
        }
        for (int i = 0; i < _deletedPoints.Count; i++)
        {
            Vector3 posA =
                new Vector3(Mathf.Round(_deletedPoints[i].position.x),
                    Mathf.Round(_deletedPoints[i].position.y),
                    Mathf.Round(_deletedPoints[i].position.z));

            bool findDoor = false;

            for (int j = 0; j < _deletedPoints.Count; j++)
            {
                if (i == j) continue;
                if(findDoor) break;

                Vector3 posB =
                    new Vector3(Mathf.Round(_deletedPoints[j].position.x),
                        Mathf.Round(_deletedPoints[j].position.y),
                        Mathf.Round(_deletedPoints[j].position.z));

                if (posA == posB)
                {
                    if(!posToPlaceVecDoors.Contains(posA)){
                        findDoor = true;
                        posToPlaceVecDoors.Add(posA);
                        posToPlaceDoors.Add(_deletedPoints[i]);
                    }
                }
            }
        }
        SpawnAdditionalDoors (posToPlaceDoors);
    }

    private void SpawnAdditionalDoors(List<Transform> points)
    {
        foreach (Transform point in points)
        {
            RaycastHit hit1;
            RaycastHit hit2;

            Physics.Raycast(point.position - point.forward * 0.1f,  point.forward, out hit1, 0.3f, roomLayer);
            Physics.Raycast(point.position + point.forward * 0.1f, -point.forward, out hit2, 0.3f, roomLayer);

            GameObject door;

            if(hit1.collider.transform.parent.parent.Find("Doorway(Clone)")){
               door = GameObject.Instantiate(doorPrefab, point.position, point.rotation, hit2.collider.transform.parent.parent);
            }
            else{
               door = GameObject.Instantiate(doorPrefab, point.position, point.rotation, hit1.collider.transform.parent.parent);
            }


            int numOfRoom1 = hit1.collider.transform.parent.parent.gameObject.GetComponent<RoomInit>().roomId;
            int numOfRoom2 = hit2.collider.transform.parent.parent.gameObject.GetComponent<RoomInit>().roomId;


            _spawnedRooms[numOfRoom1 - 1].AddNeighbor(_spawnedRooms[numOfRoom2 - 1]);
            _spawnedRooms[numOfRoom2 - 1].AddNeighbor( _spawnedRooms[numOfRoom1 - 1]);

            door.GetComponent<DoorwayGeneration>().Initialize(hit1.collider.transform.parent.parent.gameObject, hit2.collider.transform.parent.parent.gameObject);

            Destroy(hit2.collider.gameObject);
            Destroy(hit1.collider.gameObject);
        }
    }

    private void SpawnDoors()
    {
        foreach (Transform point in _spawnPointsForDoors)
        {
            RaycastHit hit1;
            RaycastHit hit2;
      
            Physics.Raycast(point.position - point.forward * 0.1f,  point.forward, out hit1, 0.3f, roomLayer);
            Physics.Raycast(point.position + point.forward * 0.1f, -point.forward, out hit2, 0.3f, roomLayer);
            
            
            int numOfRoom1 = hit1.collider.transform.parent.parent.gameObject.GetComponent<RoomInit>().roomId;
            int numOfRoom2 = hit2.collider.transform.parent.parent.gameObject.GetComponent<RoomInit>().roomId;


            _spawnedRooms[numOfRoom1 - 1].AddNeighbor(_spawnedRooms[numOfRoom2 - 1]);
            _spawnedRooms[numOfRoom2 - 1].AddNeighbor(_spawnedRooms[numOfRoom1 - 1]);


            GameObject door = GameObject.Instantiate(doorPrefab, point.position, point.rotation, hit1.collider.transform.parent.parent);
            door.GetComponent<DoorwayGeneration>().Initialize(hit1.collider.transform.parent.parent.gameObject, hit2.collider.transform.parent.parent.gameObject);

            Destroy(hit2.collider.gameObject);
            Destroy(hit1.collider.gameObject);
        }
    }

    public void UpdateSpawnPointsList(Transform spawnPoint)
    {
        _spawnPoints.Add (spawnPoint);
    }

    public List<GameObject> GetCurrentNeighborsOnMap(int id) {
        List<GameObject> neighbors = new List<GameObject>();

        Room curRoom = _spawnedRooms[id-1];

        foreach (Room room in curRoom.GetNeighbors())
        {
            neighbors.Add(room.currentRoomOnMap);
        }

        return neighbors;
    }

    public List<int> GetCurrentNeighborsOnMapIds(int id) {
        List<int> neighborsIDs = new List<int>();

        Room curRoom = _spawnedRooms[id-1];

        foreach (Room room in curRoom.GetNeighbors())
        {
            neighborsIDs.Add(room.roomId);
        }

        return neighborsIDs;
    }

    public GameObject GetCurrentRoomOnMap(int id) {

        Room curRoom = _spawnedRooms[id-1];

        return curRoom.currentRoomOnMap;
    }
}
