using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.Hands;

public class LimbAmplificationBehaviour : MonoBehaviour
{


    [SerializeField] private GameObject handPosition;
    [SerializeField] private Vector3 lasthandPosition;
    private float amplificationFactor = 1.0f;

    [SerializeField] private float c = 0.1f; // distance from origin object

    // Get limb amp factor from Feedforward script.
    
    [SerializeField] private GameObject XR_Int_Setup; // FF script storage object (where the amp fac is stored)
    private HandsController ff_script; // Limb Amplification Script storage


    // Start is called before the first frame update
    void Start()
    {   
        ff_script = XR_Int_Setup.GetComponent<HandsController>();

        // if (handPosition != null){
        //     this.transform.position = handPosition.position;
        //     this.transform.Translate(0.1f,0.1f,0.1f);

        //     lasthandPosition = Vector3.zero;
        // }

        this.transform.position = handPosition.transform.position + (c * handPosition.transform.forward);
        this.transform.rotation = handPosition.transform.rotation;
    }

    void OnEnable()
    {
        lasthandPosition = handPosition.transform.position + (c * handPosition.transform.forward);
        this.transform.position = handPosition.transform.position + (c * handPosition.transform.forward);
        this.transform.rotation = handPosition.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Adding a offset fowards so that the static hand model fits the real hand position. (skew forwards a tiny bit).
        Vector3 handpos_c_offset = handPosition.transform.position + (c * handPosition.transform.forward);

        // Get limbamp factor from feedforward script.
        amplificationFactor = ff_script.local_limbampfactor;

        // Limb amp script
        // Debug.Log("Hand Position: " + handPosition.position);

        Vector3 movement = handpos_c_offset - lasthandPosition;
        // movement = Vector3.Normalize(movement);
        // Debug.Log("Movement: " + movement);

        Vector3 ampMovement = movement * amplificationFactor;

        // if amplification = 1; then 'rubber band' the object back to the actual real hand position.
        if (amplificationFactor == 1.0f){
            if (Vector3.Distance(handpos_c_offset, this.transform.position) > 0.01f){
                Vector3 direction = (handpos_c_offset - this.transform.position).normalized;
                float distance = Vector3.Distance(handpos_c_offset, this.transform.position);
                
                // the larger the distance the faster the object moves back to the hand.
                // TODO: 1.2f can be a variable
                // this is practically 1.2 ^ x; as a graph.
                float speed = System.MathF.Pow(1.2f, distance) * Time.deltaTime; // speed is proportional to the square of the distance.
                
                // move the object towards the hand (in the direction multiplied by how fast we should move in that direction.)
                // TODO: try adding a constant here like 0.00000001 to really slow down the movement towards the hand.
                ampMovement += speed * direction;
            }
        }

        this.transform.position += ampMovement;
        this.transform.rotation = handPosition.transform.rotation;

        lasthandPosition = handpos_c_offset;
        

    }
}
