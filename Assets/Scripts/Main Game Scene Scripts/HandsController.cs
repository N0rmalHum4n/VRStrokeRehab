using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Hands.Samples.VisualizerSample;

public class HandsController : MonoBehaviour
{
    public static HandsController HC;

    //Left hand is 0, Right hand is 1
    public int affectedHand;

    public GameObject playerCamera;

    public DefaultHand[] defaultHands = { new DefaultHand("Left Hand"), new DefaultHand("Right Hand") };

    public UniversalHandsBehaviour[] modulatedHands = new UniversalHandsBehaviour[2];

    // Limb Amp
    // TODO: check whether this needs to be public
    public float local_limbampfactor = 0.5f; // local limb amp factor (which is gathered/read by the main limbamp script)

    // Hybrid Script (Christof's Script)
    public float c_healthy = 0.0f; // co-efficient of the healthy hand's force on the hybrid hand 
    // (0 = no force, 1 = full movement from healthy affects unhealthy)
    public float c_unhealthy = 1.0f; // co-efficient of the unhealthy hand's force on the hybrid hand 
    // (0 = no force, 1 = full movement from unhealthy affects healthy; >1 = limb amplification)



    // Hybrid Script (Christof's Script) - Rubberbanding
    // Refer to readme rubberbanding section for more info (all for unhealthy hand).
    public bool snapThresholdTimeRubberBand = false; // SNAPS back to actual pos based on time/ if threshold not broken.
    public bool smoothThresholdTimeRubberBand = false; // rubber bands back to actual pos based on time/ if threshold not broken.
    public bool constantRubberBand = false; // rubber bands back constantly to actual pos.
    public bool velocityRubberBand = false; // rubber bands back to actual pos based on if velocity threshold not broken.
    public bool velocityTimeRubberBand = false; // rubber bands back to actual pos based on time/ if velocity threshold not broken.

    //  TODO: transform all of these boolean variables into a single integer variable
        //    then use a switch statement to determine which type of rubber banding to use

    [SerializeField] private HandVisualizer handvis_Script;

    private void Start()
    {
        if (HC != this && HC != null)
        {
            Destroy(this);
            return;
        }

        HC = this;
        DontDestroyOnLoad(this);

        handvis_Script.drawMeshes = true;
        ResetHandPosition();
    }

    public void ResetHandPosition()
    {
        for (int i = 0; i < 2; i++)
        {
            modulatedHands[i].transform.position = transform.position;
            modulatedHands[i].transform.rotation = defaultHands[i].handObject.transform.rotation;
        }
    }

    public void SetActive(int technique)
    {
        modulatedHands[affectedHand].SetTechnique(technique);
    }

    private void DisableAll()
    {
        for (int i = 0; i < 2; i++)
        {
            defaultHands[i].handObject.SetActive(false);
            defaultHands[i].rayInteractor.SetActive(false);
            handvis_Script.drawMeshes = false;
        }
    }
}

[System.Serializable]
public class DefaultHand
{
    public DefaultHand(string name)
    {
        this.name = name;
    }

    [HideInInspector] [SerializeField] private string name;
    public GameObject handObject;
    public GameObject wristObject;
    public GameObject rayInteractor;
}

[System.Serializable]
public class ModulatedHand
{
    public ModulatedHand(string name)
    {
        this.name = name;
    }

    [HideInInspector] [SerializeField] private string name;

    [Header("Locked Hand")]
    public GameObject lockedHandObject;

    [Header("Mirrored Hand")]
    public GameObject mirroredHandObject;

    [Header("Limb Amp")]
    public GameObject limbampHandObject;
    [HideInInspector] public MimicHiddenHandPosRot limbamp_normalBehaviour_script;
    [HideInInspector] public LimbAmplificationBehaviour limbamp_activeBehaviour_script;

    [Header("Hybrid Script (Christof's Script)")]
    public GameObject hybridHandObject;
    [HideInInspector] public MimicHiddenHandPosRot hybrid_normalBehaviour_script;
    [HideInInspector] public HybridAmplificationBehaviour hybrid_activeBehaviour_script;
}
