using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HybridAmplificationBehaviour : MonoBehaviour
{

    // This script gets both the real life hand movements of the unhealthy and healthy hands - then runs a formula to contribute both to a shown unhealthy hand movement.
    // formula : Δuh = Constant_unhealthy * Δactual_unhealthy_movement + Constant_healthy * Δactual_healthy_movement

    [SerializeField] private Transform unhealthyHandPosition; // this is the 'unhealthy hand object'
    private Vector3 unhealthyLastHandPosition; // this is the 'unhealthy hand position last frame'

    [SerializeField] private Transform healthyHandPosition; // this is the 'healthy hand object'
    private Vector3 healthyLastHandPosition; // this is the 'healthy hand position last frame'

    // (these are placeholder - to be overwritten by what is retrieved from the feedfoward script).
    private float C_unhealthyAmpFactor = 1.0f; // degree of which unhealthy hand contributes to unhealthy hand movement
    private float C_healthyAmpFactor = 0.0f; // degree of which healthy hand contributes to unhealthy hand movement


    // Rubber band variables (only for functionality 1/2)
    private bool snapTTRubberBand = false; // if true, the object will snap back to the hand after a certain time.
    private bool smoothTTRubberBand = false; // if true, the object will move back to the hand over a period of time.
    [SerializeField] private float rubberBandThreshold = 0.1f; // if the hand doesn't move past (+-) this threshold, the object will move back to the hand.
    private Vector3 thresholdOrigin; // Object (left hand) that sets the threshold origin.
    [SerializeField] private float rubberBandStartTime = 2.0f; // time before rubber band effect starts
    // public float rubberBandDuration = 0.5f; // duration of rubber band effect
    private float rb1_timer;
    private float rb2_timer; 

    private bool indicator = false;

    // Rubber band variables (for functionality 3)
    private bool constantRubberBand = false; // if true, the object will constantly move back to the hand.

    // Rubber band variable (for functionality 4)
    private bool velocityRubberBand = false; // if true, the object will move back to the hand based on the velocity of the hand.
    [SerializeField] private float velocityRBThreshold = 0.009f; // if the velocity of the hand drops below this threshold, the object will move back to the hand.

    // Rubber band variable (for functionality 5)
    private bool velocityTimeRubberBand = false; // if true, the object will move back to the hand based on the velocity of the hand and time.
    private float rb5_timer;

    // Constant for offset of 'simulated hand' to get it in the position of the 'actual hand'
    [SerializeField] private float c = 0.1f; // distance from origin object

    // TODO: put all variables into some interface to be changed all in 1 place.

    // Get hybrid constants from Feedforward script.
    [SerializeField] private GameObject XR_origin; // FF script storage object (where the constants are stored/changed)
    private HandsController ff_script; // Hybrid Amplification Script storage

    // Start is called before the first frame update
    void Start()
    {   
        // retrieve the feedforward script.
        ff_script = HandsController.HC;



        // unhealthyLastHandPosition = unhealthyHandPosition.position + (c * unhealthyHandPosition.forward);
        // healthyLastHandPosition = healthyHandPosition.position;

        // this.transform.position = unhealthyHandPosition.position + (c * unhealthyHandPosition.forward ); 
        // this.transform.rotation = unhealthyHandPosition.transform.rotation;

        // thresholdOrigin = unhealthyHandPosition.position + (c * unhealthyHandPosition.forward);
    }

    void OnEnable(){
        unhealthyLastHandPosition = unhealthyHandPosition.position + (c * unhealthyHandPosition.forward);
        healthyLastHandPosition = healthyHandPosition.position;

        this.transform.position = unhealthyHandPosition.position + (c * unhealthyHandPosition.forward ); 
        this.transform.rotation = unhealthyHandPosition.transform.rotation;

        thresholdOrigin = unhealthyHandPosition.position + (c * unhealthyHandPosition.forward);
    }

    // // Rubber Band Functionality 2 | Coroutine to do the 'moving' of the object back to the real 'unhealthy' hand.
    // IEnumerator RubberBandRoutine(Vector3 target){
        

    //     float distance = Vector3.Distance(target, this.transform.position);

    //     while (distance > 0.01f){ // if = to stop overshooting

    //         Vector3 direction = (target - this.transform.position).normalized;
    
    //         // the larger the distance the faster the object moves back to the hand.
    //         float speed = MathF.Pow(1.2f, distance) * Time.deltaTime; // speed is proportional to the square of the distance.
            
    //         // move the object towards the hand (in the direction multiplied by how fast we should move in that direction.)
    //         this.transform.position += speed * direction;

    //         distance = Vector3.Distance(target, this.transform.position);

    //         yield return null;
    //     }


    // }  

    void smoothRubberBandMovement(Vector3 target, ref Vector3 ampMovement){

        if (Vector3.Distance(target, this.transform.position) > 0.01f){ // if = to stop overshooting

            Vector3 direction = (target - this.transform.position).normalized;
            float distance = Vector3.Distance(target, this.transform.position);
    
            // the larger the distance the faster the object moves back to the hand.
            // TODO: 1.2f can be a variable
            // this is practically 1.2 ^ x; as a graph.
            float speed = MathF.Pow(1.2f, distance) * Time.deltaTime; // speed is proportional to the square of the distance.
            
            // move the object towards the hand (in the direction multiplied by how fast we should move in that direction.)
            // TODO: try adding a constant here like 0.00000001 to really slow down the movement towards the hand.
            ampMovement += speed * direction;
        }

    }

    // Update is called once per frame
    void Update()
    {   
        
        // the static hand model is slightly backwards in z direction; therefore offset it fowards a bit.
        Vector3 unh_handpos_c_offset = unhealthyHandPosition.position + (c * unhealthyHandPosition.forward);

        // Get hand constants from feedforward script.
        C_healthyAmpFactor = ff_script.c_healthy;
        C_unhealthyAmpFactor = ff_script.c_unhealthy;

        // Get rubber band features from feedforward script. (only one can be enabled at a time preferably)
        snapTTRubberBand = ff_script.snapThresholdTimeRubberBand;
        smoothTTRubberBand = ff_script.smoothThresholdTimeRubberBand;
        constantRubberBand = ff_script.constantRubberBand;
        velocityRubberBand = ff_script.velocityRubberBand;
        velocityTimeRubberBand = ff_script.velocityTimeRubberBand;

        // Hybrid Amplification Script:
        // Get unhealthy/ healthy hand movement (displacement from last frame).
        Vector3 unhealthymovement = unh_handpos_c_offset - unhealthyLastHandPosition;
        Vector3 healthymovement = healthyHandPosition.position - healthyLastHandPosition;

        // Calculate the amplified movement of the unhealthy hand.
        Vector3 ampMovement = C_unhealthyAmpFactor * unhealthymovement + C_healthyAmpFactor * healthymovement;


        // Rubber band features | If the simulated unhealthy hand is not where the real hand should be
        // these features give ways to 'snap' or 'slide' the simulated hand back to the real hand position.
        // Rubber band functionality 1 --> snapping rubber band
        if (snapTTRubberBand){
            // If we move outside of the threshold, reset the timer; reset the threshold origin to the new left hand position.
            if (Vector3.Distance(unh_handpos_c_offset, thresholdOrigin) >= rubberBandThreshold){
                thresholdOrigin = unh_handpos_c_offset;
                rb1_timer = 0.0f;
            }
            else{ // we have not moved outside of the threshold; Check time
                //if time is greater than the rubber band start time (2 secs default), then reset the position of the object to the hand.
                if (rb1_timer >= rubberBandStartTime){
                    rb1_timer = 0.0f;
                    thresholdOrigin = unh_handpos_c_offset;
                    
                    this.transform.position = unh_handpos_c_offset; // primative rubber band --> snap back to real pos.
                    
                }
                rb1_timer += Time.deltaTime;
            }
        }
        // Rubber band functionality 2 --> smooth rubber band
        else if (smoothTTRubberBand){
            if (indicator){
                float distance = Vector3.Distance(unh_handpos_c_offset, this.transform.position);
                // if the hand has finished rubber banding:
                if (distance < 0.01f){
                    // Debug.Log("End smooth rubber band movement");
                    indicator = false;
                }
                else{
                    // Debug.Log("Smooth rubber band movement");
                    smoothRubberBandMovement(unh_handpos_c_offset, ref ampMovement);
                }
            } 
            else{
                // If we move outside of the threshold, reset the timer; reset the threshold origin to the new left hand position.
                if (Vector3.Distance(unh_handpos_c_offset, thresholdOrigin) >= rubberBandThreshold){
                    thresholdOrigin = unh_handpos_c_offset;
                    rb2_timer = 0.0f;
                }
                else{ // we have not moved outside of the threshold; Check time
                    //if time is greater than the rubber band start time (2 secs default), then reset the position of the object to the hand.
                    if (rb2_timer >= rubberBandStartTime){
                        // Debug.Log("Start smooth rubber band movement");
                        rb2_timer = 0.0f;
                        thresholdOrigin = unh_handpos_c_offset;

                        // Smooth rubber band --> move the object back to the hand over a period of time.
                        // Start smooth rubber band: move the hand the whole way
                        indicator = true;

                    }
                    rb2_timer += Time.deltaTime;
                }
            }

        }
        // Rubber band Functionality 3 | Constantly move the object back to the hand.
        else if (constantRubberBand){

            smoothRubberBandMovement(unh_handpos_c_offset, ref ampMovement);

        }
        // Rubber band Functionality 4 | Velocity based rubber band
        else if (velocityRubberBand){
            // if ampmovement (velocity) drops below a certain threshold; 
            if (ampMovement.magnitude < velocityRBThreshold){

                smoothRubberBandMovement(unh_handpos_c_offset, ref ampMovement);

            }

        }
        // Rubber band Functionality 5 | Velocity/ Time based rubber band
        else if (velocityTimeRubberBand){
            // If we move faster than the velocity threshold reset the timer.
            if (ampMovement.magnitude > velocityRBThreshold){
                // Debug.Log("Moved faster than threshold: resetting timer" + ampMovement.magnitude);
                rb5_timer = 0.0f;
            }
            else{ // we have not moved outside of the threshold; Check time
                //if time is greater than the rubber band start time (2 secs default), then reset the position of the object to the hand.
                if (rb5_timer >= rubberBandStartTime){
                    // Debug.Log("Slower than threshold for 2 seconds: rubber band start." + ampMovement.magnitude);
                    thresholdOrigin = unh_handpos_c_offset;

                    // Smooth rubber band --> move the object back to the hand over a period of time.
                    smoothRubberBandMovement(unh_handpos_c_offset, ref ampMovement);
                }
                rb5_timer += Time.deltaTime;
            }
        }

        this.transform.position += ampMovement; // move this object by the calculated amplified movement

        // Update the last frame positions of the hands.
        unhealthyLastHandPosition = unh_handpos_c_offset;
        healthyLastHandPosition = healthyHandPosition.position;

        this.transform.rotation = unhealthyHandPosition.rotation; // rotate the object to the rotation of the hand.
        
    }
}
