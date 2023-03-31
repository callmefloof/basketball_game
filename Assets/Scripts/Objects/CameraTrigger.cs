    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class CameraTrigger : MonoBehaviour
    {
        
        public Camera mainCamera;
        public Camera subCameraRed;
        public Camera subCameraBlue;
        public List<GameObject> ballers;
        
        private bool isCameraOneActive = true;

        void Start()
        {
            //Make sure the main camera is the one we start with
            mainCamera.gameObject.SetActive(true);
            subCameraRed.gameObject.SetActive(false);
            subCameraBlue.gameObject.SetActive(false);
            //Create a list to hold all the ballers for future reference and calculation
            ballers.Add(GameObject.Find("BALLER  (1)"));
            ballers.Add(GameObject.Find("BALLER  (2)"));
            ballers.Add(GameObject.Find("BALLER  (3)"));
            ballers.Add(GameObject.Find("BALLER  (4)"));
        }
        
        void Update()
        {
            //Switch to Camera Red if in Zone Red
            if (IsInZoneRed())
            {
                SwitchCameraRed();
            }
            //Switch back to main if not in Zone Red
             if (!IsInZoneRed())
            {
                SwitchCameraBack();
            }
            if (IsInZoneBlue())
            {
                SwitchCameraBlue();
            }
            if (!IsInZoneBlue())

            {
                SwitchCameraBack();
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
