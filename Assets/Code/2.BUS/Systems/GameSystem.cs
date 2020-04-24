/* Created: Bkack Face (bf.blackface@gmail.com)
 * Game System
 * 19/05/2019
 */
using Assets.Code._0.DTO.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
//Thiết lập những thứ cần thiết trước khi bắt đầu trò chơi
public static class GameSystem
{
    public static SettingModel Settings;//Setting game
    public static ControlFunctions ControlFunctions;
    public static bool ControlActive = true; //Cho phép thao tác mọi thứ trong scene hay không
    public static Canvas MessageCanvas; //Canvas UI dành cho confirm box, message error...
    public static readonly float TimeTransparentUI = 0.2f; //Thời gian làm mờ UI và ẩn đi
    public static AnimationCurve AnimCurve; //Đường vẽ tọa độ di chuyển object
    public static List<bool> Tutorial;
    public static bool IsTutorialing;//Có đang chạy hướng dẫn chơi hay ko

    #region Khởi tạo variable Confirm Box 
    private static GameObject ConfirmBox; //Confirm box
    private static Button ConfirmBoxOK; //Button OK của confirm box
    private static Button ConfirmBoxCancel; //Button Cancel của confirm box
    public static Text ConfirmBoxText; //Text thông báo của confirm box
    public static int ConfirmBoxResult; //0 = wait, 1 = OK, 2 = Cancel
    #endregion

    #region Khởi tạo variable Message Box 
    private static readonly int MessageMax = 20; //Tối đa số object message
    private static List<GameObject> Message;
    private static Text[] MessageText;
    private static Image[] MessageImage;
    private static float[] MessageOpacity;
    private static RectTransform[] MessageRect;
    #endregion

    #region Khởi tạo variables Information Box 

    static GameObject InforBox; //Object box thông tin
    static Text InforBoxText; //Nội dung trong box thông tin

    #endregion

    #region Khởi tạo variables Item Information 

    public static GameObject ItemInformation;
    static Image ItemInf_ImageBGItem; //Viền item
    static Image ItemInf_ImageItem; //Hình ảnh item
    static Text[] ItemInf_Text;
    static Button[] ItemInf_ButtonAction; //Các button thực hiện chức năng
    public static int ActionItemInformation; //-2 = đóng form, -1 = wait, 0 = sell, 1 = equip, 2 = remove, 3 = up level, 4 = up color, 5 = disassemble

    #endregion

    #region Khởi tạo variables Prefabs UI 
    public static GameObject BreakBallUI;
    public static GameObject InventoryUI;
    public static GameObject HeroUserUI;
    public static GameObject HeroUserDetailCanvasUI;
    public static GameObject VideoRewardGoldGemUI;
    public static GameObject CraftUI;
    public static GameObject ItemDetailCanvasUI;
    public static GameObject TowerCanvasUI;
    public static GameObject InsertSocketCanvasUI;
    public static GameObject UnoCardCanvasUI;
    public static GameObject UnoIntroductionCanvasUI;
    public static GameObject SettingCanvasUI;
    public static GameObject InputNameCanvasUI;
    public static GameObject VoteFunctionCanvasUI;
    #endregion

    public static AudioSource BGM;
    public static AudioSource Sound; //Control Âm thanh của skill

    public static void Initialize()
    {
        if (MessageCanvas != null)
            return;

        //Khởi tạo các biến tutorial
        Tutorial = new List<bool>();
        //Tutorial.Add(GameSystem.Settings.Tutorial);//1 - Hướng dẫn hero
        //Tutorial.Add(GameSystem.Settings.Tutorial);
        //Tutorial.Add(GameSystem.Settings.Tutorial);
        //Tutorial.Add(GameSystem.Settings.Tutorial);
        //Tutorial.Add(GameSystem.Settings.Tutorial);
        //Tutorial.Add(GameSystem.Settings.Tutorial);
        //Tutorial.Add(GameSystem.Settings.Tutorial);

        MessageCanvas = (Canvas)MonoBehaviour.Instantiate(Resources.Load<Canvas>("Prefabs/UI/MessageCanvas"), new Vector3(0, 0, 0), Quaternion.identity);
        ControlFunctions = MessageCanvas.transform.GetChild(2).GetComponent<ControlFunctions>();
        SetupConfirmBox();
        SetupMessage();
        SetupInforBox();
        SetupItemInformation();

        if (Sound == null)
            Sound = MessageCanvas.transform.GetChild(1).GetComponent<AudioSource>();
    }

    #region Setup and control Confirm Box 

