/// This script is put on every instantiate marker. The only function
/// RefreshMarkers() is called if a value is changed on the ArchiveScript.
/// It refresh the position of the instantiate tempo marker.

// projectName = RythmGame_LevelDesign_Tool
// name = TempoReplace
// version = 1.1
// author = FrancoisVrp

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TempoReplace : MonoBehaviour
{
    #region VARIABLES
        // The incrementation number to know where must be the marker at any time.
        #region VARIABLE_Set_OnInstanciate
            //[HideInInspector]
            public int wholeNoteNbr;
            //[HideInInspector]
            public int quarterNoteNbr;
        #endregion

        #region ARCHIVE_VARIABLES
            [HideInInspector]
            public GameObject archive;
            private float wholeNoteMod;
            private float tempsMod;
        #endregion
        private float xOffset;
    #endregion



    // Change the position of the marker every time this function is called.
    // It is an heavy charge for the TempoMarkerGenerator script to call
    // each marker, to update it's position when a change is made on the archive
    // script.
    // But I prefer this function to be called some time, than that every
    // markers use a Update() that would probably be so much heavier.
    public void RefreshMarkers()
    {   
        wholeNoteMod = archive.GetComponent<ArchiveScript>().wholeNoteMod;
        tempsMod = archive.GetComponent<ArchiveScript>().quarterNoteMod;
        xOffset= wholeNoteNbr*wholeNoteMod+quarterNoteNbr*tempsMod;
        transform.position = new Vector3 (xOffset,-0.245f,-9);
    }
}
