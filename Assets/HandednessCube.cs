using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandednessCube : MonoBehaviour
{
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float bobbingAmplitude;
    private UIBehaviour uiBehaviour;

    private bool hasBeenHit = false;
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
        uiBehaviour = FindObjectOfType<UIBehaviour>();
    }
    private void OnEnable()
    {
        Instantiate(spawnEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(spawnSound);
    }
    private void Update()
    {
        //transform.position = originalPosition + Vector3.up * Mathf.Sin(Time.time) * bobbingAmplitude;
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenHit) return;
        if (other.tag != "LEFT POKE" && other.tag != "RIGHT POKE") return;
        hasBeenHit = true;

        AudioSource.PlayClipAtPoint(breakSound, transform.position);
        GameObject broken = Instantiate(breakEffect, transform.position, Quaternion.identity);
        broken.transform.localScale = 0.4f * transform.localScale;
        uiBehaviour.OnCubeHit(other.tag);
        Destroy(gameObject, 0.1f);
    }
}
