using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InputNameController : MonoBehaviour
{
    #region Variables
    [TabGroup("Module setting")]
    [Title("Các object")]
    [HideLabel]
    [LabelText("Nút accept")]
    [Required]
    public Button ButtonAccept;

    [TabGroup("Module setting")]
    [Title("Text giao diện")]
    [LabelText("Tiêu đề")]
    [Required]
    public Text TitleText;
    [TabGroup("Module setting")]
    [LabelText("Place holder khung nhập tên")]
    [Required]
    public Text PlaceHolderNameText;
    [TabGroup("Module setting")]
    [LabelText("Text tên")]
    [Required]
    public Text NameText;
    [TabGroup("Module setting")]
    [LabelText("Text nút accept")]
    [Required]
    public Text AcceptButtonText;

    #endregion

    void Start()
    {
        SetupTextUI();

        //Button Accept
        ButtonAccept.onClick.AddListener(() =>
        {
            if (Valid())
            {
                DataUserController.User.UserName = NameText.text;
                DataUserController.SaveUserInfor();
                GameSystem.DisposePrefabUI(12);
            }
        });
    }

    /// <summary>
    /// Gán text giao diện
    /// </summary>
    private void SetupTextUI()
    {
        PlaceHolderNameText.text = Languages.lang[122];//"Nhập tên của bạn";
        TitleText.text = Languages.lang[122];//"Nhập tên của bạn";
        AcceptButtonText.text = Languages.lang[60];//"Chấp nhận";
    }

    /// <summary>
    /// Check dữ liệu
    /// </summary>
    /// <returns></returns>
    private bool Valid()
    {
        //Check tên
        if (string.IsNullOrEmpty(NameText.text))
        {
            GameSystem.ControlFunctions.ShowMessage(Languages.lang[346]);//"Please input your name";
            return false;
        }
        //Check ký tự đặc biệt
        if (GameSystem.HasSpecialChar(NameText.text))
        {
            GameSystem.ControlFunctions.ShowMessage(Languages.lang[347]);//"Please input your name";
            return false;
        }
        return true;
    }
}
