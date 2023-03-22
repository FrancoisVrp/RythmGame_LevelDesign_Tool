/// The ArchiveScript has the role to hold every crucial informations for the
/// other scripts to work. Every time the level designer change a value in the
/// editor, like the tempo or the speed, every modulos are refreshed.
/// Every objects will get the new incrementation on their OnUpdate(), the
/// function dedicated to the editor working.
/// The only objects to change on the order of the archive, are the markers.
/// Because the level designer don't have to change manually, I prefer for 
/// optimisation purpose, that their position update only when ArchiveScript
/// values change.

// projectName = RythmGame_LevelDesign_Tool
// name = ArchiveScript
// version = 1.1
// author = FrancoisVrp

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class ArchiveScript : MonoBehaviour
{
    #region VARIABLES
        // Music and level main variables.
        public float tempo;
        public float speed;
        public float jumpHeight;
        // Variables calculated inside for every other scripts. They are public
        // to be used by other scripts, but masked in the inspector.
        #region VARIABLE_CALCULATED_INSIDE
            [HideInInspector]
            public float sixteenNoteMod;
            [HideInInspector]
            public float eightNoteMod;
            [HideInInspector]
            public float quarterNoteMod;
            [HideInInspector]
            public float halfNoteMod;
            
            [HideInInspector]
            public float wholeNoteMod;
            [HideInInspector]
            public float halfNoteHeightMod;
            [HideInInspector]
            public float quarterNoteHeightMod;
            [HideInInspector]
            public float eightNoteHeightMod;
            [HideInInspector]
            public float sixteenNoteHeightMod;
        #endregion

        // Start level selection.
        public int level = 1;

        // Player references.
        public GameObject playerCharacter;
        public GameObject playerCamera;
        public GameObject trail;

        // Audio tracks.
        public AudioClip track1;
        public AudioClip track2;
    #endregion

    // Set color on Start in function of the level.
    void Start()
    {
        if (level == 1)
        {
            GetComponent<ColorChange>().ColorLevel1();
        }
        else if (level == 2)
        {
            GetComponent<ColorChange>().ColorLevel2();
        }
        else if (level == 3)
        {
            GetComponent<ColorChange>().ColorLevel3();
        }
    }

    // Set a bunch of modulos to increment the platforms position.
    void RefreshModulos()
    {
        quarterNoteMod = speed*(60/tempo);
        wholeNoteMod = quarterNoteMod*4;
        halfNoteMod = quarterNoteMod * 2;
        eightNoteMod = quarterNoteMod *0.5f;
        sixteenNoteMod = quarterNoteMod *0.25f;
        quarterNoteHeightMod = (jumpHeight * (120/tempo))-0.15f;
        halfNoteHeightMod = quarterNoteHeightMod*2;
        eightNoteHeightMod = quarterNoteHeightMod * 0.5f;
        sixteenNoteHeightMod = quarterNoteHeightMod * 0.25f;
    }

    // Each time the script is loaded or a value change in the inspector.
    void OnValidate()
    {
        RefreshModulos();
        GetComponent<TempoMarkerGenerator>().RefreshMarkers();
    }
}