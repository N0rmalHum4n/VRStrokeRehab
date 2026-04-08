using System.Collections;
using UnityEngine;

public class HandlebarController : MonoBehaviour
{
    public GameObject controleldObj;

    public float moveAmplifier;
    public float rotationAmplifier;

    private Vector3 neutralAngle;
    private Vector3 neutralPos;

    private Quaternion targetRotation;

    private void Start()
    {
        neutralAngle = transform.forward;
        neutralPos = transform.position;

        StartCoroutine(StartControl());
    }

    private IEnumerator StartControl()
    {
        while (true)
        {
            yield return null;
            float angle = Vector3.SignedAngle(neutralAngle, transform.forward, transform.up) * rotationAmplifier;
            //Vector3 pos = controleldObj.transform.position;
            //pos.x += angle * 0.01f * Time.deltaTime;
            //controleldObj.transform.position = pos;

            float xDist = neutralPos.x - transform.position.x;
            float zDist = neutralPos.z - transform.position.z;

            Vector3 amplifiedRotation = new Vector3(-zDist, angle, xDist) * moveAmplifier;



            targetRotation = Quaternion.Euler(amplifiedRotation);
        }
    }

    private void FixedUpdate()
    {
        controleldObj.GetComponent<Rigidbody>().rotation = targetRotation;
    }
}
