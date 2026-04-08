using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedHandBehaviour : MonoBehaviour
{   

    public Transform trackedHandPosition; // Right hand (changeable)

    public Vector3 offset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Left hand is locked to a constant offset from the right hand
        if (trackedHandPosition == null){
            return;
        }

        Vector3 baseoffset = new Vector3(0.0f, 0.0f, 0.0f);
        this.transform.position = trackedHandPosition.position + offset + baseoffset;

    }
}
