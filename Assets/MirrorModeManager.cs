using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MirrorModeManager : MonoBehaviour
{
    [SerializeField] private GameObject leftHandVisual;
    [SerializeField] private GameObject rightHandVisual;
    [SerializeField] private GameObject ghostLeftHandVisual;
    [SerializeField] private GameObject ghostRightHandVisual;

    [SerializeField] private bool leftHandToHide = false;

    public bool mirrorModeActive = false;
    
    public void toggleMirrorMode() {

        mirrorModeActive = !mirrorModeActive;

        if (mirrorModeActive)
        {
            if (leftHandToHide)
            {
                HideAndShow(leftHandVisual, ghostLeftHandVisual);
            }
            else if (!leftHandToHide)
            {
                HideAndShow(rightHandVisual, ghostRightHandVisual);
            }
        }
        else { 
            ShowRealHand(leftHandVisual, ghostLeftHandVisual);
            ShowRealHand(rightHandVisual, ghostRightHandVisual);
        }
    }

    // Hide real hand, show ghost/mirror hand
    // Used to hide one real hand, and show corresponding mirror hand
    void HideAndShow(GameObject realHand, GameObject ghostHand) { 
        realHand.SetActive(false);
        ghostHand.SetActive(true);
    }

    // Show real hand, hide ghost/mirror hand
    // Used to show real hand: use for both hands
    void ShowRealHand(GameObject realHand, GameObject ghostHand) { 
        realHand.SetActive(true);
        ghostHand.SetActive(false);
    }
}
