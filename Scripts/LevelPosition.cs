/// LevelPosition is the script in charge of the position of every platform
/// of the level. The level designer can choose on each platform, an 
/// incrementation level to set the position of the platform in rythm in the
/// game.
/// For example he can put a platform on every beat or half beat.

// projectName = RythmGame_LevelDesign_Tool
// name = LevelPosition
// version = 1.1
// author = FrancoisVrp

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class LevelPosition : MonoBehaviour
{
    #region VARIABLES
        // Modulo get from the archive script.
        #region ARCHIVE_VARIABLES
        public GameObject archiveObject;
        private ArchiveScript archiveScript;

        // Modulo variables
        private float sixteenNoteMod;
        private float eightNoteMod;
        private float quarterNoteMod;
        private float halfNoteMod;

        private float halfNoteHeightMod;
        private float quarterNoteHeightMod;
        private float eightNoteHeightMod;
        private float sixteenNoteHeightMod;
    #endregion

        // The variables that adapt and increment the position of platforms.
        #region POSITION_VARIABLES
        // The position without incrementation.
        private Vector3 stdPosition;

        // A modulo based on tempo and speed/jump height.
        private Vector2 modStdPosition; 

        // The incremented position of the platform.
        private Vector2 fixedPosition;
        // Useful to fixed the vertical position from the top of the platform
        // rather than its origin.
        private Vector3 scaleObject;
    #endregion

        // Public bool to pick an increment size on height and lenght.
        #region INCREMENT_BOOL
        public bool halfNoteX;
        public bool eightNoteX;
        public bool sixteenNoteX;
        public bool halfNoteY;
        public bool eightNoteY;
        public bool sixteenNoteY;

        // The private bool to know which one is change by the level designer.
        private bool previousHalfNoteX = false;
        private bool previousHalfNoteY = false;
        private bool previouseightNoteX = false;
        private bool previousEightNoteY = false;
        private bool previoussixteenNoteX = false;  
        private bool previousSixteenNoteY = false;

        // Bool to know if the level designer change a bool.
        private bool boolChange = false;
    #endregion
    #endregion

    #region FUNCTIONS
        // START
        private void Start()
        {

        }

        // ON UPDATE
        // Called if in EditorMode. It will force the platform to fit to a tempo
        // position in height and lenght.
        private void OnUpdate()
        {
            archiveScript = archiveObject.GetComponent<ArchiveScript>();
            // Get the current Archive modulo.
            quarterNoteMod = archiveScript.quarterNoteMod;
            halfNoteMod = archiveScript.halfNoteMod;
            eightNoteMod = archiveScript.eightNoteMod;
            sixteenNoteMod = archiveScript.sixteenNoteMod;
            halfNoteHeightMod = archiveScript.halfNoteHeightMod;
            quarterNoteHeightMod = archiveScript.quarterNoteHeightMod;
            eightNoteHeightMod = archiveScript.eightNoteHeightMod;
            sixteenNoteHeightMod = archiveScript.sixteenNoteHeightMod;

            scaleObject = transform.localScale;

            // Set the proper modulo incrementation based on the increment 
            // choose by the level designer. If no increment has been choosen, 
            // the modulo will be based on the beatRate/tempo.
            if (halfNoteX == true)
            {
                quarterNoteMod = halfNoteMod;
            }
            else if (eightNoteX == true)
            {
                quarterNoteMod = eightNoteMod;
            }
            else if (sixteenNoteX == true)
            {
                quarterNoteMod = sixteenNoteMod;
            }
            
            if (halfNoteY == true)
            {
                quarterNoteHeightMod = halfNoteHeightMod;
            }
            else if (eightNoteY == true)
            {
                quarterNoteHeightMod = eightNoteHeightMod;
            }
            else if (sixteenNoteY == true)
            {
                quarterNoteHeightMod = sixteenNoteHeightMod;
            }

            // Calculate the vector2 of the standard position modulo.
            stdPosition = new Vector3 (transform.position.x,
                                       transform.position.y + scaleObject.y*0.5f,
                                       transform.position.z);
            modStdPosition.x = stdPosition.x % quarterNoteMod;
            modStdPosition.y = ((stdPosition.y) % quarterNoteHeightMod);

            // Then, it allows to compare in a define range, of how much the 
            // standard position is greater or smaller than the middle between 0 
            // and the increment distance.
            // The fixed position get the closest modulo position by this method.
            if (modStdPosition.x < quarterNoteMod/2)
            {
                fixedPosition.x = stdPosition.x - modStdPosition.x;
            }
            else if (modStdPosition.x >= quarterNoteMod/ 2)
            {
                fixedPosition.x = (stdPosition.x - modStdPosition.x)
                                   + quarterNoteMod;
            }
            if (modStdPosition.y < quarterNoteHeightMod/ 2)
            {
                fixedPosition.y = stdPosition.y - modStdPosition.y;
            }
            else if (modStdPosition.y >= quarterNoteHeightMod/ 2)
            {
                fixedPosition.y = (stdPosition.y - modStdPosition.y)
                                   + quarterNoteHeightMod;
            }

            // Then the transform position get on x and y the fixedPosition.
            transform.position = new Vector3(fixedPosition.x,
                                            fixedPosition.y-scaleObject.y*0.5f,
                                            -4.5f);
        }

        // UPDATE
        //
        // Allows the seperation between editorMode and gameMode.
        private void Update()
        {
            if (!Application.isPlaying)
            {
                OnUpdate();
                return;
            }
        }

        // SWITCH BOOLEAN
        //
        // Called if a incrementation has been changed by the level designer.
        // If the new incrementation is set to true, all others are set to false.
        // If it is set to false, it just set the previousBool to false to be 
        // sure that it fit with the corresponding increment bool.
        // (halfNoteX and previousHalfNoteX)
        private void SwitchBoolean(ref bool newBool,
                                   ref bool previousBool,
                                   bool changingX)
        {
            if (newBool == true)
            {      
                if (changingX)
                {
                    halfNoteX = false;
                    eightNoteX = false;
                    sixteenNoteX = false;
                    previousHalfNoteX = false;
                    previouseightNoteX = false;
                    previoussixteenNoteX = false;
                }
                else
                {
                    halfNoteY = false;
                    eightNoteY = false;
                    sixteenNoteY = false;
                    previousHalfNoteY = false;
                    previousEightNoteY = false;
                    previousSixteenNoteY = false;
                }

                previousBool = true;
                newBool = true;
                boolChange = false;
            }
            else
            {
                newBool = false;
                previousBool = false;
                boolChange = false;
            }
            return;
        }

        // SWITCH MAIN FUNCTION
        // Verify for which boolean there is a difference between it and 
        // its last state.
        // For example if halfNoteX != previousHalfNoteX.
        // Then it will launch the "SwitchBoolean()" function, depending on
        // which value has been changed.
        private void SwitchMainFunction()
        {   
            if (boolChange)
            {
                Debug.Log ("Bool change blocked");
                return;
            }
            Debug.Log ("Bool change passed");
            if (halfNoteX != previousHalfNoteX)
            {   
                boolChange = true;
                SwitchBoolean(ref halfNoteX, ref previousHalfNoteX, true);
                return;
            }
            else if (eightNoteX != previouseightNoteX)
            {   
                Debug.Log("switch1 passed");
                boolChange = true;
                SwitchBoolean(ref eightNoteX, ref previouseightNoteX, true);
                return;
            }
            else if (sixteenNoteX != previoussixteenNoteX)
            {
                Debug.Log("switch2 passed");
                boolChange = true;
                SwitchBoolean(ref sixteenNoteX, ref previoussixteenNoteX, true);
                return;
            }

            if (halfNoteY != previousHalfNoteY)
            {
                boolChange = true;
                SwitchBoolean(ref halfNoteY, ref previousHalfNoteY, false);
                return;
            }
            else if (eightNoteY != previousEightNoteY)
            {   
                Debug.Log("switch1 passed");
                boolChange = true;
                SwitchBoolean(ref eightNoteY, ref previousEightNoteY, false);
                return;
            }
            else if (sixteenNoteY != previousSixteenNoteY)
            {
                Debug.Log("switch2 passed");
                boolChange = true;
                SwitchBoolean(ref sixteenNoteY, ref previousSixteenNoteY, false);
                return;
            }
        }
        
        // Called each time the script is loaded or a value change in the 
        // inspector.
        private void OnValidate()
        {
            archiveObject = GameObject.FindGameObjectWithTag("Archive");

            SwitchMainFunction();

        }
    #endregion
}