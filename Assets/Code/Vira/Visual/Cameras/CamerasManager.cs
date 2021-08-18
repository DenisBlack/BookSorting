using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VIRA.Visual.Cameras
{
    public class CamerasManager : MonoBehaviourSingleton<CamerasManager>
    {
        private List<CameraUnit> _cameras = new List<CameraUnit>();




        public CameraUnit GetCameraById(int id)
        {
            foreach(CameraUnit camera in _cameras)
            {
                if(camera.Id == id)
                {
                    return camera;
                }

            }

            return null;
        }

        public CameraUnit GetCameraByType(CameraTypes type)
        {
            foreach(CameraUnit cam in _cameras)
            {
                if(cam.CameraType == type)
                {
                    return cam;
                }
            }

            return null;
        }

        public void AddCameraUnit(CameraUnit cameraUnit)
        {
            _cameras.Add(cameraUnit);
        }

    }
}

