using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirroredHandCubeSpawner : MonoBehaviour
{
    public GameObject[] cubeType; // stores all cube prefab
    // 0 = Green Cube
    public List<GameObject> cubes; // stores all active cubes.
    
    public float MaxOffset = 0.2f;
    // offset for mirrored cubes:
    // Cube1|----MaxOffset----|spawnercenter|----MaxOffset----|Cube2
    
    public float beat = (60/105)*2; // beat of song: 105 BPM.
    private float timer; // count time between two beats


    // Start is called before the first frame update
    void Start()
    {
        // Center bar on mirrorpoint

    }

    public void SetSpawnCenter (float xval){ // set the x position of the spawner to the input
        
        Vector3 pos = this.transform.position;
        pos.x = xval;
        this.transform.position = pos;

    }

    // Update is called once per frame
    void Update()
    {
        if (timer > beat)
        {

            Vector3 cubeSP1 = transform.position; // center of spawner
            Vector3 cubeSP2 = transform.position; // center of spawner

            float offset = Random.Range(0,MaxOffset); // for symmetric cubes + for one cube - for other.
            cubeSP1.x += offset;
            cubeSP2.x -= offset;

            GameObject newCube1 = Instantiate(cubeType[0], cubeSP1, Quaternion.identity);
            GameObject newCube2 = Instantiate(cubeType[0], cubeSP2, Quaternion.identity); 

            timer -= beat; // reset timer

        }

        timer += Time.deltaTime;

    }
}
