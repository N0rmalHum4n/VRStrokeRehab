using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeButtonHandler : MonoBehaviour
{   
    public void PressToToggleMirror() {         
        MirrorModeManager mirrorModeManager = FindObjectOfType<MirrorModeManager>();
        if (mirrorModeManager != null) {Debug.Log("MirrorModeManager found, toggling mirror mode."); }
        mirrorModeManager.toggleMirrorMode();

    }
}
