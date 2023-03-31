    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class CameraTrigger : MonoBehaviour
    {
        //Camera objects to switch between
        public Camera mainCamera;
        public Camera subCameraRed;
        public Camera subCameraBlue;
        
            //List of baller game objects to track for camera positioning 
        public List<GameObject> ballers;
        
        //Start is called before the first frame update 
        void Start()
        {
            //Make sure the main camera is the one we start with
            mainCamera.gameObject.SetActive(true);
            subCameraRed.gameObject.SetActive(false);
            subCameraBlue.gameObject.SetActive(false);
            
            //Add the ballers to a  list to hold all the ballers for future reference and calculation
            ballers.Add(GameObject.Find("BALLER  (1)"));
            ballers.Add(GameObject.Find("BALLER  (2)"));
            ballers.Add(GameObject.Find("BALLER  (3)"));
            ballers.Add(GameObject.Find("BALLER  (4)"));
        }
        
        //Update is called once per frame
        void Update()
        {
            //Switch between cameras based on current camera state
            switch (GetCameraState())
            {
                case CameraState.Red:
                    SwitchCameraRed();
                    break;
                case CameraState.Blue:
                    SwitchCameraBlue();
                    break;
                default:
                    SwitchCameraBack();
                    break;
            }
        }
        
        //Enumeration to track camera states
        enum CameraState
        {
            Red,
            Blue,
            Main
        }
        
        //Get the current camera state based on which zone the ballers are in
        private CameraState GetCameraState()
        {
            if (IsInZoneRed())
            {
                return CameraState.Red;
            }
            else if (IsInZoneBlue())
            {
                return CameraState.Blue;
            }
            else
            {
                return CameraState.Main;
            }
        }

        private bool IsInZoneRed()
        {
            //Create a integer to hold the number of ballers within the red zone
            int numBallersInZone = 0;
            
            //For each function which will check through all the ballers colliding with the zone, in order to see how many ballers are in zone
            foreach (GameObject baller in ballers)
            {
                if (baller.GetComponent<Collider>().bounds.Intersects(GameObject.FindGameObjectWithTag("CamZone1").GetComponent<Collider>().bounds))
                {
                    numBallersInZone++;
                }
            }

            //Check if 3 or more players are in the zone
            if (numBallersInZone >= 3)
            {
                return true;
            }

            return false;
        }
        
        private bool IsInZoneBlue()
        {
            //Create a integer to hold the number of ballers within the blue zone
            int numBallersInZone = 0;
            
            //For each function which will check through all the ballers colliding with the zone, in order to see how many ballers are in zone
            foreach (GameObject baller in ballers)
            {
                if (baller.GetComponent<Collider>().bounds.Intersects(GameObject.FindGameObjectWithTag("CamZone2").GetComponent<Collider>().bounds))
                {
                    numBallersInZone++;
                }
            }

            //Check if 3 or more players are in the zone
            if (numBallersInZone >= 3)
            {
                return true;
            }

            return false;
        }

        //Functions to switch the cameras
        private void SwitchCameraRed()
        {
            //Switch the camera from main to red side
            mainCamera.gameObject.SetActive(false);
            subCameraRed.gameObject.SetActive(true);
            subCameraBlue.gameObject.SetActive(false);
        }
        private void SwitchCameraBlue()
        {
            //Switch the camera from main to blue side 
            mainCamera.gameObject.SetActive(false);
            subCameraRed.gameObject.SetActive(false);
            subCameraBlue.gameObject.SetActive(true);
        }

        private void SwitchCameraBack()
        {
            //Switch the camera back to main 
            mainCamera.gameObject.SetActive(true);
            subCameraRed.gameObject.SetActive(false);
            subCameraBlue.gameObject.SetActive(false);
            
        }
        
    }
