using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PISSCallReturnScript : MonoBehaviour
{
    [SerializeField]
    protected string sceneName;
    public Animator transition;
    
    void Update() {
        if (Input.GetKeyDown("escape")) {
            SceneManager.UnloadSceneAsync(2, UnloadSceneOptions.None); 
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            Scene scene = SceneManager.GetActiveScene();
            var objects = scene.GetRootGameObjects();
            Destroy(objects[3]);
        }
    }

}