    //Setup Confirm Box
    private static void SetupConfirmBox()
    {
        ConfirmBox = GameObject.Find("ConfirmBox");
        ConfirmBoxOK = MessageCanvas.transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        ConfirmBoxCancel = MessageCanvas.transform.GetChild(0).transform.GetChild(3).GetComponent<Button>();
        ConfirmBoxText = MessageCanvas.transform.GetChild(0).transform.GetChild(4).GetComponent<Text>();
        ConfirmBoxCancel.onClick.AddListener(() => CancelConfirmBox());
        ConfirmBoxOK.onClick.AddListener(() => AcceptConfirmBox());
        ConfirmBoxResult = 0;
        ConfirmBox.SetActive(false);
    }
    //Hủy confirm box
    private static void CancelConfirmBox()
    {
        ConfirmBox.SetActive(false);
        ConfirmBoxText.text = "";
        ConfirmBoxResult = 2; //Result = cancel
    }
    //Chấp nhận confirm box
    private static void AcceptConfirmBox()
    {
        ConfirmBox.SetActive(false);
        ConfirmBoxText.text = "";
        ConfirmBoxResult = 1; //Result = ok
    }

    /// <summary>
    /// Hiển thị màn hình xác nhận
    /// </summary>
    /// <param name="text"></param>
    public static void ShowConfirmDialog(string text)
    {
        try
        {
            ConfirmBoxResult = 0;
            ConfirmBox.SetActive(true);
            ConfirmBoxText.text = text;
        }
        catch { }
    }

    #endregion

