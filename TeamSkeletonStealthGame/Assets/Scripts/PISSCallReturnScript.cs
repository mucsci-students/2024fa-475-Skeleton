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

    void Awake() {
        SetTimeUntilTransition();
    }

    void Update() {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0) {
            SceneManager.LoadScene(1);
        }
    }

        private void SetTimeUntilTransition()
    {
        timeUntilSpawn = 5f;
    }

}
