using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPrefabBehaviour : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip spawnSound;
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject spawnEffect;
    [SerializeField] GameObject breakEffect;
    
    private bool introTriggered = false;
    private void OnEnable()
    {
        Instantiate(spawnEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(spawnSound);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (introTriggered) return;
       
            introTriggered = true;
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
            GameObject broken = Instantiate(breakEffect, transform.position, Quaternion.identity);
            broken.transform.localScale = 0.4f * transform.localScale;
            IntroManager.Instance.OnDummyHit();
            Destroy(gameObject, 0.1f);
        
    }
}

