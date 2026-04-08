using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MimicHiddenHandPosRot : MonoBehaviour
{   

    // This script causes the unanimated hand model to mimic the position and rotation of the real hidden hand model.

    public GameObject origin;
    public float c = 0.1f; // distance from origin object
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = origin.transform.position + (origin.transform.forward * c);
        this.transform.rotation = origin.transform.rotation;
    }

    void OnEnable()
    {
        // Set the position of this object to the position of the origin object
        this.transform.position = origin.transform.position + (origin.transform.forward * c);
        this.transform.rotation = origin.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = origin.transform.position + (origin.transform.forward * c);
        this.transform.rotation = origin.transform.rotation;
    }
}
