using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;


public class YesCube : MonoBehaviour
{
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float bobbingAmplitude;
    private Vector3 originalPosition;
    public HandednessCheck HandednessCheck;

    private void Awake()
    {
        originalPosition = transform.position;
    }
    private void OnEnable()
    {
        GetComponent<MeshRenderer>().enabled = true;
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
        
        if (other.gameObject.tag != "LEFT POKE" && other.gameObject.tag != "RIGHT POKE") return;
        if (HandednessCheck.isLeftHanded && other.gameObject.tag != "LEFT POKE") return;
        if (!HandednessCheck.isLeftHanded && other.gameObject.tag != "RIGHT POKE") return;
        PlayerPrefs.SetInt("IsLeftHanded", HandednessCheck.isLeftHanded ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Confirmed! IsLeftHanded: " + HandednessCheck.isLeftHanded);

        GameObject broken = Instantiate(breakEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(breakSound);
        broken.transform.localScale = 0.6f * transform.localScale;

        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(LoadAfterAudio());
    }

    private IEnumerator LoadAfterAudio()
    {
        yield return new WaitForSeconds(breakSound.length);
        SceneManager.LoadScene("Calibration Scene");
    }
}