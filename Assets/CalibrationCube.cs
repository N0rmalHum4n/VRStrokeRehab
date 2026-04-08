using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalibrationCube : MonoBehaviour
{
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float bobbingAmplitude;

    
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }
    private void OnEnable()
    {
        Instantiate(spawnEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(spawnSound);
    }
    private void Update()
    {
        transform.position = originalPosition + Vector3.up * Mathf.Sin(Time.time) * bobbingAmplitude;
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {   
        Debug.Log("CUBE SAYS: I GOT HIT WITH THE: " + other.gameObject.tag + "!");
        GameObject broken = Instantiate(breakEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(breakSound);
        broken.transform.localScale = 0.6f * transform.localScale;
        GetComponent<MeshRenderer>().enabled = false;
    }
}
