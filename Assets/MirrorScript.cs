using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;


public class MirrorScript : MonoBehaviour
{
    XRHandSubsystem currentHandSubsystem;

    [SerializeField] public Transform mirrorPlane;

    private bool rightHandIsTracked;
    private bool leftHandIsTracked;

    public bool mirrorModeActive = false;

    public float zOffset = 0;

    void Start()
    {  
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

        if (updateType == XRHandSubsystem.UpdateType.BeforeRender && XRHandSubsystem.UpdateSuccessFlags.RightHandJoints != 0)
        {
            var rightHand = subsystem.rightHand;
            HandTrackStatUpdate(rightHand);
        }
        else
        {
            return;
        }
    }
    void HandTrackStatUpdate(XRHand hand)
    {
        rightHandIsTracked = hand.isTracked;
        if (rightHandIsTracked)
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
           MirrorTransform(pose.position, pose.rotation);
        }
        else
        {
            return;
        }
    }

    public void MirrorTransform(Vector3 position, Quaternion rotation)
    {
        if (!mirrorModeActive)
        {   
            Debug.Log("Mirror mode is not active, skipping MirrorTransform.");
            return;
        }

        Debug.Log("RIGHT HAND POSITION: " + position + ", ROTATION:" + rotation);

        //Vector3 localPos = mirrorPlane.InverseTransformPoint(position);
        // // Inspired by Ryan's method
        //localPos.x = -localPos.x;
        //this.transform.localPosition = localPos;

        Vector3 mirroredPos = new Vector3(-position.x, position.y, position.z);
        this.transform.position = mirroredPos;


        // My addition for rotations
        this.transform.rotation = new Quaternion(rotation.x, -rotation.y, -rotation.z, rotation.w);

        
        Debug.Log("MIRROR HAND POSITION: " + this.transform.position + ", ROTATION: " + this.transform.rotation);
        Debug.Log("OFFSET: " + (this.transform.position - mirroredPos));

    }

}
    