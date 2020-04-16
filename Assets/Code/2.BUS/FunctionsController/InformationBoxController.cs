using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InformationBoxController : MonoBehaviour {
    public Text[] TextValues; //Set ở interface
    private Vector2 ThisRectangle; //Kích thước của khung box
    private Vector2 ThisPosition; //Tọa độ của khung box
    void Start () {
        ThisRectangle = this.GetComponent<RectTransform> ().sizeDelta;
        ThisPosition = this.GetComponent<RectTransform> ().position;
    }
    void OnEnable () {
        //TextValues[0].text = Languages.IntroductionTitle[IntroductionID]; //Title 
        //TextValues[1].text = Languages.IntroductionDescriptions[IntroductionID]; //Description
    }

    /// <summary>
    /// Show bảng thông tin
    /// </summary>
    /// <param name="isShow">Show hoặc hide</param>
    /// <param name="rec">Chiều dài rộng</param>
    /// <param name="vec">Tọa độ của box</param>
    /// <param name="title">Tiêu của box</param>
    /// <param name="content">Nội dung trong box</param>
    public void ShowBoxInformation (bool isShow, Vector2 rec, Vector2 vec, string title, string content) {
        if (isShow) {
            this.GetComponent<RectTransform> ().sizeDelta = rec;
            //this.GetComponent<RectTransform> ().position = Input.GetTouch(0).position;
            this.GetComponent<RectTransform> ().position = Input.GetTouch(0).position;
        }
        this.gameObject.SetActive (isShow);
    }

    /// <summary>
    /// Khởi tạo sự kiện information cho button - hiển thị bảng thông tin khi dí chết button
    /// </summary>
    /// <param name="obj">Object button</param>
    /// <param name="eventDown">Sự kiện nhấn</param>
    /// <param name="eventUp">Sự kiện nhả</param>
    public void CreateBoxDownUp (GameObject obj, UnityEngine.Events.UnityAction<BaseEventData> eventDown, UnityEngine.Events.UnityAction<BaseEventData> eventUp) {
        EventTrigger trigger = obj.gameObject.AddComponent<EventTrigger> ();
        var pointerDown = new EventTrigger.Entry ();
        var pointerUp = new EventTrigger.Entry ();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerDown.callback.AddListener (eventDown);
        pointerUp.callback.AddListener (eventUp);
        trigger.triggers.Add (pointerDown);
        trigger.triggers.Add (pointerUp);
    }
}