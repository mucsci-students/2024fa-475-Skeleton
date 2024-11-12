using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PISSCallReturnScript : MonoBehaviour
{
    [SerializeField]
    protected string sceneName;
    [SerializeField]
    protected string PISSCallName;

    public Animator transition;
    
    void Update() {
        if (Input.GetKeyDown("escape")) {
            SceneManager.UnloadSceneAsync(PISSCallName, UnloadSceneOptions.None); 
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            Scene scene = SceneManager.GetActiveScene();
            
            GameObject originalGameObject = GameObject.Find("Player");
            GameObject child = originalGameObject.transform.GetChild(3).gameObject;
            child.SetActive(true);

            GameObject levelLoader1 = GameObject.Find("LevelLoader 1");
            Destroy(levelLoader1);
        }
    }

}
