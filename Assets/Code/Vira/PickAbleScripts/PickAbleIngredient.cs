using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAbleIngredient : IPickUpAbleItem
{
    [SerializeField] VIRA.Animations.DotweenAppendSequence _animator;
    private void Awake()
    {
    }

    public override bool PickUp()
    {
        bool b = base.PickUp();
        _animator.PlayAnim(0);

        return b;
    }
}