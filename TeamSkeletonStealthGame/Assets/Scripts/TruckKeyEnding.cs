using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class TruckKeyEnding : MonoBehaviour
{

    [SerializeField]
    protected string levelName;

    void OnTriggerEnter2D(Collider2D col)
    {
        Player player = FindObjectOfType<Player>();

        if(player.hasTruckKey) {
            if (col.CompareTag("Player"))
                {
                    StartCoroutine(LoadLevel(levelName));
                }
        }

    }

    IEnumerator LoadLevel(string hintPrompt)
    {
       
        yield return new WaitForSeconds(0f);

        SceneManager.LoadScene(hintPrompt);

    }
}
