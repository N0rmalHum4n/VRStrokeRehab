using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Hitcube : MonoBehaviour
{
    //These allow you to assign events in the inspector
    [SerializeField] private UnityEvent SpawnEvent;
    [SerializeField] private UnityEvent EnterEvent;
    [SerializeField] private UnityEvent StayEvent;
    [SerializeField] private UnityEvent ExitEvent;
    [SerializeField] private UnityEvent UpdateEvent;

    private int entered;

    private void Start()
    {
        StartCoroutine(CallOnSpawn());
    }

    IEnumerator CallOnSpawn()
    {
        yield return null; 
        if (SpawnEvent != null)
            SpawnEvent.Invoke();
    }

    private void Update()
    {
        if (UpdateEvent != null)
            UpdateEvent.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        entered++;
        if (entered != 1)
            return;
        if (EnterEvent != null)
            EnterEvent.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (StayEvent != null)
            StayEvent.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        entered--;
        if (entered != 0)
            return;
        if (ExitEvent != null)
            ExitEvent.Invoke();
    }
}
