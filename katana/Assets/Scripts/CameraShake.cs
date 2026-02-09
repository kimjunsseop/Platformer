using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    CinemachinePositionComposer composer;

    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeStrength = 0.3f;
    [SerializeField] int shakeVibrato = 10;

    void Start()
    {
        composer = GetComponent<CinemachinePositionComposer>();
        Shake();
    }

    public void Shake()
    {
        Vector3 originalOffset = composer.TargetOffset;
        DOTween.Shake(
            () => composer.TargetOffset,
            x => composer.TargetOffset = x,
            shakeDuration, shakeStrength, shakeVibrato
        ).OnComplete(() => composer.TargetOffset = originalOffset);
    }
}