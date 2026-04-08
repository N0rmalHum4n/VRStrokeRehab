using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResetPositionButton : MonoBehaviour
{   
    public GameObject XR_origin; // VR Object

    public Transform RespawnPoint; // Respawn Point

    public FadeScreen fadeController;

    public void ButtonPressed(){
        // Debug.Log("Button Pressed");
        Vector3 cameraPos = XR_origin.transform.GetChild(0).GetChild(0).localPosition;
        cameraPos.y = 0;
        Vector3 target = RespawnPoint.position - cameraPos;

        if (Vector3.Distance(XR_origin.transform.position, target) < 1)
            StartCoroutine(MovePosition(target));
        else
            StartCoroutine(Teleport(target));
    }

    IEnumerator MovePosition(Vector3 pos)
    {
        Vector3 currentPos = XR_origin.transform.position;
        float time = 0f;
        while (time < 1f)
        {
            yield return null;
            time += Time.deltaTime;
            XR_origin.transform.position = Vector3.Lerp(currentPos, pos, time);
        }
    }

    IEnumerator Teleport(Vector3 pos)
    {
        fadeController.FadeOut();

        // Wait for fade and scene load.
        float timer = 0;
        while (timer <= fadeController.fadeDuration)
        {

            timer += Time.deltaTime;
            yield return null;

        }

        XR_origin.transform.position = pos;

        fadeController.FadeIn();
    }
}
