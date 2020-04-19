using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System.Collections;

public class VoteBoxController : MonoBehaviour
{
    #region Variables
    [Title("Các object cần phải thiết lập")]
    [LabelText("Vote Box")]
    [Required]
    public GameObject VoteBox;

    [LabelText("Nút Vote")]
    [Required]
    public Button BtnVote;

    [LabelText("Nút hủy")]
    [Required]
    public Button BtnCancel;

    [LabelText("Nút đẩy ra vào")]
    [Required]
    public Button BtnExpand;

    [LabelText("Text nội dung")]
    [Required]
    public Text ContentText;

    [LabelText("Text vote")]
    [Required]
    public Text VoteText;

    [LabelText("Text cancel")]
    [Required]
    public Text CancelText;

    [LabelText("Home Controller (nếu đặt ở home)")]
    public Home HomeController;

    [LabelText("Khoảng cách di chuyển")]
    public float MoveRange;

    private float PosOriginal;
    private bool IsExpand = false;
    #endregion

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        if (DataUserController.User.VoteCode != GlobalVariables.VoteCode)
        {
            PosOriginal = VoteBox.transform.localPosition.x;
            StartCoroutine(SetupText());
            SetupButton();
            BtnExpand.onClick.Invoke();
        }
        else
            VoteBox.SetActive(false);
    }

    /// <summary>
    /// Gán chức năng button
    /// </summary>
    private void SetupButton()
    {
        //Button vote
        BtnVote.onClick.AddListener(() =>
        {
            if (HomeController != null)//Nếu giao diện đặt tại home
            {
                HomeController.ButtonFunctions(14);//Gọi show vote từ homecontroller
            }

            //Set đã vote
            DataUserController.User.VoteCode = GlobalVariables.VoteCode;
            DataUserController.SaveUserInfor();
            VoteBox.SetActive(false);
        });

        //Button Calcel
        BtnCancel.onClick.AddListener(() =>
        {
            DataUserController.User.VoteCode = GlobalVariables.VoteCode;
            DataUserController.SaveUserInfor();
            VoteBox.SetActive(false);
        });

        //Button expand/collapse
        BtnExpand.onClick.AddListener(() =>
        {
            StartCoroutine(GameSystem.MoveObjectCurve(true, VoteBox, VoteBox.transform.localPosition, new Vector2(IsExpand ? PosOriginal : PosOriginal + MoveRange, VoteBox.transform.localPosition.y), .5f, GameSystem.AnimCurve));
            BtnExpand.transform.localScale = new Vector3(1, IsExpand ? -1 : 1, 1);
            IsExpand = !IsExpand;
        });
    }

    /// <summary>
    /// Thiết lập text giao diện
    /// </summary>
    private IEnumerator SetupText()
    {
        yield return new WaitForSeconds(.1f);
        ContentText.text = Languages.lang[348];//"Bạn muốn chức năng gì sẽ xuất hiện trong phiên bản tiếp theo";
        VoteText.text = Languages.lang[349];//"Bầu chọn";
        CancelText.text = Languages.lang[67];//"Hủy bỏ";
    }
}
