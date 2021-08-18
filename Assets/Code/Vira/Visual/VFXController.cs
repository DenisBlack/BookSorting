using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VIRA.Visual
{

    public class VFXController : MonoBehaviour
    {
        public Transform VFXPullHolder;
        public VFX[] VFXs;
        public Gradient DefaultGradient;

        private List<VFX> vfxPull;

        private Gradient currentGradient;

        public void Start()
        {
            currentGradient = DefaultGradient;

            vfxPull = new List<VFX>();
            VFX[] vfxC = VFXPullHolder.GetComponentsInChildren<VFX>();
            foreach (VFX v in vfxC)
            {
                vfxPull.Add(v);
                v.gameObject.SetActive(false);
            }

        }
        public void SetColor(Gradient gradient)
        {
            currentGradient = gradient;

            foreach (var vfx in vfxPull)
            {
                vfx.SetGradient(currentGradient);
            }
        }

        public void SetColor(Color color)
        {
            Gradient gradient = new Gradient();

            GradientColorKey[] c = { new GradientColorKey(color, 0) };
            GradientAlphaKey[] a = { new GradientAlphaKey(1, 0) };

            gradient.SetKeys(c, a);

            currentGradient = gradient;

            foreach (var vfx in vfxPull)
            {
                vfx.SetGradient(currentGradient);
            }
        }

        public void SetVFX(Vector3 _position, VFXtypes.VFXTypes type)
        {
            bool found = false;
            foreach (VFX vfx in vfxPull)
            {
                vfx.SetGradient(currentGradient);

                if (vfx.type == type)
                {
                    if (!vfx.inUse)
                    {
                        found = true;
                        vfx.transform.position = _position;



                        vfx.gameObject.SetActive(true);
                        vfx.inUse = true;
                        vfx.play();
                        StartCoroutine(active(vfx));
                        return;
                    }
                }
            }

            if (!found)
            {
                VFX vfx = createVFXPool(type);
                if (vfx != null)
                {
                    vfx.SetGradient(currentGradient);
                    vfx.transform.position = _position;
                    vfx.gameObject.SetActive(true);
                    vfx.play();
                    vfx.inUse = true;
                    StartCoroutine(active(vfx));
                }

            }
        }
        IEnumerator active(VFX vfx)
        {
            yield return new WaitForSeconds(vfx.duration);
            vfxReset(vfx);
        }
        public void vfxReset(VFX vfx)
        {
            vfx.transform.position = Vector3.zero - Vector3.forward * -10;
            vfx.ResetVFX();
        }
        public VFX createVFXPool(VFXtypes.VFXTypes type)
        {
            GameObject VFXg;
            foreach (VFX vfx in VFXs)
            {
                if (vfx.type == type)
                {
                    VFXg = GameObject.Instantiate(vfx.gameObject, VFXPullHolder);
                    VFXg.SetActive(false);
                    VFX vfxI = VFXg.GetComponent<VFX>();
                    vfxPull.Add(vfxI);
                    return vfxI;
                }
            }
            Debug.LogError("VFX Controller doesn`t hold prefab of type: " + type);
            return null;
        }
    }

}



