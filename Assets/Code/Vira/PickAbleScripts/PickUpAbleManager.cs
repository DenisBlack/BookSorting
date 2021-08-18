using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PickUpAbleManager : MonoBehaviour
{
    public event Action<Temp> OnIngredientClick = delegate { };

    [SerializeField] Camera _camera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);


            RaycastHit[] hits;

            hits = Physics.RaycastAll(ray, 1000, 1 << 8);
            IPickUpAbleItem picked;
            foreach (RaycastHit h in hits)
            {
                Temp type;
                if (h.transform.TryGetComponent<IPickUpAbleItem>(out picked) &&
                    picked.CheckPicked(out type))
                {
                    OnIngredientClick?.Invoke(type);
                }
            }
        }
    }
}