using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VIRA.Visual
{

    public class VFX : MonoBehaviour
    {

        public VFXtypes.VFXTypes type;
        public bool inUse = false;
        public float duration;
        public GameObject VFXI;
        public ParticleSystem[] PS;
        public ParticleSystem[] paintable;

        private void Start()
        {

        }
        public void play()
        {
            gameObject.SetActive(true);
            foreach (ParticleSystem p in PS)
            {
                p.Play();
            }

        }
        public void SetColor(Gradient color)
        {
            foreach (ParticleSystem p in paintable)
            {
                var main = p.main;
                main.startColor = color;
            }



        }

        public void SetGradient(Gradient _color)
        {
            foreach (ParticleSystem p in paintable)
            {
                var main = p.main;
                var st = new ParticleSystem.MinMaxGradient(_color);
                st.mode = ParticleSystemGradientMode.RandomColor;
                main.startColor = st;




            }
        }

        public void ResetVFX()
        {
            gameObject.SetActive(false);
            inUse = false;
        }

    }
}
