using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTranstionManager : MonoBehaviour
{   
    public FadeScreen fadeController;

    private bool changingScene;

    // public void GoToScene(int sceneIndex){
    //     StartCoroutine(GoToSceneRoutine(sceneIndex));
    // }

    // IEnumerator GoToSceneRoutine(int sceneIndex){
    //     // Fade out
    //     fadeController.FadeOut();
    //     yield return new WaitForSeconds(fadeController.fadeDuration);
        
    //     // Launch new scene
    //     SceneManager.LoadScene(sceneIndex);

    // }

    public void GoToSceneAsync(int sceneIndex)
    {
        if (changingScene)
            return;

        changingScene = true;
        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }

    IEnumerator GoToSceneAsyncRoutine(int sceneIndex){
        // Fade out
        fadeController.FadeOut();
        
        //Launch new scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        operation.allowSceneActivation = false;

        // Wait for fade and scene load.
        float timer = 0;
        while (timer <= fadeController.fadeDuration && !operation.isDone){
            
            timer += Time.deltaTime;
            yield return null;

        }

        operation.allowSceneActivation = true;
    }
}
