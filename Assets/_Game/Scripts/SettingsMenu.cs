using UnityEngine;
using UnityEngine.Audio; // 오디오 믹서 쓰려면 필수!
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("연결 요소")]
    public AudioMixer mainMixer; // 아까 만든 믹서기
    public Slider volumeSlider;  // 볼륨 슬라이더
    public Toggle fullscreenToggle; // 전체화면 토글

    void Start()
    {
        // 1. 현재 소리 크기를 슬라이더에 반영 (저장된 값이 없으면 기본값)
        float currentVolume;
        mainMixer.GetFloat("MasterVolume", out currentVolume);
        volumeSlider.value = currentVolume;

        // 2. 현재 전체화면 여부를 토글에 반영
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    // 🔊 볼륨 조절 함수 (슬라이더가 움직일 때마다 실행됨)
    public void SetVolume(float volume)
    {
        // 믹서의 "MasterVolume" 값을 변경합니다.
        mainMixer.SetFloat("MasterVolume", volume);
    }

    // 🖥️ 전체화면 조절 함수 (토글을 누를 때마다 실행됨)
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}