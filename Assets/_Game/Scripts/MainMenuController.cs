using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [Header("설정")]
    public GameObject firstButton; // 처음에 선택될 버튼
    public GameObject settingsPanel; // ★ 설정 창 패널 (이따가 연결할 것!)

    void Start()
    {
        // 시작 시 '새 게임' 버튼 선택
        if (firstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }

    // 1. 새 게임 시작
    public SceneFader sceneFader; 

    public void OnClickNewGame()
    {
        // 바로 이동하는 게 아니라, 페이드 효과에게 부탁함
        if (sceneFader != null)
        {
            sceneFader.FadeTo("Chapter1_scene"); 
        }
        else
        {
            // 페이더가 없으면 그냥 이동 (안전장치)
            SceneManager.LoadScene("Chapter1_scene");
        }
    }

    // 2. 이어하기 (일단 로그만)
    public void OnClickLoad()
    {
        Debug.Log("아직 저장된 데이터가 없습니다! (나중에 구현 예정)");
        // 나중에 여기에 세이브 파일 로드 기능을 넣을 거예요.
    }

    // 3. 설정 창 열기
    public void OnClickSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); // 패널 켜기
            
            // ★ 중요: 설정 창이 열리면, 설정 창 안의 '닫기' 버튼 같은 걸로 포커스를 옮겨줘야 키보드 조작이 안 꼬입니다.
            // (일단은 패널만 켜는 걸로 할게요)
        }
    }

    // ★ 3-1. 설정 창 닫기 (새로 추가됨)
    public void OnClickSettingsClose()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // 패널 끄기
            
            // 설정 창 닫으면 다시 메인 메뉴 버튼(설정 버튼 등)을 선택해줘야 함
            EventSystem.current.SetSelectedGameObject(firstButton); 
        }
    }

    // 4. 게임 종료
    public void OnClickExit()
    {
        Debug.Log("게임 종료!");
        
        // 에디터에서는 게임이 안 꺼지므로, 강제로 꺼지는 척하는 코드
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // 실제 빌드된 게임에서는 이게 작동
        #endif
    }
}