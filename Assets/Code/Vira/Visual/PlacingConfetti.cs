using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VIRA.Visual
{


    public class PlacingConfetti : MonoBehaviour
    {
        public Camera camera;
        public GameObject left;
        public GameObject right;

        public float offset;
        // Start is called before the first frame update
        void OnEnable()
        {
            left.transform.position = camera.ScreenToWorldPoint(new Vector3(0, Screen.height - Screen.height / 5, camera.nearClipPlane));
            right.transform.position = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height - Screen.height / 7, camera.nearClipPlane));
            left.transform.position -= Vector3.right * offset;
            right.transform.position += Vector3.right * offset;
            right.transform.position += Vector3.forward * 5;
            left.transform.position += Vector3.forward * 5;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
