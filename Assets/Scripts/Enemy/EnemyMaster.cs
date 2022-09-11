using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaster : MonoBehaviour
{
    private bool playerFound;

    private List<FieldOfViewEnemySystem> enemies;


    private int _currentAmountOfEnemies;
    private int _amounOfLoosePlayerEnemies;

    void Start(){
        enemies = new List<FieldOfViewEnemySystem>();
        for (int i = 0; i < transform.childCount; i++)
        {
            enemies.Add(transform.GetChild(i).GetComponent<FieldOfViewEnemySystem>());
        }
        _currentAmountOfEnemies = enemies.Count;
    }
    
    private void UpdateStateFoundPlayer(){
        foreach (FieldOfViewEnemySystem enemy in enemies)
        {
            enemy.SetStateFoundPlayer();
        }
    }

    public void PlayerFound(){
        playerFound = true;
        _amounOfLoosePlayerEnemies = 0;
        UpdateStateFoundPlayer();
    }
    
    public void PlayerLoose(){
        _amounOfLoosePlayerEnemies++;
        if(_amounOfLoosePlayerEnemies >= _currentAmountOfEnemies){
            playerFound = false;
        }
    }
}
