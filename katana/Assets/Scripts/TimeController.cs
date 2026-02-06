using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeController : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static TimeController instance;
    public static TimeController Instance => instance;

    [Header("Slow Motion Settings")]
    public float slowMotionTimeScale = 0.3f;   // 슬로우 모션 시 적용할 Time.timeScale 값
    public float slowMotionDuration = 0.5f;    // 슬로우 모션 지속 시간

    private float slowMotionTimer;              // 슬로우 모션 타이머
    public bool isSlowMotion { get; private set; } // 현재 슬로우 모션 상태 여부

    [Header("Post Processing (URP)")]
    public Volume volume;                       // URP Volume (Global Volume 권장)

    // 사용할 포스트 프로세싱 효과들
    private Vignette vignette;                  // 화면 가장자리 어둡게
    private ColorAdjustments colorAdjustments;  // 색감(채도, 대비, 노출 등)

    private void Awake()
    {
        // 싱글톤 초기화
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        // Volume Profile에서 효과 컴포넌트 가져오기
        // (Profile 안에 해당 효과가 반드시 있어야 함)
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out colorAdjustments);
    }

    void Update()
    {
        // 슬로우 모션 상태일 때 타이머 증가
        if (isSlowMotion)
        {
            // Time.timeScale 영향을 받지 않도록 unscaledDeltaTime 사용
            slowMotionTimer += Time.unscaledDeltaTime;

            // 지속 시간 초과 시 슬로우 모션 해제
            if (slowMotionTimer >= slowMotionDuration)
            {
                SetSlowMotion(false);
            }
        }
    }

    // 외부에서 현재 적용될 타임 스케일을 가져올 때 사용
    public float GetTimeScale()
    {
        return isSlowMotion ? slowMotionTimeScale : 1f;
    }

    // 슬로우 모션 ON / OFF 제어 함수
    public void SetSlowMotion(bool slow)
    {
        isSlowMotion = slow;

        if (slow)
        {
            // 슬로우 모션 시작
            slowMotionTimer = 0f;

            // 시간 속도 조절
            Time.timeScale = slowMotionTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            // 🎬 슬로우 모션 연출용 포스트 프로세싱
            vignette.intensity.value = 0.6f;      // 비네트 강하게

            colorAdjustments.saturation.value = -40f;   // 채도 감소 (무채색 느낌)
            colorAdjustments.contrast.value = 20f;      // 대비 증가
            colorAdjustments.postExposure.value = -1f;  // 전체 화면 어둡게
        }
        else
        {
            // 슬로우 모션 종료 → 정상 속도로 복귀
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            // 포스트 프로세싱 효과 원복
            vignette.intensity.value = 0f;

            colorAdjustments.saturation.value = 0f;
            colorAdjustments.contrast.value = 0f;
            colorAdjustments.postExposure.value = 0f;
        }
    }
}