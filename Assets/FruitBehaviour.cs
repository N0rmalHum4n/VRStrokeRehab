using System;

using UnityEngine;


public class FruitBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject splatEffect;
    [SerializeField] private AudioSource audioSource;
    //[SerializeField] private AudioClip scoreChomp;
    [SerializeField] private AudioClip tableSplat;
    [SerializeField] private GameObject scoreEffect;
    //[SerializeField] private float fallSpeed = 0.5f;
    private FruitSpawner fruitSpawner;
    private Rigidbody rb;
    private GameObject fruitMarker;
    private float fallSpeed;
    private bool isDone = false;

    void Start()
    {
        fruitSpawner = FindObjectOfType<FruitSpawner>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.MovePosition(transform.position + Vector3.down * fallSpeed * Time.deltaTime);
    }
    public void PlaySound(AudioClip soundClip)
    {
        if (audioSource && soundClip)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }

    public void SetMarker(GameObject m) { 
        fruitMarker = m;
    }

    public void SetFallSpeed(float x) { 
        fallSpeed = x;        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("APPLETRIGGER: hit " + other.gameObject.name);
        if (isDone) return;
        isDone = true;
        if (other.gameObject.tag == "Table")
        {
            //if (isDone) return;
            //isDone = true;
            var mesh = GetComponentInChildren<MeshRenderer>();
            if (mesh) mesh.enabled = false;
            PlaySound(tableSplat);
            Debug.Log("IT HIT THE TABLE!");
            GameObject splat = Instantiate(splatEffect, transform.position + Vector3.up * 0.075f, Quaternion.identity);
            splat.transform.localScale = 0.7f * transform.localScale;
            if (fruitMarker != null) Destroy(fruitMarker);
            fruitSpawner.FruitMissed();
            Destroy(gameObject, 1f);
        }
        else
        {
            GameObject score = Instantiate(scoreEffect, transform.position + Vector3.up * 0.05f, Quaternion.identity);
            score.transform.localScale = 0.7f * transform.localScale;
            Debug.Log("IT HIT THE " + other.gameObject.tag + "!");
            NewGameManager.Instance.AddScore();
            if (!fruitSpawner.IsTimedMode()) fruitSpawner.RequestRespawn();
            if (fruitMarker != null) Destroy(fruitMarker);
            Destroy(gameObject);
        }      

    }
}