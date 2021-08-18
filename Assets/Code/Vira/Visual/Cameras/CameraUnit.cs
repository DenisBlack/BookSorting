using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace VIRA.Visual.Cameras
{

    public enum CameraTypes
    {
        mainCamera,
        other
    }

    [System.Serializable]
    public class CameraAnimSettings
    {
        public float moveTime;
        public float rotationTime;
        public Ease moveEase;
        public Ease rotationEase;
        public bool hasRotationData = false;

        public CameraAnimSettings(float _moveTime, Ease _moveEase)
        {
            moveEase = _moveEase;
            moveTime = _moveTime;
        }

        public CameraAnimSettings(float _moveTime, Ease _moveEase, float _rotationTime, Ease _rotationEase)
        {
            moveEase = _moveEase;
            moveTime = _moveTime;
            hasRotationData = true;
            rotationEase = _rotationEase;
            rotationTime = _rotationTime;
        }
    }
    public class CameraUnit : MonoBehaviour
    {
        
        
        private Transform _transform;

        public CameraTypes CameraType => _cameraType;
        
        //private Sequence _seq;

        [Header("DefaultSetUp")]
        [SerializeField] Transform _defaultTransformPivot;
        [SerializeField] private CameraTypes _cameraType = CameraTypes.other;
#if UNITY_EDITOR
        [ReadOnly] public int ID = 0;

#endif



        public Camera CameraComponent { get; private set; }

        public int Id { get; private set; }

        
        private void Awake()
        {
            Id = name.GetHashCode();
#if UNITY_EDITOR

            ID = Id;

#endif
            _transform = transform;
            CameraComponent = GetComponentInChildren<Camera>();
            CamerasManager.Instance.AddCameraUnit(this);
            //_seq = DOTween.Sequence();

        }

        private void OnValidate()
        {
            Id = name.GetHashCode();
#if UNITY_EDITOR

            ID = Id;

#endif
        }

        public void MoveCameraToTransform(Transform target, float time)
        {
            _transform.DOKill();
            _transform.DOMove(target.position, time);

            _transform.DORotate((target.rotation).eulerAngles, time);
        }
        public void MoveCameraToTransform(Transform target, CameraAnimSettings animSettings)
        {
            _transform.DOKill();
            _transform.DOMove(target.position, animSettings.moveTime).SetEase(animSettings.moveEase);
            if (animSettings.hasRotationData)
            {
                _transform.DORotate((target.rotation).eulerAngles, animSettings.rotationTime).SetEase(animSettings.rotationEase);
            }
            
        }

        public void ResetTransform()
        {
            if (_defaultTransformPivot)
            {
                _transform.DOKill();
                _transform.position = _defaultTransformPivot.position;
                _transform.rotation = _defaultTransformPivot.rotation;
            }
            
        }



    }
}

