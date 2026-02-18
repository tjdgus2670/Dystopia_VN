using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("연결할 것들")]
    public GameObject borderImage; // 테두리 이미지
    public AudioSource audioSource; // 소리 재생기 (UI_Sound_Player)

    [Header("효과음 파일")]
    public AudioClip hoverSound; // 마우스 올릴 때 (띠링)
    public AudioClip clickSound; // 클릭할 때 (찰칵)

    [Header("반응 속도 조절 (딜레이 해결)")]
    [Range(0f, 0.5f)]
    public float startOffset = 0.05f; // ★ 0.05초만큼 앞부분을 무시하고 재생!

    // 1. 키보드로 선택됐을 때
    public void OnSelect(BaseEventData eventData)
    {
        ShowBorder(true);
        PlaySound(hoverSound);
    }

    // 2. 다른 걸로 넘어갔을 때
    public void OnDeselect(BaseEventData eventData)
    {
        ShowBorder(false);
    }

    // 3. 마우스가 올라갔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스를 올리면 그 버튼을 '선택된 상태'로 만듦
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    // 4. 마우스가 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        ShowBorder(false);
    }

    // 5. 클릭했을 때
    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickSound);
    }

    // 테두리 켜고 끄는 함수
    void ShowBorder(bool show)
    {
        if (borderImage != null) borderImage.SetActive(show);
    }

    // ★ 핵심 수정: 소리 재생 함수
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            // PlayOneShot 대신 일반 Play를 사용해야 시작 시간(time)을 조절할 수 있습니다.
            audioSource.Stop(); // 혹시 재생 중이던 소리가 있다면 끊고 (깔끔하게)
            audioSource.clip = clip; // 클립을 갈아끼운 뒤
            
            // 파일 길이를 넘지 않는 선에서 시작 위치 잡기 (안전장치)
            float safeStart = Mathf.Min(startOffset, clip.length - 0.01f);
            if (safeStart < 0) safeStart = 0;

            audioSource.time = safeStart; // ★ 여기서 앞부분 공백을 건너뜁니다!
            audioSource.Play(); // 재생
        }
    }
}