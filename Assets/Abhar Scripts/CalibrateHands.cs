using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;


public class CalibrateHands : MonoBehaviour
{
    XRHandSubsystem currentHandSubsystem;

    [SerializeField] private Transform xROrigin;
    [SerializeField] private Transform virtualTable;
    [SerializeField] private float yShift;
    [SerializeField] private float zShift;


    private Vector3 setPosition;
    private float originalYValue;
    private float originalZValue;
    private bool leftHandIsTracked;
    private float leftWristYValue;
    private float leftWristZValue;
    void Start()
    {   
        originalYValue = xROrigin.position.y;
        originalZValue = xROrigin.position.z;


        var handSubsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(handSubsystems);
        for (var i = 0; i < handSubsystems.Count; i++)
        {
            var handSubsystem = handSubsystems[i];
            if (handSubsystem.running)
            {
                currentHandSubsystem = handSubsystem;
                break;
            }
        }
        if (currentHandSubsystem != null)
        {
            currentHandSubsystem.updatedHands += OnUpdatedHands;
        }
    }
    private void OnDisable()
    {
        if (currentHandSubsystem != null)
        {
            currentHandSubsystem.updatedHands -= OnUpdatedHands;
        }
    }
    void OnUpdatedHands(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
    {

        if (updateType == XRHandSubsystem.UpdateType.Dynamic && XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints != 0)
        {
            var leftHand = subsystem.leftHand;
            HandTrackStatUpdate(leftHand);
        }
        else
        {
            return;
        }
    }
    void HandTrackStatUpdate(XRHand hand)
    {
        leftHandIsTracked = hand.isTracked;
        if (leftHandIsTracked)
        {
            ReadJointData(hand);
        }
    }
    void ReadJointData(XRHand hand)
    {
        var wristJoint = hand.GetJoint(XRHandJointID.Wrist);
        GetPosition(wristJoint);
    }

    void GetPosition(XRHandJoint joint)
    {
        if (joint.TryGetPose(out var pose))
        {
            GetYValue(pose.position);
        }
        else
        {
            return;
        }
    }
    void GetYValue(Vector3 position)
    {
        leftWristYValue = position.y;
        leftWristZValue = position.z;
    }

    public void Calibrate() {
            float tableHeight = virtualTable.position.y;
            float tableCenterZValue = virtualTable.position.z;
            float tableCenterXValue = virtualTable.position.x;         
            float yOffset = leftWristYValue - tableHeight;
            float zOffset = leftWristZValue - tableCenterZValue;
            setPosition = new Vector3(tableCenterXValue, originalYValue - yOffset + yShift, originalZValue - zOffset - zShift);        
            xROrigin.position = setPosition;
            
    }

}
