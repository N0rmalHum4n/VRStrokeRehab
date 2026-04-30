using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RabbitBehaviour : MonoBehaviour
{

    //TODO: add animations to bunny
    //[SerializeField] private float moveSpeed = 0.05f;
    //[SerializeField] private float minIdleTime = 1f;
    //[SerializeField] private float maxIdleTime = 3f;
    //[SerializeField] private float minRunTime = 1f;
    //[SerializeField] private float maxRunTime = 3f;
    //private Vector3 moveDirection;
    //private RabbitState currentState;
    //private enum RabbitState { Idling, Moving }

    //private Animator rabbitAnimator;

    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private UnityEvent onPressed;
    [SerializeField] private GameObject spawnPoof;
    [SerializeField] private GameObject despawnPoof;

    public bool isPressed = false;
    private AudioSource audioSource;
    private static int pressedCount = 0;
    private BunnySpawner spawner;
    public bool firstPress = true;
    

    void Start()
    {
        //rabbitAnimator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
       
        spawner = FindObjectOfType<BunnySpawner>();
        
        pressedCount = 0;
        PlaySound(spawnSound);
        //randomise rotation of bunny, but face player somewhat
        transform.rotation = Quaternion.Euler(0, Random.Range(120f, 240f), 0);
        //randomise scale of bunny
        float randomScale = 0.09f;
        transform.localScale = Vector3.one * randomScale;
        GameObject poof = Instantiate(spawnPoof, transform.position, Quaternion.identity);
        poof.transform.localScale = 0.5f * transform.localScale;
        //StartCoroutine(ScoreGuard());
    }

    // to prevent autoscoring, don't let bunnies instantly pop when inside user's hand colliders.
    // the timer used is human reaction time average, 0.3 seconds.
    //private IEnumerator ScoreGuard() {
    //    yield return new WaitForSeconds(0.3f);
    //    scoreGuarded = false;
    //}
    // not a good implementation, entire hand collider has to exit and re-enter.

    private void OnTriggerEnter(Collider other)
    {
        //if (scoreGuarded) return;
        Debug.Log("BUNNYDEBUG: I GOT HIT WITH THE: " + other.tag + " !");
        // added foreach because the bunny's eyes stay behind and look scary 
        foreach (var mesh in GetComponentsInChildren<SkinnedMeshRenderer>())
            mesh.enabled = false;
        GetComponent<Collider>().enabled = false;
        PlaySound(hitSound);
        if (isPressed) return;
        isPressed = true;
        pressedCount++;
        if (firstPress)
        {
            NewGameManager.Instance.timerActive = true;
            firstPress = false;
        }
        onPressed.Invoke();
        if (pressedCount >= 1)
        {
            pressedCount = 0;
            NewGameManager.Instance.AddScore();
            GameObject poof = Instantiate(despawnPoof, transform.position, Quaternion.identity);
            poof.transform.position = poof.transform.position + new Vector3(0f, 0.05f, 0f);
            poof.transform.localScale = 0.5f * transform.localScale;
            Destroy(gameObject, 2f);
            spawner.RequestRespawn();
        }
    }

    public void PlaySound(AudioClip soundClip)
    {
        if (audioSource && soundClip)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }

    public void SetDespawnTimer(float time)
    {   
        StartCoroutine(DespawnTimer(time));
    }

    private IEnumerator DespawnTimer(float time)
    {
        yield return new WaitForSeconds(time);
        if (isPressed) yield break; // if already pressed, don't despawn
        spawner.BunnyMissed();
        Destroy(gameObject);
    }
}

