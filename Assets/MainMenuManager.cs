using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    //[SerializeField] private GameObject instructionCanvas;
    //[SerializeField] private GameObject timerCanvas;
    [SerializeField] private Transform L_MiddleMetacarpalTip;
    [SerializeField] private Transform R_MiddleMetacarpalTip;
    [SerializeField] private float zOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private GameObject gameNameCanvas;
    [SerializeField] private GameObject virtualTable;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip transitionSound;
    [SerializeField] private Transform xROrigin;
    [SerializeField] private GameObject rabbitIcon;
    [SerializeField] private GameObject appleIcon;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject bunnyCanvas;
    [SerializeField] private GameObject fruitCanvas;
    [SerializeField] private GameObject quitCanvas;
   
    [SerializeField] private float canvasYOffset;
    [SerializeField] private float canvasXOffset;
    [SerializeField] private float canvasZOffset;

    //[SerializeField] private GameObject spawnEffect;
    //[SerializeField] private GameObject spawnSound;

    void Start() {
        xROrigin.position = CalibrationData.CalibratedPosition;
        //xROrigin.rotation = Quaternion.Euler(0, 180f, 0);
        //Debug.Log("DEBUGPOSITION, MAIN MENU CALIBRATION: " + CalibrationData.CalibratedPosition);
        //Debug.Log("DEBUGPOSITION, MAIN MENU XR ORIGIN: " + xROrigin.position);
        ShowMainMenu();
    }
    public void ShowMainMenu()
    {
        StartCoroutine(ShowMainMenuRoutine());
    }

    private IEnumerator ShowMainMenuRoutine()
    {
            
        yield return new WaitForSeconds(1f);
       
        audioSource.PlayOneShot(transitionSound);

        StartCoroutine(FadeInTable(virtualTable));
        StartCoroutine(FadeInCanvas(gameNameCanvas));
        yield return new WaitForSeconds(2f);
        StartCoroutine(ShowButtons());
    }

    // The code below has been modified from the original.
    // It was found on Reddit, made by user u/Quetzal-Labs.
    // Modifications: fade in instead of out, with timer
    public IEnumerator FadeInTable(GameObject table)
    {   
        //Debug.Log("TABLE FADER GOT CALLED!");
        MeshRenderer meshRenderer = table.GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        Color colour = meshRenderer.materials[0].color;
        colour.a = 0f;
        meshRenderer.materials[0].color = colour;

        while (colour.a < 1f)
        {
            colour.a += Time.deltaTime / 2f; // 2 seconds to fade in
            colour.a = Mathf.Clamp01(colour.a);
            meshRenderer.materials[0].color = colour;
            yield return null;
        }
    }
    private IEnumerator FadeInCanvas(GameObject canvas)
    {   
        //Debug.Log("CANVAS FADER GOT CALLED!");
        canvas.SetActive(true);
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = canvas.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime / 2f;
            canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha);
            yield return null;
        }
    }

    private IEnumerator ShowButtons()
    {   
        // these values are fine tuned to fit the average human hand spacing on the table
        // determined through copious iterations
        yield return new WaitForSeconds(1f);
        float iconY = virtualTable.transform.position.y + yOffset;
        float iconZ = Mathf.Max(L_MiddleMetacarpalTip.position.z, R_MiddleMetacarpalTip.position.z) + zOffset;

        Instantiate(rabbitIcon, new Vector3(-0.1f, iconY, iconZ - 0.03f), Quaternion.identity);
        bunnyCanvas.transform.position = new Vector3(-0.1f, iconY + 0.02f , iconZ - 0.0925f);
        bunnyCanvas.transform.rotation = Quaternion.Euler(20f, 0f, 0f);
        bunnyCanvas.SetActive(true);

        Instantiate(appleIcon, new Vector3(0.1f, iconY, iconZ - 0.03f), Quaternion.Euler(270f, 0f, 0f));
        fruitCanvas.transform.position = new Vector3(0.1f, iconY + 0.02f, iconZ - 0.0925f);
        fruitCanvas.transform.rotation = Quaternion.Euler(20f, 0f, 0f);
        fruitCanvas.SetActive(true);

        Instantiate(quitButton, new Vector3(-0.21f, iconY + 0.035f, iconZ - 0.15f), Quaternion.Euler(0f, 0f, 45f));
        Instantiate(quitButton, new Vector3(0.21f, iconY + 0.035f, iconZ - 0.15f), Quaternion.Euler(0f, 0f, 45f));
        GameObject quitCanvasLeft = Instantiate(quitCanvas, new Vector3(-0.24f, iconY + 0.09f, iconZ - 0.12f), Quaternion.Euler(30f, -40f, 0f));
        GameObject quitCanvasRight = Instantiate(quitCanvas, new Vector3(0.24f, iconY + 0.09f, iconZ - 0.12f), Quaternion.Euler(30f, 40f, 0f));
        //quitCanvasLeft.SetActive(true);        
        //quitCanvasRight.SetActive(true);

    }
}