using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public void SwitchState()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void TeleportUp()
    {
        Vector3 pos = transform.position;
        pos.y += 2.2f;
        transform.position = pos;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void RemoveAfterTime(float timer)
    {
        StartCoroutine(RemoveTimer(timer));
    }

    IEnumerator RemoveTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        Remove();
    }

    public void Fall(float speed)
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void RemoveOnHeight()
    {
        if(transform.position.y < 0)
            Remove();
    }
}
