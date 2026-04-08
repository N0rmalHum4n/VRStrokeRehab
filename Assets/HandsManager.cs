using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

public class HandsManager : MonoBehaviour
{
    [SerializeField] private GameObject leftHandAmplified;
    [SerializeField] private GameObject rightHandAmplified;
    [SerializeField] private GameObject leftHandRealVisual;
    [SerializeField] private GameObject rightHandRealVisual;

    void Start()
    {
        if (CalibrationData.IsLeftHanded)
        {
            // hide real right hand, deactivate amplified left hand
            var meshController = rightHandRealVisual.GetComponent<XRHandMeshController>();
            meshController.showMeshWhenTrackingIsAcquired = false;
            meshController.handMeshRenderer.enabled = false;
            leftHandAmplified.SetActive(false);
        }
        else {
            var meshController = leftHandRealVisual.GetComponent<XRHandMeshController>();
            meshController.showMeshWhenTrackingIsAcquired = false;
            meshController.handMeshRenderer.enabled = false;
            rightHandAmplified.SetActive(false);            
        }
        
    }

   
}
