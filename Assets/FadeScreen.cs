using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{       
    public bool fadeOnStart = true;

    public float fadeDuration = 2;
    public Color fadeColor;
    private Renderer rend;
    // Start is called before the first frame update
    
    public GameObject fadeQuad;

    void Start()
    {
        rend = fadeQuad.GetComponent<Renderer>();
        if (fadeOnStart){
            FadeIn();
        }
        else{
            fadeQuad.SetActive(false);
        }
    }

    public void FadeIn(){
        Fade(1, 0);
    }

    public void FadeOut(){
        Fade(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {   
        // Fade
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {      
       
        if (!fadeOnStart){
            fadeQuad.SetActive(true);
        }

        float timer = 0;
        // at start timer = 0; at end timer = fadeDuration
        // therefore color on way out will be 1 (alphaOut); on way in (alphaIn)
        while(timer <= fadeDuration){ // until timer bigger than fadeDuration

            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration); // calculate new alpha value

            rend.material.SetColor("_Color", newColor); // set new color to material

            timer += Time.deltaTime; // increase timer by time passed since last frame
            yield return null; // wait for 1 frame
        }

        // Make sure that last frame is alphaOut
        Color newColor2 = fadeColor;
        newColor2.a = alphaOut; // calculate new alpha value
        rend.material.SetColor("_Color", newColor2); // set new color to material
        
        // Disable quad infront of camera (done after loading)

        if (fadeOnStart){
            fadeQuad.SetActive(false);
            // after first fade in, deactivate quad.
            fadeOnStart = false;
        }

    }

}
