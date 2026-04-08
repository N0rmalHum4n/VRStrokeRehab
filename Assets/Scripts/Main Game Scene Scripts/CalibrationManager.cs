using System.Collections;
using UnityEngine;

public class CalibrationManager : MonoBehaviour
{
    public static CalibrationManager instance;

    private int calibrationStage;

    [SerializeField] private GameObject confirmButton;

    public void OnConfirm()
    {
        calibrationStage++;
        print(calibrationStage);
    }

    private float[] maxReach = new float[2];

    public void StartCalibration()
    {
        confirmButton.SetActive(true);
        calibrationStage = 0;
        for (int i = 0; i < 2; i++)
            maxReach[i] = 0f;

        StartCoroutine(RecordMovement(HandsController.HC.playerCamera, HandsController.HC.defaultHands));
    }

    private IEnumerator RecordMovement(GameObject cameraObj, DefaultHand[] defaultHands)
    {
        float currentReach = 0f;
        while (calibrationStage == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                currentReach = Mathf.Abs(cameraObj.transform.position.x - defaultHands[i].wristObject.transform.position.x);
                if (maxReach[i] < currentReach)
                    maxReach[i] = currentReach;
            }

            yield return null;
        }

        int affectedHand = maxReach[0] > maxReach[1] ? 1 : 0;
        HandsController.HC.affectedHand = affectedHand;

        HandsController.HC.local_limbampfactor = maxReach[affectedHand] / maxReach[1 - affectedHand];

        while (true)
        {
            if (calibrationStage == 2)
            {
                RecordHandPos(defaultHands, HandsController.HC.modulatedHands, affectedHand);
                break;
            }
            yield return null;
        }

        confirmButton.SetActive(false);
    }

    private void RecordHandPos(DefaultHand[] defaultHands, UniversalHandsBehaviour[] modulatedHands, int affectedHand)
    {
        HandsController.HC.ResetHandPosition();
        //Take the position of the unhealthy wrist and the position of the fake hand and subtract the distance between them from the position of the unhealthy hand
        Vector3 pos = Vector3.zero;
        pos.y = -defaultHands[affectedHand].wristObject.transform.position.y;
        modulatedHands[affectedHand].GetComponent<UniversalHandsBehaviour>().SetOffset = pos; //- modulatedHands[affectedHand].transform.GetChild(0).position;

        //TODO: Record the mirror point by finding the middle point between the two hands

        //TODO: Record the height of the desk
    }
}