    #region Setup and control Message 
    private static void SetupMessage()
    {
        Message = new List<GameObject>(); // new GameObject[MessageMax];
        MessageText = new Text[MessageMax];
        MessageImage = new Image[MessageMax];
        MessageOpacity = new float[MessageMax];
        MessageRect = new RectTransform[MessageMax];
        for (int i = 0; i < MessageMax; i++)
        {
            Message.Add((GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/PanelMessage"), new Vector3(0, 0, 0), Quaternion.identity));
            Message[i].transform.SetParent(MessageCanvas.transform, false);
            MessageRect[i] = Message[i].transform.GetChild(0).transform.GetChild(1).GetComponent<RectTransform>();
            MessageImage[i] = Message[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
            MessageText[i] = Message[i].transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
            MessageOpacity[i] = 1f;
            Message[i].SetActive(false);
        }
    }

    /// <summary>
    /// Trả về object đang rảnh
    /// </summary>
    private static int GetSlot()
    {
        var find = Message.FindIndex(x => !x.activeSelf);
        if (find != -1) //Nếu tìm thấy
        {
            Message[find].transform.position = new Vector3(0, 0, 0); //Set lại tọa độ
            Message[find].SetActive(true); //Hiển thị mess lên
        }
        return find;
    }

    /// <summary>
    /// Hiển thị thông báo
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static IEnumerator ShowMessage(string text)
    {
        var slotmessage = GetSlot();
        if (slotmessage != -1)
        {
            MessageText[slotmessage].text = text;
            MessageRect[slotmessage].sizeDelta = new Vector2(MessageText[slotmessage].preferredWidth, MessageRect[slotmessage].sizeDelta.y);
            var rect = Message[slotmessage].transform;
            var startPos = Message[slotmessage].transform.position;
            var targetPos = startPos + new Vector3(0, 5f, 0);
            float time = 0;
            float rate = 1 / 2f; //2 giây
            yield return new WaitForSeconds(.3f);
            while (time < 1)
            {
                time += rate * Time.deltaTime;
                rect.transform.position = Vector2.Lerp(startPos, targetPos, AnimCurve.Evaluate(time));
                yield return 0;
            }
            //Gán lại tọa độ sau khi move xong
            rect.position = targetPos;
            Message[slotmessage].SetActive(false);
            MessageText[slotmessage].text = "";
        }
        //========================================
    }

    /// <summary>
    /// Disable toàn bộ message đang hiển thị (dành cho chuyển scene mà vẫn đang hiển thị)
    /// </summary>
    public static void DisableAllMessenger()
    {
        var count = Message.Count;
        for (int i = 0; i < count; i++)
        {
            Message[i].SetActive(false);
        }
    }

    #endregion

    #region Setup and control Infor box 

    /// <summary>
    /// Khởi tạo information box
    /// </summary>
    static void SetupInforBox()
    {
        InforBox = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InformationBox"), new Vector3(0, 0, 0), Quaternion.identity);
        InforBox.transform.SetParent(MessageCanvas.transform, false);
        InforBox.SetActive(false);
        InforBoxText = InforBox.transform.GetChild(1).GetComponent<Text>(); //Text nội dung của box
    }

    /// <summary>
    /// Khởi tạo sự kiện information cho button - hiển thị bảng thông tin khi dí chết button
    /// </summary>
    /// <param name="obj">Object button</param>
    /// <param name="eventDown">Sự kiện nhấn</param>
    /// <param name="eventUp">Sự kiện nhả</param>
    public static void CreateBoxDownUp(GameObject obj, UnityEngine.Events.UnityAction<BaseEventData> eventDown, UnityEngine.Events.UnityAction<BaseEventData> eventUp)
    {
        EventTrigger trigger = obj.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        var pointerUp = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown; //Event nhấn
        pointerUp.eventID = EventTriggerType.PointerUp; //Event nhả
        pointerDown.callback.AddListener(eventDown);
        pointerUp.callback.AddListener(eventUp);
        trigger.triggers.Add(pointerDown);
        trigger.triggers.Add(pointerUp);
    }

    /// <summary>
    /// Show bảng thông tin (Đang chưa sử dụng, chưa viết hoàn thành)
    /// </summary>
    /// <param name="rec">Chiều dài rộng</param>
    /// <param name="pos">Tọa độ của box</param>
    /// <param name="space">Khoảng cách từ pos</param>
    /// <param name="posID">Vị trí so với con trỏ khi tap, 1->9</param>
    /// <param name="content">Nội dung trong box</param>
    public static void ShowBoxInformation(Vector2 rec, Vector3 pos, Vector2 space, sbyte posID, string content)
    {
        InforBoxText.text = content;
        var sizebox = InforBox.GetComponent<RectTransform>().sizeDelta = new Vector2(rec.x + 16, InforBoxText.GetComponent<RectTransform>().sizeDelta.y + 16);
        //Debug.Log (lineCount);
        //Debug.Log (sizebox);
        switch (posID)
        { //Set tọa độ dựa trên posID, 1 = topleft, 2 = top, 3 = topright, 4 = left, 5 = mid, 6 = right, 7 = botleft, 8 = bot, 9 = botright
            case 1:
                InforBox.GetComponent<RectTransform>().position = pos - new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(sizebox.x / 2 + space.x, sizebox.y / 2 + space.y, 0)).x, Camera.main.ScreenToWorldPoint(new Vector3(sizebox.x / 2 + space.x, sizebox.y / 2 + space.y, 0)).y, 0);
                break;
            case 2:
                InforBox.GetComponent<RectTransform>().position = new Vector3(Camera.main.ScreenToWorldPoint(pos).x, Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(rec.x / 2, rec.y / 2, 0)).y, 0);
                break;
            case 3:
                InforBox.GetComponent<RectTransform>().position = new Vector3(Camera.main.ScreenToWorldPoint(pos + new Vector3(rec.x / 2, rec.y / 2, 0)).x, Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(rec.x / 2, rec.y / 2, 0)).y, 0);
                break;
            case 4:
                InforBox.GetComponent<RectTransform>().position = new Vector3(Camera.main.ScreenToWorldPoint(pos - new Vector3(rec.x / 2, rec.y / 2, 0)).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
                break;
            case 5:
                InforBox.GetComponent<RectTransform>().position = new Vector3(Camera.main.ScreenToWorldPoint(pos).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
                break;
            case 6:
                InforBox.GetComponent<RectTransform>().position = new Vector3(Camera.main.ScreenToWorldPoint(pos + new Vector3(rec.x / 2, rec.y / 2, 0)).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
                break;
            case 7:
                InforBox.GetComponent<RectTransform>().position = new Vector3(Camera.main.ScreenToWorldPoint(pos - new Vector3(rec.x / 2, rec.y / 2, 0)).x, Camera.main.ScreenToWorldPoint(Input.mousePosition - new Vector3(rec.x / 2, rec.y / 2, 0)).y, 0);
                break;
            case 8:
                InforBox.GetComponent<RectTransform>().position = new Vector3(Camera.main.ScreenToWorldPoint(pos).x, Camera.main.ScreenToWorldPoint(Input.mousePosition - new Vector3(rec.x / 2, rec.y / 2, 0)).y, 0);
                break;
            case 9:
                InforBox.GetComponent<RectTransform>().position = new Vector3(Camera.main.ScreenToWorldPoint(pos + new Vector3(rec.x / 2, rec.y / 2, 0)).x, Camera.main.ScreenToWorldPoint(Input.mousePosition - new Vector3(rec.x / 2, rec.y / 2, 0)).y, 0);
                break;
            default:
                InforBox.GetComponent<RectTransform>().position = Vector2.zero;
                break;
        }
        InforBox.GetComponent<RectTransform>().position = pos;
        InforBox.gameObject.SetActive(true);
    }

    /// <summary>
    /// Ẩn bảng thông tin
    /// </summary>
    public static void HideBoxInformation()
    {
        InforBox.gameObject.SetActive(false);
    }

    #endregion

    #region Setup and control Sound, Music 

    /// <summary>
    /// Khởi tạo nhạc nền
    /// </summary>
    private static void SetupBGM()
    {
        if (GameSystem.Settings.MusicEnable || GameSystem.Settings.SoundEnable)
        {
            BGM = MessageCanvas.transform.GetChild(1).GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Chạy nhạc nền
    /// </summary>
    /// <param name="type">0: nhạc nền chung, 1: nhạc nền battle</param>
    public static void RunBGM(int type)
    {
        if (GameSystem.Settings.MusicEnable)
        {
            if (BGM == null)
                SetupBGM();
            if (!BGM.isPlaying)
            {
                BGM.clip = type.Equals(0) ? Resources.Load<AudioClip>("Audio/BGM/Main" + UnityEngine.Random.Range(0, 3).ToString()) : Resources.Load<AudioClip>("Audio/BGM/Battle" + UnityEngine.Random.Range(0, 4).ToString());
                BGM.Play(0);
            }
        }
    }

    /// <summary>
    /// Dừng nhạc nền
    /// </summary>
    public static void StopBGM()
    {
        if (BGM != null)
            BGM.Stop();
    }

    public static IEnumerator PlaySound(AudioClip audioFile, float timeDelay)
    {
        if (GameSystem.Settings.SoundEnable)
        {
            if (BGM == null)
                SetupBGM();
            yield return new WaitForSeconds(timeDelay);
            BGM.PlayOneShot(audioFile);
        }
    }

    /// <summary>
    /// Fadeout BGM
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="FadeTime"></param>
    /// <returns></returns>
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        if (GameSystem.Settings.MusicEnable)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
            audioSource.Stop();
            audioSource.volume = startVolume;
        }
    }

    #endregion

    #region Setup and control Item Information 
    private static void SetupItemInformation()
    {
        ItemInformation = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UI-ItemInfor"), new Vector3(0, 0, 0), Quaternion.identity);
        ItemInformation.transform.SetParent(MessageCanvas.transform, false);
        ItemInformation.SetActive(false);
        ItemInf_ImageBGItem = ItemInformation.transform.GetChild(3).GetComponent<Image>(); //Viền item
        ItemInf_ImageItem = ItemInformation.transform.GetChild(4).GetComponent<Image>(); //Hình ảnh item
        ItemInformation.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => ActionItemInformation = -2); //Trạng thái -2 = null khi đóng form, đồng nghĩa với việc ko thao tác gì
        ItemInformation.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => ItemInformation.SetActive(false)); //Nút đóng form

        //Tạo text
        ItemInf_Text = new Text[4];
        ItemInf_Text[0] = ItemInformation.transform.GetChild(5).GetComponent<Text>(); //Item Name
        ItemInf_Text[1] = ItemInformation.transform.GetChild(6).GetComponent<Text>(); //Item quantity
        ItemInf_Text[2] = ItemInformation.transform.GetChild(10).GetComponent<Text>(); //Item Price
        ItemInf_Text[3] = ItemInformation.transform.GetChild(7).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>(); //Item description

        //Tạo các button thực hiện chức năng
        ItemInf_ButtonAction = new Button[6];
        ItemInf_ButtonAction[0] = ItemInformation.transform.GetChild(12).GetComponent<Button>(); //Button sell
        ItemInf_ButtonAction[1] = ItemInformation.transform.GetChild(13).GetComponent<Button>(); //Button trang bị
        ItemInf_ButtonAction[2] = ItemInformation.transform.GetChild(14).GetComponent<Button>(); //Button bỏ trang bị
        ItemInf_ButtonAction[3] = ItemInformation.transform.GetChild(15).GetComponent<Button>(); //Button nâng cấp
        ItemInf_ButtonAction[4] = ItemInformation.transform.GetChild(16).GetComponent<Button>(); //Button nâng phẩm
        ItemInf_ButtonAction[5] = ItemInformation.transform.GetChild(17).GetComponent<Button>(); //Button phân giải

        //Chức năng button - Trả về các giá trị tương ứng, detect ở hàm gọi tới và thực hiện action, giống confirm box
        ItemInf_ButtonAction[0].onClick.AddListener(() => ActionItemInformation = 0); //Button sell
        ItemInf_ButtonAction[1].onClick.AddListener(() => ActionItemInformation = 1); //Button trang bị
        ItemInf_ButtonAction[2].onClick.AddListener(() => ActionItemInformation = 2); //Button bỏ trang bị
        ItemInf_ButtonAction[3].onClick.AddListener(() => ActionItemInformation = 3); //Button nâng cấp
        ItemInf_ButtonAction[4].onClick.AddListener(() => ActionItemInformation = 4); //Button nâng phẩm
        ItemInf_ButtonAction[5].onClick.AddListener(() => ActionItemInformation = 5); //Button phân giải

        //Text cho button
        ItemInformation.transform.GetChild(12).transform.GetChild(0).GetComponent<Text>().text = Languages.lang[138]; //Text sell
        ItemInformation.transform.GetChild(13).transform.GetChild(0).GetComponent<Text>().text = Languages.lang[134]; //Text trang bị
        ItemInformation.transform.GetChild(14).transform.GetChild(0).GetComponent<Text>().text = Languages.lang[48]; //Text bỏ trang bị
        ItemInformation.transform.GetChild(15).transform.GetChild(0).GetComponent<Text>().text = Languages.lang[154]; //Text nâng cấp
        ItemInformation.transform.GetChild(16).transform.GetChild(0).GetComponent<Text>().text = Languages.lang[155]; //Text nâng phẩm
        ItemInformation.transform.GetChild(17).transform.GetChild(0).GetComponent<Text>().text = Languages.lang[156]; //Text phân giải
    }

