using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ExtractionPointScript : MonoBehaviour
{

    public Animator transition;

    public float transitionTime = 1f;

    public GameObject[] player;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(LoadLevel(1));
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);

        player = GameObject.FindGameObjectsWithTag("Player");
        player[0].transform.position = new Vector3(0f, 0f, 0f);
    }
}