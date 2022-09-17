using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parametrs : MonoBehaviour
{
    public int Health;
    public GameObject[] partsOfBody;


    private Health _healthScript;
    // Start is called before the first frame update
    void Start()
    {
        _healthScript = gameObject.GetComponent<Health>();
        _healthScript.InitHealth(Health);
        foreach (GameObject part in partsOfBody)
        {
            part.GetComponent<BodyPart>().InitPart(_healthScript);
        }

    }
}
