using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class StoryEffectController : MonoBehaviour
{
    public AudioSource audioSource; // 1번 스피커 (효과음용: 띠링, 총소리)
    public AudioSource bgmSource;   // 2번 스피커 (배경음용: 빗소리, 음악) - 새로 추가!
    public Image flashPanel;        // 번쩍이는 화면

    // 1. 효과음 (기존과 동일)
    [YarnCommand("play_sfx")]
    public void PlaySFX(string soundName)
    {
        string path = "Sounds/" + soundName;
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip != null) audioSource.PlayOneShot(clip);
        else Debug.LogError("⚠️ 효과음 파일 없음: " + path);
    }

    // 2. [New] 서서히 켜지는 배경음악 (Fade In)
    [YarnCommand("play_bgm_fade")]
    public void PlayBGMFade(string soundName)
    {
        StartCoroutine(FadeInRoutine(soundName));
    }

    IEnumerator FadeInRoutine(string soundName)
    {
        string path = "Sounds/" + soundName;
        AudioClip clip = Resources.Load<AudioClip>(path);

        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true; // 빗소리는 계속 내리기 (무한반복)
            bgmSource.volume = 0f; // 일단 소리 0으로 시작
            bgmSource.Play();

            // 3초 동안 서서히 소리 키우기
            float duration = 3.0f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(0f, 1f, elapsed / duration);
                yield return null;
            }
            bgmSource.volume = 1f; // 3초 뒤엔 최대 볼륨 고정
        }
        else
        {
            Debug.LogError("⚠️ 배경음 파일 없음: " + path);
        }
    }

    // 3. 화면 번쩍임 (기존과 동일)
    [YarnCommand("flash")]
    public void FlashScreen()
    {
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        flashPanel.color = new Color(1, 1, 1, 1);
        flashPanel.gameObject.SetActive(true);
        float duration = 0.5f;
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            flashPanel.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, elapsed / duration));
            yield return null;
        }
        flashPanel.gameObject.SetActive(false);
    }
}