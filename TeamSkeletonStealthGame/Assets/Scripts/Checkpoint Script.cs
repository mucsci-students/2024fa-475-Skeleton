using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CheckpointScript : MonoBehaviour
{
    public float transitionTime = 1f;

    public GameObject[] player;

    public Animator transition;

    [SerializeField]
    protected string levelName;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(LoadLevel(levelName));
        }
    }

    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        player = GameObject.FindGameObjectsWithTag("Player");
        player[0].transform.position = new Vector3(player[0].GetComponent<Transform>().position.x, player[0].GetComponent<Transform>().position.y, player[0].GetComponent<Transform>().position.z);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }
}
