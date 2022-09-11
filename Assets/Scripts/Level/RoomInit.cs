using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInit : MonoBehaviour
{
    public Transform spawnPointsHolder;

    private bool initialized = false;

    public float length = 2;

    public int roomId;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized) return;

        LevelGenerator lvlGen = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        for (int i = 0; i < spawnPointsHolder.childCount; i++)
        {
            Transform point = spawnPointsHolder.GetChild(i);
            lvlGen.UpdateSpawnPointsList(point);
        }

        transform.Find("Checker").gameObject.SetActive(false);

        initialized = true;
    }

    void OnDrawGizmos()
    {
        if (!spawnPointsHolder) return;
        if (spawnPointsHolder.childCount <= 0) return;
        Gizmos.color = Color.red;
        for (int i = 0; i < spawnPointsHolder.childCount; i++)
        {
            Gizmos
                .DrawLine(spawnPointsHolder.GetChild(i).position,
                spawnPointsHolder.GetChild(i).position +
                spawnPointsHolder.GetChild(i).forward * length);
            Gizmos.DrawSphere(spawnPointsHolder.GetChild(i).position, length);
        }
    }
}
