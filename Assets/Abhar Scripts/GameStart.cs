using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Transform xROrigin;
    [SerializeField] private Transform virtualTable;
    [SerializeField] private Transform L_Wrist;
    [SerializeField] private Transform R_Wrist;
    [SerializeField] private float yShift;

    // the smaller this value, the farther forward the player is moved
    [SerializeField] private float zShift;

    [SerializeField] private float transitionDuration;

    private float originalYValue;
    private float originalZValue;

    void Start()
    {
        //StartCoroutine(FaceRightWay());
        
        originalYValue = xROrigin.position.y;
        //Debug.Log("GAMESTART: XR Origin Original Y Value: " + originalYValue);
        originalZValue = xROrigin.position.z;
        //Debug.Log("GAMESTART: XR Origin Original Z Value: " + originalZValue);
        StartCoroutine(CalibrateAfterDelay());
    }
   
    private IEnumerator CalibrateAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        CalibrateHeight();
    }

    private void CalibrateHeight()
    {
        float averageWristY = (L_Wrist.position.y + R_Wrist.position.y) / 2f;
        float averageWristZ = (L_Wrist.position.z + R_Wrist.position.z) / 2f;
        //Debug.Log("GAMESTART: Average Wrist Height: " + averageWristY);
        float tableHeight = virtualTable.position.y;
        //Debug.Log("GAMESTART: Table Height: " + tableHeight);
        float tableCenterZ = virtualTable.position.z;
        //Debug.Log("GAMESTART: Table Center Z: " + tableCenterZ);
        float tableCenterX = virtualTable.position.x;
        float yOffset = averageWristY - tableHeight;
        float zOffset = averageWristZ - tableCenterZ;
        Vector3 newPosition = new Vector3(tableCenterX, originalYValue - yOffset + yShift, originalZValue - zOffset - zShift);
        //Debug.Log("GAMESTART: XR Origin New Position: " + newPosition);
        CalibrationData.CalibratedPosition = newPosition;
        CalibrationData.IsCalibrated = true;
        //StartCoroutine(SmoothMove(newPosition));
    }

    private IEnumerator SmoothMove(Vector3 targetPosition)
    {
        Vector3 startPosition = xROrigin.position;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            xROrigin.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        xROrigin.position = targetPosition;
    }
}