using UnityEngine;

public static class CalibrationData
{
    public static bool IsCalibrated { get; set; }
    public static Vector3 CalibratedPosition { get; set; }

    public static bool IsLeftHanded { get; set; }
}