    /// <summary>
    /// Hiển thị thông tin item khi click vào
    /// </summary>
    /// <param name="item">Item</param>
    /// <param name="actionType">Kiểu thao tác: 0-Bán trang bị, 1-Trang bị, 2-Gỡ trang bị</param>
    /// <param name="isShowUpgrade">Hiển thị button nâng cấp?</param>
    /// <param name="isShowUpColor">Hiển thị button nâng phẩm?</param>
    /// <param name="isShowSeparate">Hiển thị button phân giải?</param>
    /// <param name="isShowNextItem">Hiển thị button next và prev item?</param>
    public static void ShowItemIformation(ItemModel item, int actionType, bool isShowUpgrade, bool isShowUpColor, bool isShowSeparate, bool isShowNextItem)
    {
        ActionItemInformation = -1; //Chờ lệnh thao tác từ box item information
        ItemInf_ImageBGItem.sprite = Resources.Load<Sprite>("Images/BorderItem/" + item.ItemColor); //Viền item
        ItemInf_ImageItem.sprite = Resources.Load<Sprite>("Images/Items/" + item.ItemType + @"/" + item.ItemID); //Hình ảnh item
        ItemInf_Text[0].text = ItemSystem.GetItemName(item.ItemType, item.ItemID); //Item Name
        if (item.ItemTypeMode.Equals(global::ItemModel.TypeMode.Equip))
            ItemInf_Text[1].text = Languages.lang[23] + item.ItemLevel; //Item level
        else
            ItemInf_Text[1].text = Languages.lang[75] + item.Quantity; //Item quantity
        ItemInf_Text[2].text = Languages.lang[76] + ItemSystem.GetItemPrice(item).ToString(); //Item price
        ItemInf_Text[3].text = ItemSystem.GetItemDescription(item); //Item description

        //Ẩn hiện button theo yêu cầu
        ItemInf_ButtonAction[0].gameObject.SetActive(actionType.Equals(0) ? true : false); //Button sell
        ItemInf_ButtonAction[1].gameObject.SetActive(actionType.Equals(1) ? true : false); //Button trang bị
        ItemInf_ButtonAction[2].gameObject.SetActive(actionType.Equals(2) ? true : false); //Button bỏ trang bị
        ItemInf_ButtonAction[3].gameObject.SetActive(isShowUpgrade); //Button nâng cấp
        ItemInf_ButtonAction[4].gameObject.SetActive(isShowUpColor); //Button nâng phẩm
        ItemInf_ButtonAction[5].gameObject.SetActive(isShowSeparate); //Button phân giải
        ItemInformation.transform.GetChild(8).gameObject.SetActive(isShowNextItem); //Nút prev item
        ItemInformation.transform.GetChild(9).gameObject.SetActive(isShowNextItem); //Nút next item

        ItemInformation.SetActive(true);
    }

