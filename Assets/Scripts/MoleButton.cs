using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoleButton : MonoBehaviour
{
   
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private UnityEvent onPressed;    

    public bool isPressed = false;
    private AudioSource audioSource;
    private Renderer buttonRenderer;
    private static int pressedCount = 0;
    private CubeSpawner spawner;
    public bool firstPress = true;

    void Start()
    {   
        spawner = FindObjectOfType<CubeSpawner>();
        buttonRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        pressedCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {   
        PlaySound();
        pressedCount++;
        if (isPressed) return;

        isPressed = true;
        if (firstPress)
        {
            NewGameManager.Instance.timerActive = true;
            firstPress = false;
        } 

        onPressed.Invoke();

        buttonRenderer.material.color = Color.green;       

        if (pressedCount >= 1)
        {   
            pressedCount = 0;
            NewGameManager.Instance.AddScore();            
            Destroy(gameObject, 0.03f);
            spawner.EventRespawn();
        }  
        
    }
    public void PlaySound() { 
        if (audioSource && hitSound)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}