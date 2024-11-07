using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PISSCallScript : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    public GameObject[] checkpoint;

    [SerializeField]
    protected string PISSCallName;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(LoadLevel(PISSCallName));
        }

    }

    IEnumerator LoadLevel(string pisscall)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(pisscall, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(pisscall));
    }
}