    #endregion

    #region UI Controller 

    /// <summary>
    /// Khởi tạo các prefab UI
    /// </summary>
    /// <param name="type"></param>
    public static void InitializePrefabUI(int type)
    {
        switch (type)
        {
            case 0: //Break ball UI (Đập bóng)
                BreakBallUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/BreakBallCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                BreakBallUI.GetComponent<Canvas>().worldCamera = Camera.main;
                BreakBallUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 1: //Inventory UI (Thùng đồ)
                InventoryUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                InventoryUI.GetComponent<Canvas>().worldCamera = Camera.main;
                InventoryUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 2: //Hero User UI (Tướng)
                HeroUserUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/HeroUserCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                HeroUserUI.GetComponent<Canvas>().worldCamera = Camera.main;
                HeroUserUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 3: //VideoRewardGoldGemUI xem video nhận thưởng
                VideoRewardGoldGemUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/VideoRewardGoldGemUI"), new Vector3(0, 0, 0), Quaternion.identity);
                VideoRewardGoldGemUI.GetComponent<Canvas>().worldCamera = Camera.main;
                VideoRewardGoldGemUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 4: //Chi tiết tướng
                HeroUserDetailCanvasUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/HeroUserDetailCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                HeroUserDetailCanvasUI.GetComponent<Canvas>().worldCamera = Camera.main;
                HeroUserDetailCanvasUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 5: //Hệ thống chế tạo item
                CraftUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/CraftCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                CraftUI.GetComponent<Canvas>().worldCamera = Camera.main;
                CraftUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 6: //Chi tiết item
                ItemDetailCanvasUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/ItemDetailCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                ItemDetailCanvasUI.GetComponent<Canvas>().worldCamera = Camera.main;
                ItemDetailCanvasUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 7: //Tower: vượt tháp
                TowerCanvasUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/TowerCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                TowerCanvasUI.GetComponent<Canvas>().worldCamera = Camera.main;
                TowerCanvasUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 8: //Khảm socket
                InsertSocketCanvasUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InsertSocketCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                InsertSocketCanvasUI.GetComponent<Canvas>().worldCamera = Camera.main;
                InsertSocketCanvasUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 9: //Uno Card
                UnoCardCanvasUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UnoCardCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                UnoCardCanvasUI.GetComponent<Canvas>().worldCamera = Camera.main;
                UnoCardCanvasUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 10: //UnoIntroductionCanvasUI
                UnoIntroductionCanvasUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/UnoIntroductionCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                UnoIntroductionCanvasUI.GetComponent<Canvas>().worldCamera = Camera.main;
                UnoIntroductionCanvasUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 11: //SettingCanvasUI
                SettingCanvasUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/SettingCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                SettingCanvasUI.GetComponent<Canvas>().worldCamera = Camera.main;
                SettingCanvasUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 12: //InputNameCanvasUI
                InputNameCanvasUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/InputNameCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                InputNameCanvasUI.GetComponent<Canvas>().worldCamera = Camera.main;
                InputNameCanvasUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            case 13: //VoteFunctionCanvasUI
                VoteFunctionCanvasUI = (GameObject)MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UI/VoteFunctionCanvasUI"), new Vector3(0, 0, 0), Quaternion.identity);
                VoteFunctionCanvasUI.GetComponent<Canvas>().worldCamera = Camera.main;
                VoteFunctionCanvasUI.GetComponent<Canvas>().planeDistance = 8;
                break;
            default:
                break;
        }
        //yield return null;
    }

