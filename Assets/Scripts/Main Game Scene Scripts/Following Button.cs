using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingButton : MonoBehaviour
{ 
    private Vector3 targetDist = new Vector3(0,0,0.6f);
    private Vector3 targetPos;

    private Transform rayInteractor;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        rayInteractor = transform.parent;
        cameraTransform = transform.parent.parent.parent.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = rayInteractor.TransformPoint(targetDist);
        //print(targetPos);
        Debug.DrawLine(rayInteractor.position, targetPos, Color.red);

        

        Vector3 forwardDir = cameraTransform.position - targetPos;

        if (Physics.Linecast(rayInteractor.position, targetPos, out RaycastHit hit))
        {
            transform.position = hit.point;
            forwardDir = hit.normal;
            //Debug.DrawRay(hit.point, hit.normal, Color.red);
            //forwardDir.z = -forwardDir.z;
        }
        else
            transform.position = targetPos;

        transform.rotation = Quaternion.LookRotation(forwardDir, cameraTransform.up);
    }

    public void OnPress()
    {
        print("Working");
    }
}
