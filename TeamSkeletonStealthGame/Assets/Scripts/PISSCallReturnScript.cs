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
            var objects = scene.GetRootGameObjects();
            Destroy(objects[2]);
            Destroy(objects[3]);
            GameObject originalGameObject = GameObject.Find("Player");
            GameObject child = originalGameObject.transform.GetChild(3).gameObject;
            child.SetActive(true);
        }
    }

}
