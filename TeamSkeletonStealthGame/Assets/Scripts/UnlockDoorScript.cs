using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class UnlockDoorScript : MonoBehaviour
{
    [SerializeField]
    protected string levelName;

    [SerializeField]
    protected int clearanceLevel;

    [SerializeField]
    protected string doorLevelName;

    [SerializeField]
    protected string doorToUnlock;

void OnTriggerEnter2D(Collider2D col)
    {
        Player player = FindObjectOfType<Player>();

        if(player.securityClearance >= clearanceLevel) {
            if (col.CompareTag("Player"))
                {
                    StartCoroutine(LoadLevel(levelName, doorLevelName, doorToUnlock));
                    player.hasTruckKey = true;
                }
        }
        

    }
    
    IEnumerator LoadLevel(string hintPrompt, string doorName, string doorToUnlock)
    {
        GameObject sceneTrigger = GameObject.Find(doorToUnlock);
        GameObject door = GameObject.Find(doorName);

        yield return new WaitForSeconds(0f);

        SceneManager.LoadSceneAsync(hintPrompt, LoadSceneMode.Additive);
        Destroy(sceneTrigger);
        Destroy(door);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(hintPrompt));

    }
}
