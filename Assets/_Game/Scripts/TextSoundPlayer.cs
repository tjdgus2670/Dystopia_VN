using UnityEngine;
using TMPro;

public class TextSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    
    [Header("소리 파일")]
    public AudioClip soundBlip;
    public AudioClip soundMessage;

    [Header("설정")]
    public TextMeshProUGUI nameText; 
    
    [Range(1, 5)] 
    public int soundInterval = 2; // 몇 글자마다 소리 낼지

    [Range(0f, 0.2f)]
    public float startOffset = 0.05f; // 소리 앞부분 공백 스킵

    private TextMeshProUGUI myText; 
    private int lastVisibleCount = 0;

    void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();

        // ★ 핵심 수정: 시작하자마자 현재 글자 수를 미리 저장해버립니다.
        // 이렇게 하면 첫 프레임에서 "글자가 늘어났다"고 착각하지 않습니다.
        if (myText != null)
        {
            lastVisibleCount = myText.maxVisibleCharacters;
        }
    }

    void Update()
    {
        if (myText == null) return;

        int currentVisibleCount = myText.maxVisibleCharacters;
        int totalCharacterCount = myText.textInfo.characterCount;

        // 1. 글자가 늘어날 때 (타자 치는 중)
        if (currentVisibleCount > lastVisibleCount)
        {
            if (currentVisibleCount % soundInterval == 0) 
            {
                PlayTypingSound();
            }
        }
        
        // 2. ★ 핵심: 글자 출력이 모두 끝났다면 소리를 즉시 정지
        // 현재 보이는 글자 수가 전체 글자 수와 같아지는 순간 소리를 끕니다.
        if (currentVisibleCount >= totalCharacterCount && totalCharacterCount > 0)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop(); 
            }
        }
        
        lastVisibleCount = currentVisibleCount;
    }

    void PlayTypingSound()
    {
        string currentName = "";
        if (nameText != null) currentName = nameText.text;

        if (currentName == "System" || currentName == "Unknown" || currentName == "시스템") 
        {
            audioSource.clip = soundBlip;
            audioSource.pitch = Random.Range(0.95f, 1.05f); 
        }
        else if (currentName == "Gang" || currentName == "조직원")
        {
            audioSource.clip = soundMessage;
            audioSource.pitch = Random.Range(0.6f, 0.7f);
        }
        else
        {
            audioSource.clip = soundMessage;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
        }

        audioSource.Stop(); 
        audioSource.time = startOffset; 
        audioSource.Play();
    }
}