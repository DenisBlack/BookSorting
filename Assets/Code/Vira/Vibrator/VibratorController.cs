using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VIRA.Vibrator
{
    /// <summary>
    /// Sample of vibrator usage
    /// </summary>
    public class VibratorController : MonoBehaviour
    {
        [SerializeField] private Vibrator vibrator = default;
        [SerializeField] private TextMeshProUGUI console = default;
        [SerializeField] private TMP_InputField timingsField = default;
        [SerializeField] private Toggle repeat = default;
        [SerializeField] private Button play = default;
        [SerializeField] private Button cancel = default;

        private long[] timings;
        private int parseErrors;

        private void Start()
        {
            string msg = "";
            msg += $"[DEVICE] Type: {SystemInfo.deviceType}";
            msg += $"\n[DEVICE] Model: {SystemInfo.deviceModel}";
            msg += $"\n[DEVICE] Name: {SystemInfo.deviceName}";
            msg += $"\n[Vibrator] HasVibrator: {vibrator.HasVibrator()}";
            msg += $"\n[Vibrator] HasAmplitudeControl: {vibrator.HasAmplitudeControl()}";
            msg += $"\n[Vibrator] IsVibrationEffectsSupported: {vibrator.IsVibrationEffectsSupported()}";
            msg += $"\n[Vibrator] IsPredefinedEffectsSupported: {vibrator.IsPredefinedEffectsSupported()}";
            msg += $"\n[Vibrator] IsCompositionsSupported: {vibrator.IsCompositionsSupported()}";
            VibrationEffectSupport[] effectsSupport = vibrator.EffectSupported();
            msg += $"\n[Vibrator] Effects support:";
            if (effectsSupport != null)
            {
                msg += $"\n\t {VibrationEffect.EFFECT_CLICK} \t| {effectsSupport[0]}";
                msg += $"\n\t {VibrationEffect.EFFECT_DOUBLE_CLICK} \t| {effectsSupport[1]}";
                msg += $"\n\t {VibrationEffect.EFFECT_TICK} \t| {effectsSupport[2]}";
                msg += $"\n\t {VibrationEffect.EFFECT_HEAVY_CLICK} \t| {effectsSupport[3]}";
            }
            else
            {
                msg += "\n\tNONE";
            }
            bool[] primitivesSupport = vibrator.PrimitivesSupported();
            msg += $"\n[Vibrator] Primitives support:";
            if (primitivesSupport != null)
            {
                msg += $"\n\t {CompositionPrimitiveId.PRIMITIVE_CLICK} \t| {primitivesSupport[0]}";
                msg += $"\n\t {CompositionPrimitiveId.PRIMITIVE_QUICK_RISE} \t| {primitivesSupport[1]}";
                msg += $"\n\t {CompositionPrimitiveId.PRIMITIVE_SLOW_RISE} \t| {primitivesSupport[2]}";
                msg += $"\n\t {CompositionPrimitiveId.PRIMITIVE_QUICK_FALL} \t| {primitivesSupport[3]}";
                msg += $"\n\t {CompositionPrimitiveId.PRIMITIVE_TICK} \t| {primitivesSupport[4]}";
            }
            else
            {
                msg += "\n\tNONE";
            }
            console.text = msg;

            timingsField.onEndEdit.AddListener(TimingsEditHandler);
            play.onClick.AddListener(TestWaveForm);
            cancel.onClick.AddListener(CancelHandler);
        }

        private void OnDestroy()
        {
            timingsField.onEndEdit.RemoveListener(TimingsEditHandler);
            play.onClick.RemoveListener(TestWaveForm);
            cancel.onClick.RemoveListener(CancelHandler);
        }

        public void TestVibrate()
        {

        }

        public void TestEffect()
        {

        }

        public void TestWaveForm()
        {
            if (parseErrors > 0)
            {
                console.text += $"\n[ERROR] Can't play having {parseErrors} parse errors";
                return;
            }
            vibrator.VibrateWaveform(timings, repeat.isOn ? 0 : -1);
        }

        public void TestComposition()
        {

        }

        private void CancelHandler()
        {
            vibrator.Cancel();
        }

        private void TimingsEditHandler(string value)
        {
            parseErrors = 0;
            string[] values = value.Split(',');
            timings = new long[values.Length];
            int timing;
            for (int i = 0; i < timings.Length; i++)
            {
                if (!int.TryParse(values[i], out timing))
                {
                    console.text += $"\n[ERROR] Can't parse {values[i]}";
                    parseErrors++;
                    continue;
                }
                timings[i] = timing;
            }
        }
    }
}