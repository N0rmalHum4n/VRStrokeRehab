using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.SceneManagement;
using TMPro;

public class TableCalibration : MonoBehaviour
{
    //XRHandSubsystem currentHandSubsystem;
    //[SerializeField] private Transform xROrigin;
    //[SerializeField] private Transform virtualTable;
    //[SerializeField] private float yShift;
    //[SerializeField] private float zShift;
    //[SerializeField] private GameObject timerCanvas;
    //[SerializeField] private TextMeshProUGUI countdownText;
    //[SerializeField] private GameObject calibrationCube;
    //[SerializeField] private AudioClip bellSound;
    //[SerializeField] private AudioSource audioSource;
    //private Vector3 setPosition;
    //private float originalYValue;
    //private float originalZValue;
    //private bool weakHandIsTracked;
    //private float weakWristYValue;
    //private float weakWristZValue;
    //private bool isLeftHanded;

    //private IEnumerator Start()
    //{
    //    Debug.Log("TABLE SAYS: Start begun");
    //    isLeftHanded = PlayerPrefs.GetInt("IsLeftHanded", 0) == 1;
    //    originalYValue = xROrigin.position.y;
    //    originalZValue = xROrigin.position.z;

    //    var handSubsystems = new List<XRHandSubsystem>();
    //    SubsystemManager.GetSubsystems(handSubsystems);
    //    for (var i = 0; i < handSubsystems.Count; i++)
    //    {
    //        var handSubsystem = handSubsystems[i];
    //        if (handSubsystem.running)
    //        {
    //            Debug.Log("TABLE SAYS: Found running hand subsystem");
    //            currentHandSubsystem = handSubsystem;
    //            break;
    //        }
    //    }
    //    if (currentHandSubsystem != null)
    //    {
    //        Debug.Log("TABLE SAYS: Subscribing to hand updates");
    //        currentHandSubsystem.updatedHands += OnUpdatedHands;
    //    }

    //    yield return new WaitForSeconds(10f);
    //    Debug.Log("TABLE SAYS: 10 seconds passed");
    //    //countdownText.text = "Cube arriving in:"; 
    //    if (timerCanvas == null) Debug.Log("TABLE SAYS: timerCanvas is null: " + (timerCanvas == null));
    //    if (timerCanvas != null) { 
    //        timerCanvas.SetActive(true);
    //        StartCoroutine(Countdown());
    //}
    //}

    //private IEnumerator Countdown()
    //{
    //    Debug.Log("TABLE SAYS: Countdown started");
    //    audioSource.PlayOneShot(bellSound);
    //    for (int i = 10; i > 0; i--)
    //    {
    //        countdownText.text = "Cube arriving in: " + i.ToString() + " seconds";
    //        yield return new WaitForSeconds(1f);
    //    }
    //    countdownText.text = "";
    //    timerCanvas.SetActive(false);
    //    if (calibrationCube == null) Debug.Log("TABLE SAYS: calibrationCube is null: " + (calibrationCube == null));
    //    if (calibrationCube != null)
    //    {
    //        calibrationCube.SetActive(true);
    //    }
    //}

    //private void OnDisable()
    //{
    //    if (currentHandSubsystem != null)
    //    {   
    //        Debug.Log("TABLE SAYS: Unsubscribing from hand updates");
    //        currentHandSubsystem.updatedHands -= OnUpdatedHands;
    //    }
    //}

    //void OnUpdatedHands(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
    //{
    //    if (updateType != XRHandSubsystem.UpdateType.Dynamic) return;
    //    // track opposite of strong hand
    //    var weakHand = isLeftHanded ? subsystem.rightHand : subsystem.leftHand;
    //    weakHandIsTracked = weakHand.isTracked;
    //    if (weakHandIsTracked)
    //    {   
    //        //Debug.Log("TABLE SAYS: Weak hand is tracked");
    //        var wristJoint = weakHand.GetJoint(XRHandJointID.Wrist);
    //        if (wristJoint.TryGetPose(out var pose))
    //        {
    //            weakWristYValue = pose.position.y;
    //            weakWristZValue = pose.position.z;
    //        }
    //    }
    //}
    //public void Calibrate()
    //{   
    //    Debug.Log("TABLE SAYS: Calibrate called!");
    //    float tableHeight = virtualTable.position.y;
    //    float tableCenterZValue = virtualTable.position.z;
    //    float tableCenterXValue = virtualTable.position.x;
    //    float yOffset = weakWristYValue - tableHeight;
    //    float zOffset = weakWristZValue - tableCenterZValue;
    //    setPosition = new Vector3(tableCenterXValue, originalYValue - yOffset + yShift, originalZValue - zOffset - zShift);
    //    xROrigin.position = setPosition;
    //    CalibrationData.CalibratedPosition = setPosition;
    //    CalibrationData.IsCalibrated = true;
    //}
}