    /// <summary>
    /// Gỡ bỏ các prefab UI và giải phóng bộ nhớ
    /// </summary>
    /// <param name="type"></param>
    public static void DisposePrefabUI(int type)
    {
        //DisableAllMessenger ();
        switch (type)
        {
            case 0: //Break ball UI
                MonoBehaviour.Destroy(BreakBallUI);
                break;
            case 1: //Inventory UI (Thùng đồ)
                MonoBehaviour.Destroy(InventoryUI);
                break;
            case 2: //Hero User UI (Tướng)
                MonoBehaviour.Destroy(HeroUserUI);
                break;
            case 3: //VideoRewardGoldGemUI
                MonoBehaviour.Destroy(VideoRewardGoldGemUI);
                break;
            case 4: //Chi tiết tướng
                MonoBehaviour.Destroy(HeroUserDetailCanvasUI);
                break;
            case 5: //Hệ thống chế tạo item
                MonoBehaviour.Destroy(CraftUI);
                break;
            case 6: //Chi tiết item
                MonoBehaviour.Destroy(ItemDetailCanvasUI);
                break;
            case 7: //Tower: vượt tháp
                MonoBehaviour.Destroy(TowerCanvasUI);
                break;
            case 8: //Khảm socket
                MonoBehaviour.Destroy(InsertSocketCanvasUI);
                break;
            case 9: //UnoCardCanvasUI
                MonoBehaviour.Destroy(UnoCardCanvasUI);
                break;
            case 10: //UnoIntroductionCanvasUI
                MonoBehaviour.Destroy(UnoIntroductionCanvasUI);
                break;
            case 11: //SettingCanvasUI
                MonoBehaviour.Destroy(SettingCanvasUI);
                break;
            case 12: //InputNameCanvasUI
                MonoBehaviour.Destroy(InputNameCanvasUI);
                break;
            case 13: //VoteFunctionCanvasUI
                MonoBehaviour.Destroy(VoteFunctionCanvasUI);
                break;
            default:
                break;
        }
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// Xóa bỏ toàn bộ các object con từ object cha, giải phóng bộ nhớ
    /// </summary>
    /// <param name="obj"></param>
    public static void DisposeAllObjectChild(GameObject obj)
    {
        var count = obj.transform.childCount;
        for (var i = count - 1; i >= 0; i--)
        {
            MonoBehaviour.Destroy(obj.transform.GetChild(i).gameObject);
        }
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// Xóa bỏ 1 game object và cho phép giải phóng bộ nhớ
    /// </summary>
    /// <param name="obj">Object cần loại bỏ</param>
    /// <param name="isRefreshMemory">Có giải phóng bộ nhớ luôn ko ?</param>
    public static void DisposeObjectCustom(GameObject obj, bool isRefreshMemory)
    {
        MonoBehaviour.Destroy(obj);
        if (isRefreshMemory)
            Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// Xóa danh sách object và cho phép giải phóng bộ nhớ 
    /// </summary>
    /// <param name="objList"></param>
    /// <param name="isRefreshMemory"></param>
    public static void DisposeObjectList<T>(List<T> objList, bool isRefreshMemory) where T : UnityEngine.Object
    {
        if (objList != null)
        {
            var count = objList.Count;
            for (var i = count - 1; i >= 0; i--)
            {
                MonoBehaviour.Destroy(objList[i]);
            }
            if (isRefreshMemory)
                Resources.UnloadUnusedAssets();
        }
    }

    /// <summary>
    /// Thay đổi scale của 1 object UI theo thời gian
    /// </summary>
    /// <param name="type">0 = rectransform, 1 = transform</param>
    /// <param name="obj">Objec truyền vào</param>
    /// <param name="targetScale">Scalse sẽ thay đổi</param>
    /// <param name="duration">Thời gian thay đổi là bao lâu</param>
    /// <returns></returns>
    public static IEnumerator ScaleUI(int type, GameObject obj, Vector3 targetScale, float duration)
    {
        float time = 0;
        float rate = 1 / duration;
        Vector2 startPos = type == 0 ? obj.GetComponent<RectTransform>().localScale : obj.GetComponent<Transform>().localScale;
        var rect = type == 0 ? obj.GetComponent<RectTransform>() : obj.GetComponent<Transform>();
        while (time < 1)
        {
            time += rate * Time.deltaTime;
            rect.localScale = Vector3.Lerp(startPos, targetScale, time);
            yield return 0;
        }
    }

    /// <summary>
    /// hay đổi scale của 1 object UI theo thời gian
    /// </summary>
    /// <param name="type">0 = rectransform, 1 = transform</param>
    /// <param name="obj">Objec truyền vào</param>
    /// <param name="startScale">Scalse ban đầu</param>
    /// <param name="targetScale">Scalse sẽ thay đổi</param>
    /// <param name="duration">Thời gian thay đổi là bao lâu</param>
    /// <returns></returns>
    public static IEnumerator ScaleUI(int type, GameObject obj, Vector3 startScale, Vector3 targetScale, float duration)
    {
        if (type.Equals(0))
            obj.GetComponent<RectTransform>().localScale = startScale;
        else
            obj.GetComponent<Transform>().localScale = startScale;
        float time = 0;
        float rate = 1 / duration;
        Vector2 startPos = type == 0 ? obj.GetComponent<RectTransform>().localScale : obj.GetComponent<Transform>().localScale;
        var rect = type == 0 ? obj.GetComponent<RectTransform>() : obj.GetComponent<Transform>();
        while (time < 1)
        {
            time += rate * Time.deltaTime;
            rect.localScale = Vector3.Lerp(startPos, targetScale, time);
            yield return 0;
        }
    }

    /// <summary>
    /// Ẩn object sau 1 khoảng thời gian (dành cho UI)
    /// </summary>
    /// <param name="obj">Object cần ẩn</param>
    /// <param name="time">Thời gian delay</param>
    /// <returns></returns>
    public static IEnumerator HideObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    /// <summary>
    /// Hiển thị thông tin của item
    /// </summary>
    public static void ShowItemInfor(ItemModel item)
    {

    }

    /// <summary>
    /// Di chuyển object tới vị trí trong khoảng time.
    /// Lưu ý, nếu type là rect, thì startPos và targetPos phải là anchorPosition, còn ko là position
    /// </summary>
    /// <param name="isRect">true = rectransform, false = transform</param>
    /// <param name="obj">object sẽ move</param>
    /// <param name="startPos">tọa độ bắt đầu</param>
    /// <param name="targetPos">tọa độ kết thúc</param>
    /// <param name="duration">thời gian move</param>
    /// <param name="animCurve">đường cong move</param>
    /// <returns></returns>
    public static IEnumerator MoveObjectCurve(bool isRect, GameObject obj, Vector2 startPos, Vector2 targetPos, float duration, AnimationCurve animCurve)
    {
        var rect = isRect ? obj.GetComponent<RectTransform>() : null;
        var rect2 = isRect ? null : obj.GetComponent<Transform>();
        float time = 0;
        float rate = 1 / duration;
        while (time < 1)
        {
            time += rate * Time.deltaTime;
            if (isRect)
                rect.localPosition = Vector2.Lerp(startPos, targetPos, animCurve.Evaluate(time));
            else
                rect2.position = Vector2.Lerp(startPos, targetPos, animCurve.Evaluate(time));
            yield return null;
        }
        //Gán lại tọa độ sau khi move xong
        if (isRect)
            rect.localPosition = targetPos;
        else
            rect2.position = targetPos;
        GlobalVariables.ObjectIsMoving = false;
    }

    /// <summary>
    /// Xoay object theo góc và time
    /// </summary>
    /// <param name="isRect"></param>
    /// <param name="obj"></param>
    /// <param name="startPos"></param>
    /// <param name="targetPos"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator RotationObject(bool isRect, GameObject obj, Vector3 startPos, Vector3 targetPos, float duration)
    {
        var rect = isRect ? obj.GetComponent<RectTransform>() : null;
        var rect2 = isRect ? null : obj.GetComponent<Transform>();
        float time = 0;
        float rate = 1 / duration;
        while (time < 1)
        {
            time += rate * Time.deltaTime;
            if (isRect)
                rect.eulerAngles = Vector3.Lerp(startPos, targetPos, time);
            else
                rect2.eulerAngles = Vector3.Lerp(startPos, targetPos, time);
            yield return null;
        }
        //Gán lại tọa độ sau khi move xong
        if (isRect)
            rect.eulerAngles = targetPos;
        else
            rect2.eulerAngles = targetPos;
        GlobalVariables.ObjectIsMoving = false;
    }

    /// <summary>
    /// Trả về số lớn nhất trong mảng
    /// </summary>
    public static int FindNumberHighest(int[] listNumber)
    {
        var result = listNumber[0];
        for (int i = 0; i < listNumber.Length; i++)
        {
            if (result > listNumber[i])
                result = listNumber[i];
        }
        return result;
    }

    /// <summary>
    /// Trả về index số lớn nhất trong mảng
    /// </summary>
    public static int FindSlotNumberHighest(int[] listNumber)
    {
        var result = 0;
        for (int i = 0; i < listNumber.Length; i++)
        {
            if (listNumber[result] < listNumber[i])
                result = i;
        }
        return result;
    }

    /// <summary>
    /// Cập nhật tài nguyên
    /// </summary>
    public static void UpdateResourceText(Text gold, Text gems)
    {
        gold.text = DataUserController.User.Golds.ToString();
        gems.text = DataUserController.User.Gems.ToString();
    }
    #endregion

    #region Object Controller 

    /// <summary>
    /// Check và trả về thứ tự object đang ko hoạt động
    /// </summary>
    /// <param name="listGameObject"></param>
    /// <returns></returns>
    public static int? CheckListObjectActive(List<GameObject> listGameObject)
    {
        var count = listGameObject.Count;
        for (int i = 0; i < count; i++)
        {
            if (!listGameObject[i].activeSelf)
            {
                return i;
            }
            if (i >= count - 1)
                return null;
        }
        return null;
    }

    /// <summary>
    /// Nhân bản GameObject từ list
    /// </summary>
    /// <param name="listGameObject"></param>
    /// <param name="position"></param>
    /// <param name="isSetParent"></param>
    /// <param name="parent"></param>
    public static void DuplicateGameObjectFromList(List<GameObject> listGameObject, Vector3 position, bool isSetParent, GameObject parentObject)
    {
        //Nhân bản object đầu tiên của mảng
        listGameObject.Add(MonoBehaviour.Instantiate(listGameObject[0], position, Quaternion.identity));

        //Set parent nếu như truyền tham số
        if (isSetParent && parentObject != null)
        {
            listGameObject[listGameObject.Count - 1].transform.SetParent(parentObject.transform, false);
        }
    }

    /// <summary>
    /// Acive object trong list và tạo mới nếu toàn bộ object đang active
    /// </summary>
    /// <param name="listGameObject"></param>
    /// <param name="position"></param>
    /// <param name="isSetParent"></param>
    /// <param name="parentObject"></param>
    public static void CheckExistAndCreateObjectFromList(List<GameObject> listGameObject, Vector3 position, bool isSetParent, GameObject parentObject)
    {
        //Lấy slot trong mảng đang deactive
        var slotDeactive = CheckListObjectActive(listGameObject);

        //Nếu có object đang deactive => active nó
        if (slotDeactive != null)
        {
            listGameObject[(int)slotDeactive].SetActive(true);
            listGameObject[(int)slotDeactive].transform.position = position;
        }
        else
        { //Tạo mới 1 object nếu toàn bộ object trong mảng đều đang active
            DuplicateGameObjectFromList(listGameObject, position, isSetParent, parentObject);
        }
    }

    /// <summary>
    /// Disable toàn bộ object của list
    /// </summary>
    /// <param name="listGameObject"></param>
    /// <param name="position"></param>
    /// <param name="isSetParent"></param>
    /// <param name="parentObject"></param>
    public static void DisableObjectFromList(List<GameObject> listGameObject)
    {
        var count = listGameObject.Count;
        for (int i = 0; i < count; i++)
            listGameObject[i].SetActive(false);
    }

    /// <summary>
    /// Sắp xếp tăng giảm dần trong mảng
    /// </summary>
    /// <param name="list">mảng</param>
    /// <param name="isDesc">true = tăng dần, false = giảm dần</param>
    public static void QuickSortList(List<int> listObject, bool isDesc)
    {
        var count = listObject.Count;
        for(int i = 0;i<count;i++)
        {
            for(int j = i+1;j<count; j++)
            {
                if(listObject[i] > listObject[j])
                {
                    var temp = listObject[i];
                    listObject[i] = listObject[j];
                    listObject[j] = temp;
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// Check ký tự đặc biệt
    /// </summary>
    public static bool HasSpecialChar(string input)
    {
        string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
        foreach (var item in specialChar)
        {
            if (input.Contains(item)) return true;
        }

        return false;
    }
}