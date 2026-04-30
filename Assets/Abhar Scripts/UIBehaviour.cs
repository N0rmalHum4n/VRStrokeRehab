using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private Transform L_MiddleMetacarpalTip;
    [SerializeField] private Transform R_MiddleMetacarpalTip;
    [SerializeField] private float zOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private string leftHandTag;
    [SerializeField] private string rightHandTag;

    private enum HandednessState { WaitingForFirst, WaitingForConfirm, Confirmed }
    private HandednessState currentState = HandednessState.WaitingForFirst;
    private string firstHand;

    void Start()
    {
        instructionText.text = "Hit the cube with your strongest hand!";
        StartCoroutine(SpawnAfterDelay());
    }

    private IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        SpawnCube();
    }

        private IEnumerator LoadNextSceneAfterDelay()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenu Scene");
    }
    private void SpawnCube()
    {
        float cubeY = ((L_MiddleMetacarpalTip.position.y + R_MiddleMetacarpalTip.position.y) / 2f) + yOffset;
        float cubeZ = Mathf.Max(L_MiddleMetacarpalTip.position.z, R_MiddleMetacarpalTip.position.z) + zOffset;
        Vector3 spawnPosition = new Vector3(0f, cubeY, cubeZ);
        Instantiate(cubePrefab, spawnPosition, Quaternion.Euler(45f, 0f, -45f));
    }

    public void OnCubeHit(string handTag)
    {
        Debug.Log("HANDEDNESS: OnCubeHit called. Tag: " + handTag + " State: " + currentState);
        if (currentState == HandednessState.WaitingForFirst)
        {
            firstHand = handTag;
            currentState = HandednessState.WaitingForConfirm;
            instructionText.text = "Hit it with the same hand to confirm. Hit it with the other hand to try again.";
            StartCoroutine(SpawnAfterDelay());
        }
        else if (currentState == HandednessState.WaitingForConfirm)
        {
            if (handTag == firstHand)
            {
                currentState = HandednessState.Confirmed;
                instructionText.text = "Great job!";
                if (firstHand == rightHandTag)
                {
                    Debug.Log("HANDEDNESS: Player is right-handed.");
                    CalibrationData.IsLeftHanded = false;

                }
                else if (firstHand == leftHandTag)
                {
                    Debug.Log("HANDEDNESS: Player is left-handed.");
                    CalibrationData.IsLeftHanded = true;
                }
                StartCoroutine(LoadNextSceneAfterDelay());

            }
            else
            {
                firstHand = null;
                currentState = HandednessState.WaitingForFirst;
                instructionText.text = "Hit the cube with your strongest hand!";
                StartCoroutine(SpawnAfterDelay());
            }
        }
    }
}