using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class VoteFunctionController : MonoBehaviour
{
    //[Title("Các object cần phải thiết lập")]
    //[LabelText("Home Controller (nếu đặt ở home)")]
    //public Home HomeController;

    //Vote1
    [FoldoutGroup("Cài đặt chức năng vote")]
    [HorizontalGroup("Cài đặt chức năng vote/Horizontal")]
    [BoxGroup("Cài đặt chức năng vote/Horizontal/Vote 1")]
    [LabelText("Tiêu đề vote")]
    [Required]
    public Text TitleVote1;
    [FoldoutGroup("Cài đặt chức năng vote")]
    [HorizontalGroup("Cài đặt chức năng vote/Horizontal")]
    [BoxGroup("Cài đặt chức năng vote/Horizontal/Vote 1")]
    [LabelText("Hình ảnh vote")]
    [Required]
    public Image ImgVote1;
    [FoldoutGroup("Cài đặt chức năng vote")]
    [HorizontalGroup("Cài đặt chức năng vote/Horizontal")]
    [BoxGroup("Cài đặt chức năng vote/Horizontal/Vote 1")]
    [LabelText("Nội dung vote")]
    [Required]
    public Text ContentVote1;
    [FoldoutGroup("Cài đặt chức năng vote")]
    [HorizontalGroup("Cài đặt chức năng vote/Horizontal")]
    [BoxGroup("Cài đặt chức năng vote/Horizontal/Vote 1")]
    [LabelText("Nút bấm vote")]
    [Required]
    public Button BtnVote1;
    [FoldoutGroup("Cài đặt chức năng vote")]
    [HorizontalGroup("Cài đặt chức năng vote/Horizontal")]
    [BoxGroup("Cài đặt chức năng vote/Horizontal/Vote 1")]
    [LabelText("Text nút bấm")]
    [Required]
    public Text TextBtnVote1;

    //Vote2
    [FoldoutGroup("Cài đặt chức năng vote")]
    [HorizontalGroup("Cài đặt chức năng vote/Horizontal")]
    [BoxGroup("Cài đặt chức năng vote/Horizontal/Vote 2")]
    [LabelText("Tiêu đề vote")]
    [Required]
    public Text TitleVote2;
    [FoldoutGroup("Cài đặt chức năng vote")]
    [HorizontalGroup("Cài đặt chức năng vote/Horizontal")]
    [BoxGroup("Cài đặt chức năng vote/Horizontal/Vote 2")]
    [LabelText("Hình ảnh vote")]
    [Required]
    public Image ImgVote2;
    [FoldoutGroup("Cài đặt chức năng vote")]
    [HorizontalGroup("Cài đặt chức năng vote/Horizontal")]
    [BoxGroup("Cài đặt chức năng vote/Horizontal/Vote 2")]
    [LabelText("Nội dung vote")]
    [Required]
    public Text ContentVote2;
    [FoldoutGroup("Cài đặt chức năng vote")]
    [HorizontalGroup("Cài đặt chức năng vote/Horizontal")]
    [BoxGroup("Cài đặt chức năng vote/Horizontal/Vote 2")]
    [LabelText("Nút bấm vote")]
    [Required]
    public Button BtnVote2;
    [FoldoutGroup("Cài đặt chức năng vote")]
    [HorizontalGroup("Cài đặt chức năng vote/Horizontal")]
    [BoxGroup("Cài đặt chức năng vote/Horizontal/Vote 2")]
    [LabelText("Text nút bấm")]
    [Required]
    public Text TextBtnVote2;


    // Start is called before the first frame update
    void Start()
    {
        SetupText();
        BtnVote1.onClick.AddListener(() =>
        {
            VoteFunction(0);
        });
        BtnVote2.onClick.AddListener(() =>
        {
            VoteFunction(1);
        });
    }

    /// <summary>
    /// Cài đặt giao diện ngôn ngữ
    /// </summary>
    private void SetupText()
    {
        TitleVote1.text = Languages.lang[350];
        TitleVote2.text = Languages.lang[352];
        ContentVote1.text = Languages.lang[351];
        ContentVote2.text = Languages.lang[353];
        TextBtnVote1.text = TextBtnVote2.text = Languages.lang[349];
    }

    /// <summary>
    /// Bình chọn cho chức năng
    /// </summary>
    /// <param name="type"></param>
    private void VoteFunction(int type)
    {
        StartCoroutine(SyncData.VoteFunction(type.Equals(0) ? "Chatbox" : "AddNewHero"));
        GameSystem.DisposePrefabUI(13);
        GameSystem.ControlFunctions.ShowMessage(Languages.lang[354]);
    }
}
