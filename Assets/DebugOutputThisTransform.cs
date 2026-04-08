using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOutputThisTransform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Output to output log the world position of this object.
        Debug.Log("World Position: " + transform.position);

    }
}
