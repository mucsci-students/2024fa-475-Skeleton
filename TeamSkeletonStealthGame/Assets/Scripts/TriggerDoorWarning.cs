using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class TriggerDoorWarning : MonoBehaviour
{
    [SerializeField]
    protected string levelName;

    void OnTriggerEnter2D(Collider2D col)
    {
        Player player = FindObjectOfType<Player>();

        if(player.securityClearance < 1) {
            if (col.CompareTag("Player"))
                {
                    StartCoroutine(LoadLevel(levelName));
                }
        }

    }
    
    IEnumerator LoadLevel(string hintPrompt)
    {
        GameObject sceneTrigger = GameObject.Find("Door Warning");

        yield return new WaitForSeconds(0f);

        SceneManager.LoadSceneAsync(hintPrompt, LoadSceneMode.Additive);
        Destroy(sceneTrigger);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(hintPrompt));
    }
}
