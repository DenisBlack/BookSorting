using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VIRA.InputSystem
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField, HideInInspector] private new SpriteRenderer renderer = default;
        [SerializeField, HideInInspector] private new BoxCollider2D collider = default;
        [SerializeField, HideInInspector] private new Camera camera = default;

        private void OnValidate()
        {
            renderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<BoxCollider2D>();
            camera = GetComponentInParent<Camera>();
        }

        private void Awake()
        {
            AdjustBackground();
        }

        [ContextMenu("Adjust Background")]
        private void AdjustBackground()
        {
            float height = CalculateHeight();
            AdjustRenderer(height);
            AdjustCollider(height);
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(renderer);
            EditorUtility.SetDirty(collider);
            EditorUtility.SetDirty(transform);
            EditorUtility.SetDirty(gameObject);
#endif
        }

        private float CalculateHeight()
        {
            if (camera == null) return 0;
            transform.localPosition = new Vector3(0f, 0f, camera.farClipPlane - 1f);
            return 2f * Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView * 0.5f) * transform.localPosition.z;
        }

        private void AdjustRenderer(float height)
        {
            if (renderer == null) return;
            renderer.size = new Vector2(camera.aspect * height, height);
        }

        private void AdjustCollider(float height)
        {
            if (collider == null) return;
            collider.size = new Vector2(camera.aspect * height, height);
        }
    }
}