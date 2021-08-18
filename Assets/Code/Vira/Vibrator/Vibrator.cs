using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

namespace VIRA.Vibrator
{
    /// <summary>
    /// Vibrator plugin wrapper
    /// </summary>
    public class Vibrator : MonoBehaviour
    {
        //TODO Create native dll using c++
        AndroidJavaObject vibrator;
        private bool _hasVibrator;
        public static bool canVibrate = true;

        private void Awake()
        {
            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject unityPlayerActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
#else
            AndroidJavaObject unityPlayerActivity = null;
#endif
            Debug.Log("[Vibrator] Creating vibrator object");
            vibrator = new AndroidJavaObject("com.example.vibratecontroller.VibrateController", unityPlayerActivity);
            _hasVibrator = vibrator.Call<bool>("HasVibrator");
        }

        private void OnDestroy()
        {
            vibrator.Dispose();
        }

        public bool HasVibrator()
        {

            return _hasVibrator && canVibrate;
        }

        public bool HasAmplitudeControl()
        {
            return vibrator.Call<bool>("HasAmplitudeControl");
        }

        public bool IsVibrationEffectsSupported()
        {
            return vibrator.Call<bool>("IsVibrationEffectsSupported");
        }

        public bool IsPredefinedEffectsSupported()
        {
            return vibrator.Call<bool>("IsPredefinedEffectsSupported");
        }

        public bool IsCompositionsSupported()
        {
            return vibrator.Call<bool>("IsCompositionsSupported");
        }

        public VibrationEffectSupport[] EffectSupported()
        {
            int[] input = vibrator.Call<int[]>("EffectSupported");
            if (input == null) return null;
            return input.Cast<VibrationEffectSupport>().ToArray();
        }

        public bool[] PrimitivesSupported()
        {
            return vibrator.Call<bool[]>("PrimitivesSupported");
        }

        public void Vibrate(long duration, int amplitude)
        {
            vibrator.Call("Vibrate", duration, amplitude);
        }

        public void VibratePredefined(int effectId)
        {
            vibrator.Call("VibratePredefined", effectId);
        }

        public void VibrateWaveform(long[] timings, int[] amplitudes, int repeat)
        {
            vibrator.Call("VibrateWaveform", timings, amplitudes, repeat);
        }

        public void VibrateWaveform(long[] timings, int repeat)
        {
            vibrator.Call("VibrateWaveform", timings, repeat);
        }

        public void VibrateComposition(int[] primitiveIds)
        {
            vibrator.Call("VibrateComposition", primitiveIds);
        }

        public void Cancel()
        {
            vibrator.Call("Cancel");
        }
    }
}