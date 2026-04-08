using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HandednessCheck : MonoBehaviour
{
   
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float bobbingAmplitude;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private GameObject yesCube;
    [SerializeField] private GameObject noCube;
    [SerializeField] private GameObject yesSign;
    [SerializeField] private GameObject noSign;

    public bool firstTime = true;
    public static bool isLeftHanded;
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }
    private void OnEnable()
    {   
        Instantiate(spawnEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(spawnSound);
        GetComponent<MeshRenderer>().enabled = true;
        
    }
    private void Update()
    {   
        
        transform.position = originalPosition + Vector3.up * Mathf.Sin(Time.time) * bobbingAmplitude;
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "LEFT POKE" && other.gameObject.tag != "RIGHT POKE") return;

        if (other.gameObject.tag == "LEFT POKE")
        {
            Debug.Log("HAND SELECTED: LEFT!!!");
            isLeftHanded = true;
        }
        else if (other.gameObject.tag == "RIGHT POKE")
        {
            Debug.Log("HAND SELECTED: RIGHT!!!");
            isLeftHanded = false;
        }
        //PlayerPrefs.SetInt("IsLeftHanded", isLeftHanded ? 1 : 0);
        //PlayerPrefs.Save();

        GameObject broken = Instantiate(breakEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(breakSound);
        broken.transform.localScale = 0.6f * transform.localScale;
        Debug.Log("IsLeftHanded: " + isLeftHanded);
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(DeactivateAfterAudio());
        StartCoroutine(ActivateAfterDelay());
    }

    private IEnumerator DeactivateAfterAudio()
    {
        yield return new WaitForSeconds(breakSound.length);
        gameObject.SetActive(false);
    }

    private IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        confirmationText.text = "Great job! You chose the: " + "\n\n" + (isLeftHanded ? "LEFT" : "RIGHT") + " HAND," + "\n\n" + "as your primary hand." + "\n" + "Are you sure? Hit a cube to confirm YES or NO.";
        yesCube.SetActive(true);
        noCube.SetActive(true);
        yesSign.SetActive(true);
        noSign.SetActive(true);
    }
}

