using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedHandCubeSpawner : MonoBehaviour
{
    public GameObject[] cubeType; // stores all cube prefab
    // 0 = Green Cube
    public List<GameObject> cubes; // stores all active cubes.
    
    public float offset = 0.15f; // +- x axis spawn offset that cubes will spawn at
    
    private float lockedCubeOffset = 0.3f; // offset for locked hand cubes

    public float beat = (60/105)*2; // beat of song: 105 BPM.
    private float timer; // count time between two beats


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > beat)
        {
            
            Vector3 middleSpawnPos = transform.position;
            middleSpawnPos.x += Random.Range(-offset, offset); // randomize central x position

            // Locked hand version - spawn two cubes with 1/2 offset each side (middleSpawnPos is the middle of the two cubes).
            // E.g if lockedCubeOffset = 0.3f. Then the two cubes will spawn at x += -0.15f and x += 0.15f from middleSpawnPos.x.
            Vector3 cubeSP1 = middleSpawnPos;
            Vector3 cubeSP2 = middleSpawnPos;

            cubeSP1.x += -(0.5f * lockedCubeOffset);
            cubeSP2.x += (0.5f * lockedCubeOffset);

            GameObject newCube1 = Instantiate(cubeType[0], cubeSP1, Quaternion.identity);
            GameObject newCube2 = Instantiate(cubeType[0], cubeSP2, Quaternion.identity);

            // cubes.Add(newCube); // add the cube to the list of active cubes

            timer -= beat; // reset timer

        }

        timer += Time.deltaTime;

    }
}
