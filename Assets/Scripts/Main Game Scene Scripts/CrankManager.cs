using UnityEngine;

public class AngleController : MonoBehaviour
{
    private int controllerID;

    public int GetID => controllerID;
    public int SetID { set => controllerID = value; }

    private float output;

    public float GetOutput => output;

    public delegate void AngleEvent(AngleController controller);

    public AngleEvent OnChange;
    public AngleEvent OnStay;

    private Vector3 lastDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastDirection = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        output = Vector3.SignedAngle(lastDirection, transform.forward, transform.up);

        lastDirection = transform.forward;

        if (Mathf.Abs(output) > 0.01f && OnChange != null)
        {
            OnChange.Invoke(this);
            return;
        }

        if (OnStay != null)
            OnStay.Invoke(this);
    }
}
