using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMirrorPlane : MonoBehaviour
{   
    public Vector3 offset = new Vector3(0,-0.3f, 0.3f);
    public bool resetPlanePosition;
    // Start is called before the first frame update

    // Mirrored Spawner reference
    public GameObject mirroredSpawner;
    private MirroredHandCubeSpawner mS_Script;

    void Start()
    {   
        resetPlanePosition = true;

        mS_Script = mirroredSpawner.GetComponent<MirroredHandCubeSpawner>();
    }

    // TODO: constant return cycle might be causing lag? Might need a fix

    // Update is called once per frame
    void Update()
    {
        if (!resetPlanePosition){
            return;
        }

        this.transform.position = Camera.main.transform.position + offset;
        // also reset the middle position of the spawner. (mirroredspawner is locked to the mirror plane pos)
        //mS_Script.SetSpawnCenter(this.transform.position.x);


        resetPlanePosition = false;
    }
    void ResetMirrorPosition(){
        resetPlanePosition = true;
    }

}
