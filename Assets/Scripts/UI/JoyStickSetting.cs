using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// // // // //
using UnityEngine.UI;
using UnityEngine.EventSystems; // 이벤트 핸들러 사용 using

// 캐릭터 움직임 버튼 조이스틱
public class JoyStickSetting : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    // 이벤트 핸들러 enum값
    public enum eEventHandle { Click, Drag }
    public eEventHandle m_ePrevEvent;

    // ??
//    private RectTransform m_BackGround;

    // 조이스틱과 조이스틱 백그라운드 오브젝트
    public GameObject m_JoyStickBackGround;
    public GameObject m_JoyStick;

    private RectTransform m_TransJoyStickBackGround;
    private RectTransform m_TransJoyStick;

    // 포지션값 따로 저장하기 위함
    public Vector2 m_VecJoystickValue { get; private set; }
    public Vector3 m_VecJoyRotValue { get; private set; }

    // 조이스틱의 범위 계산 위한 반지름값
    private float m_fRadius;

    // 예시 상태값
    public enum ePlayerState { Idle, Attack, Move, End }
    public ePlayerState m_ePlayerState { get; private set; }

    // // // // //
    private Vector3 inputVector;
    public Image bgImg;    // 조이스틱 배경
    public Image joystickImg;  // 조이스틱

    private void Awake()
    {
        Init(); // 초기화
    }

    // 이벤트들
    #region event
    public void OnPointerClick(PointerEventData eventData)
    {
        SetPlayerState(ePlayerState.Idle);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CallJoyStick(eventData);
        SetHandleState(eEventHandle.Click);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // // // // //
        inputVector = Vector3.zero;

        m_JoyStickBackGround.SetActive(false);

        if(m_ePrevEvent == eEventHandle.Drag) {
            return;
        }

        SetPlayerState(ePlayerState.Attack);
        SetHandleState(eEventHandle.Click);
    }

    public void OnDrag(PointerEventData eventData)
    {
        JoyStickMove(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        JoyStickMoveEnd(eventData);
    }
    #endregion

    public void Init()
    {
        m_TransJoyStickBackGround = m_JoyStickBackGround.GetComponent<RectTransform>();
        m_TransJoyStick = m_JoyStick.GetComponent<RectTransform>();
        m_fRadius = m_TransJoyStickBackGround.rect.width * 0.5f;    // 조이스틱 행동반경 계산

        m_JoyStick.SetActive(true);
        m_JoyStickBackGround.SetActive(false);
    }

    private void JoyStickMoveEnd(PointerEventData eventData)
    {
        m_TransJoyStick.position = eventData.position;
        m_JoyStickBackGround.SetActive(false);

        SetHandleState(eEventHandle.Click);
        SetPlayerState(ePlayerState.Idle);
    }

    private void CallJoyStick(PointerEventData eventData)
    {
        m_JoyStickBackGround.transform.position = eventData.position;
        m_JoyStick.transform.position = eventData.position;
        m_JoyStickBackGround.SetActive(true);
    }

    private void JoyStickMove(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bgImg.rectTransform, eventData.position, eventData.pressEventCamera, out pos)) {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2, pos.y * 2, 0);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // 조이스틱 이동
            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x *
                (bgImg.rectTransform.sizeDelta.x / 3), inputVector.y * (bgImg.rectTransform.sizeDelta.y / 3));
        }
    }

    private void SetHandleState(eEventHandle _handle)
    {
        m_ePrevEvent = _handle;
    }

    private void SetPlayerState(ePlayerState _state)
    {
        m_ePlayerState = _state;
    }

    // // // // // // // //
    // 플레이어 컨트롤 스크립트에서 x값을 받기 위함
    public float GetHorizontalValue()
    {
        return inputVector.x;
    }

    // 플레이어 컨트롤 스크립트에서 y값을 받기 위함
    public float GetVerticalValue()
    {
        return inputVector.y;
    }

}