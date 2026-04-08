using UnityEngine;
using TMPro;
using System.Collections;

public class NoCube : MonoBehaviour
{
    [SerializeField] private GameObject spawnEffect;
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float bobbingAmplitude;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private GameObject handednessCube;
    [SerializeField] private GameObject yesCube;
    [SerializeField] private GameObject yesSign;
    [SerializeField] private GameObject noSign;

    private Vector3 originalPosition;

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

        GameObject broken = Instantiate(breakEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(breakSound);
        broken.transform.localScale = 0.6f * transform.localScale;

        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(ResetAfterAudio());
    }

    private IEnumerator ResetAfterAudio()
    {
        yield return new WaitForSeconds(breakSound.length);       
        yield return new WaitForSeconds(1f);
        confirmationText.text = "Hit the cube with your strong hand!";
        yesCube.SetActive(false);
        handednessCube.SetActive(true);
        gameObject.SetActive(false);
        yesSign.SetActive(false);
        noSign.SetActive(false);
    }
}