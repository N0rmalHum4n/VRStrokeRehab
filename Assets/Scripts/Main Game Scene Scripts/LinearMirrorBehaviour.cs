using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMirrorBehaviour : MonoBehaviour
{
    
    [SerializeField] private Transform trackedHandPosition; // Right hand (changeable)

    [SerializeField] private Transform mirrorPlane; // Mirror plane

    [SerializeField] private float mirrored_offset = 0.05f;

    [SerializeField] private Vector3 world_offset = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (trackedHandPosition == null || mirrorPlane == null){
            return;
        }

        Vector3 relativeHandPos = mirrorPlane.InverseTransformPoint(trackedHandPosition.position);
        Debug.Log("Relative Hand Position: " + relativeHandPos);

        Vector3 mirrorPos = relativeHandPos;
        
        // Orientation based
        mirrorPos.x *= -1;

        mirrorPos += Vector3.right * mirrored_offset;

        Vector3 finalPos = mirrorPlane.TransformPoint(mirrorPos);
        Debug.Log("World Space Final Hand Position: " + finalPos);


        this.transform.position = finalPos + world_offset;
    }
}
