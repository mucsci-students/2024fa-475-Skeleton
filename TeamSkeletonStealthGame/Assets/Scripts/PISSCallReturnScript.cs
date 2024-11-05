using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PISSCallReturnScript : MonoBehaviour
{

    public Animator transition;

    public GameObject[] player;

    private float timeUntilSpawn;
    
    void Update() {
        if (Input.GetKeyDown("escape")) {
            SceneManager.LoadScene(1);
        }
    }

}
