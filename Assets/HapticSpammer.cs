using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR;

public class HapticSpammer : MonoBehaviour
{
    private PCMHaptics pcmHaptics;
    private float sampleRate = 0;
    private PCMHaptics.ControllerHand controller = PCMHaptics.ControllerHand.Left;
    public InputActionReference m_CycleWaveform;
    public AudioClip[] m_Waveforms;
    private int currentWaveform;

    private void Awake()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        if (pcmHaptics == null)
            pcmHaptics ??= OpenXRSettings.Instance.GetFeature<PCMHapticsFeature>()?.InitializePCMHaptics();
        if (pcmHaptics == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Init());
        }
        else Debug.Log("haptics initialized yay");

        StartCoroutine(GetSampleRate());
    }

    IEnumerator GetSampleRate()
    {
        if (pcmHaptics != null && sampleRate == 0)
        {
            sampleRate = pcmHaptics.GetControllerSampleRateHz(controller);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(GetSampleRate());
        }
        else Debug.Log($"got sample rate of {sampleRate}");
        
        yield return null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWaveform = 0;
        m_CycleWaveform.action.performed += (ctx) => { currentWaveform = (currentWaveform + 1) % m_Waveforms.Length; };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
