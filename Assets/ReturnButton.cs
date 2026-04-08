using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ReturnButton : MonoBehaviour
{
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private float rotateSpeed;

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
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {  
        GameObject broken = Instantiate(breakEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(breakSound);
        broken.transform.localScale = 0.5f * transform.localScale;
        foreach (var mesh in GetComponentsInChildren<SkinnedMeshRenderer>())
            mesh.enabled = false;
        
        StartCoroutine(LoadAfterAudio());
    }
    private IEnumerator LoadAfterAudio()
    {
        yield return new WaitForSeconds(breakSound.length);
        SceneManager.LoadScene("MainMenu Scene");
    }

}
