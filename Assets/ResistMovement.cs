using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistMovement : MonoBehaviour
{   

    // This script just binds this object to the origin object
    public GameObject origin;

    // Start is called before the first frame update
    void Start()
    {
        // Set the position of this object to the position of the origin object
        this.transform.position = origin.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the position of this object to the position of the origin object
        this.transform.position = origin.transform.position;
    }
}
