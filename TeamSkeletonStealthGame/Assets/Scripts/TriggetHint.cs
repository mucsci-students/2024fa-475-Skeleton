using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class TriggetHint : MonoBehaviour
{

    [SerializeField]
    protected string levelName;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(LoadLevel(levelName));
        }
    }

    IEnumerator LoadLevel(string hintPrompt)
    {
        GameObject sceneTrigger = GameObject.Find("Hint Box");

        yield return new WaitForSeconds(0f);

        SceneManager.LoadSceneAsync(hintPrompt, LoadSceneMode.Additive);
        Destroy(sceneTrigger);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(hintPrompt));
    }
}
