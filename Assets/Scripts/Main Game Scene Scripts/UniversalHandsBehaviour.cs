using System.Collections;
using UnityEngine;

public class UniversalHandsBehaviour : MonoBehaviour
{
    private int technique;

    public void SetTechnique(int value)
    {
        technique = value;

        LimbAmplificationBehaviour lab = GetComponent<LimbAmplificationBehaviour>();
        HybridAmplificationBehaviour hab = GetComponent<HybridAmplificationBehaviour>();

        lab.enabled = false;
        hab.enabled = false;

        print(technique);

        switch (technique)
        {
            case 0:
                return;
            case 3:
                lab.enabled = true;
                break;
            case 4:
                hab.enabled = true;
                break;
            default:
                StartCoroutine(Temp());
                break;
        }
    }

    [SerializeField] private Transform trackedHandPosition; // Right hand (changeable)

    private Vector3 offset = Vector3.zero;

    public Vector3 SetOffset { set => offset = value; }

    [SerializeField] private float mirrored_offset = 0.05f;

    [SerializeField] private Transform mirrorPlane;


    IEnumerator Temp()
    {
        switch (technique)
        {
            case 1:
                LockedHands();
                //PivotedHands();
                break;
            case 2:
                MirroredHands(false);
                break;
            default:
                yield break;
        }

        yield return null;

        StartCoroutine(Temp());
    }

    void LockedHands()
    {
        // Left hand is locked to a constant offset from the right hand
        if (trackedHandPosition == null)
            return;

        transform.position = trackedHandPosition.position + offset;

    }

    void MirroredHands(bool fullyMirrored)
    {
        if (trackedHandPosition == null || mirrorPlane == null)
            return;
        
        Vector3 relativeHandPos = mirrorPlane.InverseTransformPoint(trackedHandPosition.position);
        // Debug.Log("Relative Hand Position: " + relativeHandPos);

        Vector3 mirrorPos = relativeHandPos;

        // Orientation based
        mirrorPos.x *= -1;

        if (fullyMirrored)
            mirrorPos.y *= -1;

        mirrorPos += Vector3.right * mirrored_offset;

        Vector3 finalPos = mirrorPlane.TransformPoint(mirrorPos);
        // Debug.Log("World Space Final Hand Position: " + finalPos);


        transform.position = finalPos + offset;
    }

    void PivotedHands()
    {
        // Left hand is locked to a constant offset from the right hand
        if (trackedHandPosition == null)
            return;

        Vector3 currentOffset = offset + transform.GetChild(0).localPosition;

        //Vector3.Angle(currentOffset, transform.right);

        currentOffset = trackedHandPosition.localRotation * currentOffset;

        transform.position = trackedHandPosition.position + currentOffset - transform.GetChild(0).localPosition;

    }


}
