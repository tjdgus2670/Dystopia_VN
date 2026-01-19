using System.Collections;
using UnityEngine;
using TMPro;            // 텍스트(TMP) 제어용
using Yarn.Unity;       // Yarn Spinner 연동용

public class KeypadController : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject puzzlePanel;      // 퍼즐 전체 패널
    public TMP_InputField inputField;   // 비밀번호 입력창
    
    // 내부 상태 변수
    private string correctPassword;     
    private bool isPuzzleActive = false; 

    // [수정] Awake 대신 Start 사용
    void Start()
    {
        // DialogueRunner를 찾아서 안전하게 명령어 등록
        var runner = FindObjectOfType<DialogueRunner>();
        
        if (runner != null)
        {
            runner.AddCommandHandler<string>("start_hack", StartHackRoutine);
            Debug.Log("[System] KeypadController 명령어(start_hack) 등록 완료");
        }
        else
        {
            Debug.LogError("[Error] DialogueRunner를 찾을 수 없습니다. KeypadController가 작동하지 않습니다.");
        }
    }

    // Yarn이 이 함수가 끝날 때까지 대기를 타게 만드는 Coroutine
    private IEnumerator StartHackRoutine(string password)
    {
        Debug.Log($"[System] 해킹 시작. 목표 비밀번호: {password}");
        
        // 1. 초기화
        correctPassword = password;
        inputField.text = "";           
        puzzlePanel.SetActive(true);    // UI 켜기
        isPuzzleActive = true;

        // 2. 대기 (플레이어가 퍼즐을 풀거나 닫을 때까지 무한 루프)
        while (isPuzzleActive)
        {
            yield return null; // 한 프레임 대기
        }

        // 3. 종료 (루프를 빠져나오면 UI 끄기)
        puzzlePanel.SetActive(false);
        Debug.Log("[System] 해킹 종료");
    }

    // 제출 버튼에 연결할 함수
    public void CheckCode()
    {
        if (inputField.text == correctPassword)
        {
            Debug.Log("해킹 성공!");
            // Yarn에 성공했다는 변수를 저장 ($hack_success = true)
            FindObjectOfType<InMemoryVariableStorage>().SetValue("$hack_success", true);
            isPuzzleActive = false; // 루프 종료 -> 대화 재개
        }
        else
        {
            Debug.Log("비밀번호 불일치!");
            inputField.text = ""; // 틀리면 지우기
        }
    }

    // 닫기 버튼에 연결할 함수
    public void ClosePuzzle()
    {
        Debug.Log("해킹 취소");
        FindObjectOfType<InMemoryVariableStorage>().SetValue("$hack_success", false);
        isPuzzleActive = false; // 루프 종료 -> 대화 재개
    }
}