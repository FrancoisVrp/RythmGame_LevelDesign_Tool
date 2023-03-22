/// The TempoMarkerGenerator is in charge of the generation and adaptation of
/// every tempo markers. This markers a visual help for level designer and
/// player to watch the distance with rythm.
/// Each time a value is changed in the ArchiveScript, a function is called on 
/// this script. It calls on every markers Instance another script that will
/// refresh the position of the instantiate object. 
/// Sometimes, for some loading orders issues, the markers can be broken.
/// For this reason, I put another function that can be launch from the editor,
/// which ReGenerateMarkers() in order to delete old ones and create new ones 
/// with a functionnal script.
/// I will try to find from where come from this issue.

// projectName = RythmGame_LevelDesign_Tool
// name = TempoMarkerGenerator
// version = 1.1
// author = FrancoisVrp

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[ExecuteInEditMode]
public class TempoMarkerGenerator : MonoBehaviour
{
    #region VARIABLES
        // Public variables. Allows to launch functions and choose the level
        // length (the number of markers).
        public bool regenerateMarkers;
        public bool refreshMarkers;
        public int wholeNoteTotalNbr;
        public GameObject wholeNoteNbrMarker;
        public GameObject quarterNoteMarker;
        private GameObject tempoGrp;

        // The positionModulo/ order given to each marker.
        private float xOffset;
        private float quarterNoteMod;
        private float wholeNoteNbrMod;
    #endregion

    // Destroy all the markers.
    public void DestroyMarkers()
    {   
        if (tempoGrp != null)
        {
            DestroyImmediate(tempoGrp);
        }
    }
    // Generate all the tempo markers.
    public void GenerateMarkers()
    {   
        tempoGrp = new GameObject("tempoGrp");
        quarterNoteMod = GetComponent<ArchiveScript>().quarterNoteMod;
        wholeNoteNbrMod = GetComponent<ArchiveScript>().wholeNoteMod;
        for (int wholeNoteNbr=0; wholeNoteNbr < wholeNoteTotalNbr; wholeNoteNbr++)
        {   
            for (int quarterNoteNbr =0; quarterNoteNbr < 4; quarterNoteNbr++)
            {
                xOffset= wholeNoteNbr*wholeNoteNbrMod+quarterNoteNbr*quarterNoteMod;
                if (quarterNoteNbr==0)
                {
                    GameObject wholeNoteNbrInstance = 
                        Instantiate(wholeNoteNbrMarker,
                        new Vector3(xOffset,-0.245f,-9),
                        Quaternion.identity);
                    
                    TempoReplace tempoReplaceScript = 
                        wholeNoteNbrInstance.GetComponent<TempoReplace>();

                    tempoReplaceScript.wholeNoteNbr = wholeNoteNbr;
                    tempoReplaceScript.quarterNoteNbr = quarterNoteNbr;
                    tempoReplaceScript.archive = gameObject;
                    wholeNoteNbrInstance.transform.parent = tempoGrp.transform;
                }
                else
                {
                    GameObject quarterNoteInstance = 
                        Instantiate(quarterNoteMarker,
                                    new Vector3(xOffset,-0.245f,-9),
                                    Quaternion.identity);

                    TempoReplace tempoReplaceScript = 
                        quarterNoteInstance.GetComponent<TempoReplace>();
                    
                    tempoReplaceScript.wholeNoteNbr = wholeNoteNbr;
                    tempoReplaceScript.quarterNoteNbr = quarterNoteNbr;
                    tempoReplaceScript.archive = gameObject;
                    quarterNoteInstance.transform.parent = tempoGrp.transform;
                }
            }
        }
    }
    // Start the RefreshMarkers() function in all tempo markers.
    public void RefreshMarkers()
    {
        foreach (Transform child in tempoGrp.transform)
        {
            child.gameObject.GetComponent<TempoReplace>().RefreshMarkers();
        }
    }
    private void Update()
    {
        if (regenerateMarkers)
        {
            regenerateMarkers = false;
            DestroyMarkers();
            GenerateMarkers();
            

        }
        if (refreshMarkers)
        {
            refreshMarkers = false;
            RefreshMarkers();
        }
    }

}
