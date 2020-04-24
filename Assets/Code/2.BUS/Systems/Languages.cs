//Credit: music: Music by Eric Matyas - www.soundimage.org
//Home image: Erin Linnnn
public static class Languages {

    #region Variables
    public static string[] lang = new string[1000];
    public static string[] ItemName = new string[600];
    public static string[] ItemInfor = new string[600];
    public static string[] ItemNameType0 = new string[100];
    public static string[] ItemInforType0 = new string[100];
    public static string[] ItemNameType1 = new string[10];
    public static string[] ItemNameType2 = new string[10];
    public static string[] ItemNameType3 = new string[10];
    public static string[] ItemNameType4 = new string[10];
    public static string[] ItemNameType5 = new string[10];
    public static string[] ItemNameType6 = new string[10];
    public static string[] ItemNameType7 = new string[10];
    public static string[] ItemNameType8 = new string[10];
    public static string[] ItemNameType10 = new string[50];
    public static string[] ItemInforType10 = new string[50];
    public static string[] ItemNameType11 = new string[50];
    public static string[] ItemInforType11 = new string[50];
    public static string[] ItemNameType12 = new string[34];
    public static string[] ItemInforType12 = new string[34];
    public static string[] ItemInforType1 = new string[10];
    public static string[] HeroSkillName = new string[20]; //Tên skill của các hero
    public static string[] HeroSkillDescription = new string[20]; //Giới thiệu skill của các hero
    public static string[] HeroIntrinsic = new string[HeroSkillDescription.Length]; //Giới thiệu nội tại của các hero
    public static string[] IntroductionTitle = new string[10];
    public static string[] IntroductionDescriptions = new string[10];
    #endregion

    // Use this for initialization
    //public void Start () {
    //       lang = new string[2000];
    //       Language_VN();
    //}
    public static void SetupLanguage (int _idlanguage) {
        switch (_idlanguage) {
            case 0:
                Language_EN ();
                break;
            case 1:
                Language_VN ();
                break;
            default:
                Language_EN ();
                break;
        }

    }
    public static void SetupDefaultLanguage () {
        if (string.IsNullOrEmpty (lang[11]))
            SetupLanguage (1);
    }
    private static void Language_VN () {
        //0-99: information game
        //100-199: player name near
        //200-299: player near descript
        //300-399: player name magic
        //400-499: player magic descript
        //500-599: player name far
        //600-699: player far descript

        #region Phần ngôn ngữ tổng quan 

        lang[0] = "";
        lang[1] = "";
        lang[2] = "Sắp ra mắt...!";
        lang[11] = "Thông tin";
        lang[12] = "Trang bị";
        lang[13] = "Kỹ năng";
        lang[14] = "Đặc biệt";
        lang[15] = "Tiểu sử";
        lang[16] = "Cận chiến";
        lang[17] = "Sát thủ";
        lang[18] = "Hỗ trợ";
        lang[19] = "Đỡ đòn";
        lang[20] = "Xạ thủ";
        lang[21] = "Pháp sư";
        lang[22] = "Chỉ số";
        lang[23] = "Lv: ";
        lang[24] = "Máu";
        lang[25] = "Thể lực";
        lang[26] = "S.thương vật lý";
        lang[27] = "S.thương phép";
        lang[28] = "Giáp";
        lang[29] = "Kháng phép";
        lang[30] = "Hút máu";
        lang[31] = "Hút máu phép";
        lang[32] = "Xuyên giáp (%)";
        lang[33] = "Xuyên phép (%)";
        lang[34] = "Hồi máu mỗi " + ItemCoreSetting.SecondHeathRegen.ToString () + " giây";
        lang[35] = "Hồi thể lực mỗi giây";
        lang[36] = "Chí mạng (%)";
        lang[37] = "+ h.ứng gây ra";
        lang[38] = "Kháng hiệu ứng (%)";
        lang[39] = "Tốc độ đánh";
        lang[40] = "%";
        lang[41] = "/s";
        lang[42] = "/cấp";
        lang[43] = "Giảm t.gian hồi chiêu";
        lang[45] = "T.gian hồi chiêu";
        lang[46] = "Thể lực kỹ năng";
        lang[47] = "+ ";
        lang[48] = "Gỡ bỏ";

        //Setting --Window setting
        lang[50] = "Cài đặt";
        lang[51] = "Đồ họa";
        lang[52] = "Cực thấp";
        lang[53] = "Thấp";
        lang[54] = "Trung bình";
        lang[55] = "Cao";
        lang[56] = "Audio";
        lang[57] = "Nhạc nền";
        lang[58] = "Âm thanh";
        lang[59] = "Ngôn ngữ";
        //--------Button
        lang[60] = "Chấp nhận";
        lang[61] = "Chọn NV";
        lang[62] = "Phản hồi";
        lang[63] = "Thoát game";
        lang[64] = "Tiêu đề...";
        lang[65] = "Nội dung...";
        //--------Window feedback
        lang[66] = "Gửi";
        lang[67] = "Hủy bỏ";
        lang[68] = "Email của bạn...";
        lang[69] = "";
        lang[70] = "Bạn chưa nhập tiêu đề";
        lang[71] = "Bạn chưa nhập nội dung";
        lang[72] = "<color=green>Cám ơn phản hồi của bạn!</color>";
        lang[73] = "Phản hồi của bạn chưa được gửi đi, kiểm tra lại mạng xem sao";
        //---------Inventory
        lang[74] = "Sử dụng: ";
        lang[75] = "Số lượng: ";
        lang[76] = "Giá bán: ";
        lang[77] = "Không thao tác được khi đang ở chế độ lựa chọn vật phẩm";
        lang[78] = "Trang bị nhanh";
        lang[79] = "Gỡ nhanh";
        lang[80] = "Đã gỡ trang bị";

        //Scene Room
        lang[100] = "Vào trận";
        lang[101] = "Phát hiện thấy phiên bản {0} trên máy chủ, bạn có muốn tải và cập nhật phiên bản mới ?";
        lang[102] = "Bạn muốn thoát trò chơi?";
        lang[103] = "<color=green>Chuyển đổi ngôn ngữ thành công, vui lòng tải lại màn hình.</color>";
        lang[104] = "Nhập mã phòng";
        lang[110] = " tạo thành công phòng ";
        lang[111] = "Đã vào phòng ";
        lang[112] = " đã kết nối";
        lang[113] = "Bạn cùng phòng đã thoát";
        lang[114] = "Chờ đồng đội sẵn sàng";
        lang[115] = "Sẵn sàng";
        lang[116] = "Không thực hiện được";
        lang[117] = "Không kết nối được tới máy chủ";
        lang[118] = "Đội hình";
        lang[119] = "Cá nhân";

        //Phần kết nối
        lang[120] = "Không kết nối được máy chủ...";
        lang[121] = "Vui lòng nhập tên";
        lang[122] = "Nhập tên của bạn";
        lang[123] = "Đội hình của bạn chưa đủ nhân vật";
        lang[124] = "Tiết kiệm pin";
        lang[125] = " - Mở";
        lang[126] = " - Tắt";
        lang[127] = "Đang thiết lập hệ thống trò chuyện...!";
        lang[128] = "Không thể trang bị thêm";
        lang[129] = "Trang bị thành công";
        //Scene Inventory
        lang[130] = "Xác nhận bán {0} vật phẩm với {1} vàng";
        lang[131] = "Xác nhận bán vật phẩm này với giá {0} vàng";
        lang[132] = "Xem";
        lang[133] = "tất cả";
        lang[134] = "Trang bị";
        lang[135] = "Vật phẩm";
        lang[136] = "Nhiệm vụ";
        lang[137] = "Lựa chọn";
        lang[138] = "Bán";
        lang[139] = "Sắp xếp";
        lang[140] = "Không đủ chỗ trống trong thùng đồ";
        lang[141] = "<color=green>Chế tạo thành công</color>";
        lang[142] = "<color=red>Lỗi - Không thể chế tạo đồ</color>";
        lang[143] = "<color=red>Không đủ nguyên liệu để chế tạo</color>";
        lang[144] = "Túi đồ";
        lang[145] = "Chế tạo";
        lang[146] = "Nhân vật";
        lang[147] = "Chế tạo hết";
        lang[148] = "Yêu cầu";
        lang[149] = "<color=red>Không đủ tài nguyên</color>";

        lang[150] = "Đất";
        lang[151] = "Nước";
        lang[152] = "Lửa";
        lang[153] = "Gió";

        lang[154] = "Nâng cấp";
        lang[155] = "Nâng phẩm";
        lang[156] = "Phân giải";
        lang[157] = "<color=green>Nâng cấp thành công</color>";
        lang[158] = "<color=red>Trang bị đã được nâng cấp tối đa</color>";
        lang[159] = "<color=green>Nâng phẩm thành công</color>";
        lang[160] = "<color=red>Trang bị đã được nâng phẩm tối đa</color>";
        lang[161] = "Nút bấm kỹ năng bên phải";
        lang[162] = "Cài đặt hệ thống";
        lang[163] = "Cài đặt trận đấu";
        lang[164] = "Hiệu ứng chuyển động chậm khi dùng kỹ năng";
        lang[165] = "Nội tại và kỹ năng";
        lang[166] = "<color=green>Nội tại</color>";
        lang[167] = "<color=orange>Kỹ năng</color>";
        lang[168] = "<color=#454523>Rừng rậm</color>";
        lang[169] = "<color=#BDAD62>Đồng bằng</color>";
        lang[170] = "<color=#FF5410>Núi lửa</color>";
        lang[171] = "Núi tuyết";
        lang[172] = "<color=red>Địa ngục</color>";
        lang[173] = "<color=green>Hang độc</color>";
        lang[174] = "<color=#3F42AF>Hang ma</color>";
        lang[175] = "<color=red>Vượt quá giới hạn rương đồ</color>";
        lang[176] = "Chiến thắng";
        lang[177] = "Thất bại";
        lang[178] = "EXP";
        lang[179] = "Tạm dừng\nBạn muốn thoát khỏi trận đấu?";
        lang[180] = "Bạn muốn video để mở rộng thùng đồ?";
        lang[181] = "Hãy chờ video tiếp theo được tải nhé";
        lang[182] = "Xem video để nhận thưởng";
        lang[183] = "Xem";
        lang[184] = "Chinh chiến";
        lang[185] = "Trang bị vật lý";
        lang[186] = "Trang bị phép thuật";
        lang[187] = "Trang bị phòng thủ";
        lang[188] = "Vật dụng tiêu hao";
        lang[189] = "Trang bị đặc biệt";
        lang[190] = "Bạn nhận được <color=#FF5410>{0}</color>";
        lang[191] = "<color=red>Không đủ Lông phượng để thực hiện</color>";
        lang[192] = "Tự quay";
        lang[193] = "Dừng quay";
        lang[194] = "Hướng dẫn";
        lang[195] = "- Yêu cầu tối thiểu cho 1 lần quay thưởng là " + Module.SpinItemQuantity.ToString () + " <color=red>Lông phượng</color>";
        lang[195] += "\n\n- Lông phượng có thể tìm ở bất kỳ đâu trong lục địa";
        lang[195] += "\n\n- Phần thưởng quay được có thể là bất kỳ vật phẩm nào, từ giá trị thấp tới cực kỳ quý hiếm.";
        lang[195] += "\n\n- Chú ý: Sắp xếp thùng đồ của bạn trước khi quay";
        lang[196] = "Chỉ số của trang bị sẽ ngẫu nhiên khi chế tạo";
        lang[197] = "Xác nhận";
        lang[198] = "Thuê sát thủ";
        lang[199] = "Xác nhận thuê sát thủ với {0} đá quý";
        lang[200] = "Máu";
        lang[201] = "Năng lượng";
        lang[202] = "Sát thương vật lý";
        lang[203] = "Sát thương phép thuật";
        lang[204] = "Giáp";
        lang[205] = "kháng phép";
        lang[206] = "Hồi máu mỗi " + ItemCoreSetting.SecondHeathRegen + " giây";
        lang[207] = "Hồi năng lượng mỗi giây";
        lang[208] = "Sát thương hệ đất";
        lang[209] = "Sát thương hệ nước";
        lang[210] = "Sát thương hệ lửa";
        lang[211] = "Kháng hệ đất";
        lang[212] = "Kháng hệ nước";
        lang[213] = "Kháng hệ hỏa";
        lang[214] = "Tốc độ đánh";
        lang[215] = "Thời gian hồi chiêu";
        lang[216] = "Hút máu (%)";
        lang[217] = "Hút máu phép(%)";
        lang[218] = "Xuyên giáp (%)";
        lang[219] = "Xuyên phép (%)";
        lang[220] = "Chí mạng (%)";
        lang[221] = "Kháng hiệu ứng (%)";
        lang[222] = "Giảm thời gian hồi chiêu (%)";
        lang[223] = "Sát thương hoàn hảo (%)";
        lang[224] = "Phòng thủ hoàn hảo  (%)";
        lang[225] = "Tỉ lệ x2 đòn đánh (%)";
        lang[226] = "Tỉ lệ x3 đòn đánh (%)";
        lang[227] = "Phản sát thương (%)";
        lang[228] = "Tăng lượng vàng rơi ra (%)";
        lang[229] = "Năng lượng đòn đánh thường";
        lang[230] = "Năng lượng dùng kỹ năng";
        lang[231] = "Máu mỗi cấp độ";
        lang[232] = "Sát thương vật lý/cấp";
        lang[233] = "Sức mạnh phép thuật/cấp";
        lang[234] = "Giáp/cấp";
        lang[235] = "Kháng phép/cấp";
        lang[236] = "Hồi máu/cấp";
        lang[237] = "Hồi năng lượng/cấp";
        lang[238] = "Giảm thời gian hồi chiêu/cấp";
        lang[250] = "Bạn không đủ đá quý";
        lang[251] = "Thuê sát thủ thành công!";
        lang[252] = "Xem trước";
        lang[253] = "Ngẫu nhiên";
        lang[254] = "Sinh tử";
        lang[255] = "Chế độ này cho phép bạn biết trước đội hình và trang bị của đối phương, để có thể lựa chọn những chiến thuật và trang bị phù hợp nhất cho mình";
        lang[256] = "Bạn sẽ không biết trước được mình sẽ chạm trán với ai ở chế độ này, tất nhiên phần thưởng bạn nhận được nếu vượt qua cũng sẽ cao hơn";
        lang[257] = "Hãy chiến đấu đến khi gục ngã, bảng đếm số sẽ luôn chờ đợi bạn.";
        lang[258] = "Vàng: ";
        lang[259] = "Đá quý: ";
        lang[260] = ". Có thể tìm thấy tại ";
        lang[261] = "Xóa";
        lang[262] = "Thông tin";
        lang[263] = "Trang bị";
        lang[264] = "Đổi trang bị";
        lang[265] = "Lựa chọn trang bị";
        lang[266] = "Không thể đổi trang bị cho chính mình";
        lang[267] = "Nhân vật chưa được trang bị";
        lang[268] = "Đổi trang bị thành công";
        lang[269] = "Đập bóng";
        lang[270] = "Búa: ";
        lang[271] = "Quay thưởng";
        lang[272] = "Đập bóng là một tính năng cần kết nối mạng. Mỗi ngày, bạn sẽ có 5 chiếc búa để đập bóng"; //Hướng dẫn break ball, lấy từ server
        lang[273] = "Không thể phân giải vật phẩm này";
        lang[274] = "Túi đồ";
        lang[275] = "Nhận ";
        lang[276] = " vàng";
        lang[277] = " đá quý";
        lang[278] = "Xác nhận phân giải trang bị này?";
        lang[279] = "Thưởng Online";
        lang[280] = "Nhận";
        lang[281] = "Đội hình đã đủ";
        lang[282] = "Lựa chọn loại vật phẩm chế tạo";
        lang[283] = "Lựa chọn vật phẩm chế tạo";
        lang[284] = "Đây là những nguyên liệu cần thiết để chế tạo";
        lang[285] = "Với các vật phẩm sử dụng, bạn có thể chế tạo toàn bộ nếu đủ nguyên liệu";
        lang[286] = "Lựa chọn để lọc tướng theo hệ";
        lang[287] = "Danh sách tướng đã lọc theo hệ";
        lang[288] = "Lựa chọn các chức năng để thao tác";
        lang[289] = "Phần thông tin chứa các chi tiết thông tin chỉ số về nhân vật";
        lang[290] = "Bạn có thể trang bị cho nhân vật ở phần này";
        lang[291] = "Thông tin về kỹ năng và nội tại của nhân vật";
        lang[292] = "Tiểu sử, quá khứ và cốt truyện của nhân vật";
        lang[293] = "Các hiệu ứng đặc biệt mà nhân vật này sở hữu";
        lang[294] = "Bắt đầu";
        lang[295] = "Không thể khảm thêm ngọc";
        lang[296] = "Không thể khảm ngọc";
        lang[297] = "Khảm ngọc";
        lang[298] = "Gỡ khảm";
        lang[299] = "Đục lỗ";
        lang[300] = "<color=red>Bạn muốn phá hủy ngọc này?</color>";
        lang[301] = "Ngọc đã bị phá hủy";
        lang[302] = "Lỗ";
        lang[303] = "Không thể đục lỗ";
        lang[304] = "Bạn không đủ vàng";
        lang[305] = "Tháp địa ngục";
        lang[306] = "Xác nhận rời khỏi màn chơi?";
        lang[307] = "Bạn cần chọn 1 lá bài hoặc bỏ lượt";
        lang[308] = "Lá bài không phù hợp";
        lang[309] = "Tùy chọn cách chơi";
        lang[310] = "Chọn bài đánh nhanh";
        lang[311] = "Chọn màu muốn đổi";
        lang[312] = "Đánh";
        lang[313] = "Bỏ lượt";
        lang[314] = "Thoát";
        lang[315] = "Chơi";
        lang[316] = "Xếp hạng";
        lang[317] = "Đặt cược";
        lang[318] = "Đối thủ";
        lang[319] = "Tùy chọn";
        lang[320] = "Màu nền";
        lang[321] = "Người chơi ";
        lang[322] = "Đánh bạc";
        lang[323] = "Bỏ lượt nhanh";
        lang[324] = "Hỗ trợ hiển thị lá bài được đánh";
        lang[325] = "Lá bài số";
        lang[326] = "Các lá bài này được chia đều ra 4 màu: đỏ, vàng, lục, lam. Chúng được đánh số lớn dần từ 0 đến 9";
        lang[327] = "Lá bài chức năng";
        lang[328] = "Lá bài mất lượt: Khi bạn đánh lá bài này, người chơi tiếp theo sẽ bị mất lượt";
        lang[329] = "Lá bài đảo chiều: Khi quân bài này được đánh xuống sẽ tiến hành Đảo ngược chiều đánh bài của cuộc chơi. Mặc định khi khai cuộc là vòng theo chiều kim đồng hồ";
        lang[330] = "Lá bài +2: Khi đánh quân bài này xuống, người kế tiếp bắt buộc bốc 2 lá bài và mất lượt";
        lang[331] = "Lá bài đổi màu: Khi đánh quân bài này, bạn có quyền lựa chọn màu sắc cho lượt đánh tiếp theo";
        lang[332] = "Các lá bài mở rộng";
        lang[333] = "Đây là những lá bài mở rộng so với bộ bài cơ bản để tăng thêm phần thú vị trong quá trình chơi";
        lang[334] = "Lá bài mất lượt x2: Bỏ qua liên tiếp 2 lượt chơi, nếu chỉ có 2 người chơi trong bàn, nó sẽ chỉ có tác dụng như lá bài mất lượt bình thường";
        lang[335] = "Lá bài bão tố: Khi đánh lá bài này, đối phương tiếp theo buộc phải bốc bài cho tới khi bốc được lá bài trùng màu với lá bão tố đã đánh hoặc bốc được lá bài đổi màu, và mất lượt";
        lang[336] = "Lá bài xả màu: Khi đánh lá bài này, bạn được phép xả hết tất cả các lá bài cùng màu";
        lang[337] = "Lá bài tấn công bất ngờ: Đổi màu ván bài tùy ý, buộc đối phương bốc thêm 4 lá, tuy nhiên được phép chọn đối phương bất kỳ để tấn công.";
        lang[338] = "Lá bài ngẫu nhiên: Buộc đối phương phải bốc ngẫu nhiên từ 1-9 lá bài và mất lượt";
        lang[339] = "Khi bắt đầu, ván bài sẽ được chơi theo chiều kim đồng hồ, người chơi đầu tiên đánh quân bài trùng màu với yêu cầu xuống bàn, Người chơi tiếp theo cần tiền hành đánh một quân bài tiếp nối với quân bài của người chơi đầu tiên. Mỗi lần người chơi chỉ được đánh một quân bài trong mỗi lượt của mình dựa theo nguyên tắc sau: Người chơi đánh quân bài xuống bằng cách sử dụng một quân bài cùng số hoặc cùng màu với quân bài của người chơi trước đó ( bao gồm cả lá số và lá chức năng có một màu đều được). Hoặc người chơi có thể đánh các quân bài có chức năng đổi màu.\n\n" +
            "- Nếu không đánh được bài xuống thì người chơi phải bốc 1 lá bài. Nếu lá bài đó đúng với nguyên tắc đánh bài, người chơi có thể đánh luôn lá bài đó. Còn nếu không, người chơi sẽ bị mất lượt.\n" +
            "- Người chơi sẽ thay phiên nhau đánh các là bài theo lượt cho đến khi người chơi đánh hết lá bài trên tay của mình xuống.\n\n" +
            "Một ván bài kết thúc khi có người bỏ được hết bài xuống. Khi đó tiến hành đếm điểm của những người chơi còn lại. Người thắng cuộc sẽ được có toàn bộ số điểm này. Số điểm này có thể quy ra phần thưởng sau này.\n\n" +
            "Cách tính điểm trong trò chơi như sau:\n" +
            "- Cộng tổng điểm của các lá bài trên tay người thua cuộc." +
            "- Các lá bài số (0-9) được tính điểm bằng với số ghi trên lá bài." +
            "- Các lá bài chức năng như +2, Cấm lượt, Đổi chiều... được tính 20 điểm." +
            "- Các lá bài Đổi màu, Đổi màu +4... được tính 50 điểm";
        lang[340] = "Cơ bản";
        lang[341] = "Mở rộng";
        lang[342] = "Cách chơi";
        lang[343] = "Lá bài đổi màu +4: Bạn có quyền lựa chọn màu sắc cho lượt tiếp theo, đồng thời buộc người chơi tiếp theo bốc 4 lá bài và mất lượt";
        lang[344] = "Thông tin cách chơi";
        lang[345] = "Tự bốc bài khi không có lá phù hợp";
        lang[346] = "Vui lòng nhập tên";
        lang[347] = "Tên không được chứa ký tự đặc biệt";

        //Vote setup
        lang[348] = "Bạn muốn chức năng gì sẽ xuất hiện trong phiên bản tiếp theo";
        lang[349] = "Bầu chọn";
        lang[350] = "Chatbox";
        lang[351] = "Chatbox là chức năng giúp bạn có thể trò chuyện giữa những người chơi cùng nhau, tuy đây là trò chơi offline, nhưng chức năng chatbox sẽ yêu cầu thiết bị của bạn phải được kết nối internet";
        lang[352] = "Thêm nhân vật mới";
        lang[353] = "Thêm một nhân vật mới bổ sung vào danh sách";
        lang[354] = "Cám ơn bình chọn của bạn";
        //=============================================
        lang[355] = "Có vẻ như đây là lần đầu tiên chơi game, bạn có muốn một chút hướng dẫn?";
        lang[356] = "Mua";
        lang[357] = "Mua thành công";
        lang[358] = "Mở rộng 1 ô trống trong thùng đồ với {0} đá quý?";
        lang[359] = "Túi đồ +1 ô";

        //Thông tin chỉ số
        lang[700] = "+{0} Sát thương vật lý";
        lang[701] = "+{0} Sức mạnh phép thuật";
        lang[702] = "+{0} Máu";
        lang[703] = "+{0} Năng lượng";
        lang[704] = "+{0} Giáp";
        lang[705] = "+{0} Kháng phép";
        lang[706] = "<color=darkblue>+{0} Hồi máu mỗi " + ItemCoreSetting.SecondHeathRegen + " giây</color>";
        lang[707] = "<color=darkblue>+{0} Hồi năng lượng mỗi giây</color>";
        lang[708] = "<color=darkblue>+{0} Sát thương hệ thổ</color>";
        lang[709] = "<color=darkblue>+{0} Sát thương hệ nước</color>";
        lang[710] = "<color=darkblue>+{0} Sát thương hệ lửa</color>";
        lang[711] = "<color=darkblue>+{0} Kháng sát thương hệ thổ</color>";
        lang[712] = "<color=darkblue>+{0} Kháng sát thương hệ nước</color>";
        lang[713] = "<color=darkblue>+{0} Kháng sát thương hệ lửa</color>";
        lang[714] = "<color=darkblue>+{0}% Tốc độ tấn công</color>";
        lang[715] = "<color=darkblue>+{0}% Hút máu</color>";
        lang[716] = "<color=darkblue>+{0}% Hút máu phép</color>";
        lang[717] = "<color=darkblue>+{0}% Xuyên giáp</color>";
        lang[718] = "<color=darkblue>+{0}% Xuyên phép</color>";
        lang[719] = "<color=darkblue>+{0}% Chí mạng</color>";
        lang[720] = "<color=darkblue>+{0}% Kháng hiệu ứng</color>";
        lang[721] = "<color=darkblue>+{0}% Giảm t.gian hồi chiêu</color>";
        lang[722] = "<color=darkblue>+{0}% Sát thương vật lý</color>";
        lang[723] = "<color=darkblue>+{0}% Sức mạnh phép thuật</color>";
        lang[724] = "<color=darkblue>+{0}% Máu</color>";
        lang[725] = "<color=darkblue>+{0}% Năng lượng</color>";
        lang[726] = "<color=red>+{0}% Sát thương hoàn hảo</color>";
        lang[727] = "<color=red>+{0}% Phòng thủ hoàn hảo</color>";
        lang[728] = "<color=red>+{0}% Tỉ lệ x2 đòn đánh</color>";
        lang[729] = "<color=red>+{0}% Tỉ lệ x3 đòn đánh</color>";
        lang[730] = "<color=red>+{0}% Phản sát thương</color>";
        lang[731] = "<color=red>+{0}% Vàng nhận được sau trận đấu</color>";
        lang[732] = "<color=purple>+{0} Lỗ khảm ngọc</color>";
        lang[733] = "<color=purple>- (Trống)</color>";
        lang[734] = "<color=darkblue>+{0}% Giáp</color>";
        lang[735] = "<color=darkblue>+{0}% Kháng phép</color>";

        #endregion

        #region Tên item type 0 

        ItemNameType0[0] = "Dao ngắn 1";
        ItemNameType0[1] = "Dao ngắn 2";
        ItemNameType0[2] = "Dao ngắn 3";
        ItemNameType0[3] = "Dao ngắn 4";
        ItemNameType0[4] = "Dao ngắn 5";
        ItemNameType0[5] = "Dao ngắn 6";
        ItemNameType0[6] = "Dao ngắn 7";
        ItemNameType0[7] = "Kiếm 1";
        ItemNameType0[8] = "Kiếm 2";
        ItemNameType0[9] = "Kiếm 3";
        ItemNameType0[10] = "Kiếm 4";
        ItemNameType0[11] = "Kiếm 5";
        ItemNameType0[12] = "Kiếm 6";
        ItemNameType0[13] = "Kiếm 7";
        ItemNameType0[14] = "Kiếm 8";

        ItemNameType0[15] = "Rìu 1";
        ItemNameType0[16] = "Rìu 2";
        ItemNameType0[17] = "Rìu 3";
        ItemNameType0[18] = "Rìu 4";
        ItemNameType0[19] = "Rìu 5";
        ItemNameType0[20] = "Rìu 6";
        ItemNameType0[21] = "Rìu 7";
        ItemNameType0[22] = "Rìu 8";
        ItemNameType0[23] = "Rìu 9";
        ItemNameType0[24] = "Rìu 10";
        ItemNameType0[25] = "Rìu 11";
        ItemNameType0[26] = "Rìu 12";

        ItemNameType0[27] = "Búa 1";
        ItemNameType0[28] = "Búa 2";
        ItemNameType0[29] = "Búa 3";
        ItemNameType0[30] = "Búa 4";
        ItemNameType0[31] = "Búa 5";
        ItemNameType0[32] = "Búa 6";
        ItemNameType0[33] = "Búa 7";
        ItemNameType0[34] = "Búa 8";
        ItemNameType0[35] = "Búa 9";
        ItemNameType0[36] = "Búa 10";
        ItemNameType0[37] = "Búa 11";
        ItemNameType0[38] = "Búa 12";

        ItemNameType0[39] = "Chùy 1";
        ItemNameType0[40] = "Chùy 2";
        ItemNameType0[41] = "Chùy 3";
        ItemNameType0[42] = "Chùy 4";
        ItemNameType0[43] = "Chùy 5";
        ItemNameType0[44] = "Chùy 6";
        ItemNameType0[45] = "Chùy 7";
        ItemNameType0[46] = "Chùy 8";
        ItemNameType0[47] = "Chùy 9";
        ItemNameType0[48] = "Chùy 10";
        ItemNameType0[49] = "Chùy 11";
        ItemNameType0[50] = "Cung 1";
        ItemNameType0[51] = "Cung 2";
        ItemNameType0[52] = "Cung 3";
        ItemNameType0[53] = "Cung 4";
        ItemNameType0[54] = "Cung 5";
        ItemNameType0[55] = "Nỏ 1";
        ItemNameType0[56] = "Nỏ 2";
        ItemNameType0[57] = "Nỏ 3";
        ItemNameType0[58] = "Nỏ 4";
        ItemNameType0[59] = "Súng 1";
        ItemNameType0[60] = "Súng 2";
        ItemNameType0[61] = "Súng 3";
        ItemNameType0[62] = "Súng 4";
        ItemNameType0[63] = "Súng 5";
        ItemNameType0[64] = "Súng 6";
        ItemNameType0[65] = "Súng 7";
        ItemNameType0[66] = "Súng 8";

        #endregion

        #region Tên item type 1 

        ItemNameType1[0] = "Trượng 1";
        ItemNameType1[1] = "Trượng 2";
        ItemNameType1[2] = "Trượng 3";
        ItemNameType1[3] = "Trượng 4";
        ItemNameType1[4] = "Trượng 5";
        ItemNameType1[5] = "Trượng 6";
        ItemNameType1[6] = "Trượng 7";
        ItemNameType1[7] = "Trượng 8";

        #endregion

        #region Tên item type 2 

        ItemNameType2[0] = "Áo choàng 1";
        ItemNameType2[1] = "Áo choàng 2";
        ItemNameType2[2] = "Áo choàng 3";
        ItemNameType2[3] = "Áo choàng 4";
        ItemNameType2[4] = "Áo choàng 5";
        ItemNameType2[5] = "Áo choàng 6";

        #endregion

        #region Tên item type 3 

        ItemNameType3[0] = "Nhẫn 1";
        ItemNameType3[1] = "Nhẫn 2";
        ItemNameType3[2] = "Nhẫn 3";
        ItemNameType3[3] = "Nhẫn 4";
        ItemNameType3[4] = "Nhẫn 5";

        #endregion

        #region Tên item type 4 

        ItemNameType4[0] = "Giáp 1";
        ItemNameType4[1] = "Giáp 2";
        ItemNameType4[2] = "Giáp 3";
        ItemNameType4[3] = "Giáp 4";
        ItemNameType4[4] = "Giáp 5";
        ItemNameType4[5] = "Giáp 6";
        ItemNameType4[6] = "Giáp 7";
        ItemNameType4[7] = "Giáp 8";
        ItemNameType4[8] = "Giáp 9";
        ItemNameType4[9] = "Giáp 10";

        #endregion

        #region Tên item type 5 

        ItemNameType5[0] = "Đai lưng 1";
        ItemNameType5[1] = "Đai lưng 2";
        ItemNameType5[2] = "Đai lưng 3";
        ItemNameType5[3] = "Đai lưng 4";
        ItemNameType5[4] = "Đai lưng 5";
        ItemNameType5[5] = "Đai lưng 6";

        #endregion

        #region Tên item type 6 

        ItemNameType6[0] = "Giáp tay 1";
        ItemNameType6[1] = "Giáp tay 2";
        ItemNameType6[2] = "Giáp tay 3";
        ItemNameType6[3] = "Giáp tay 4";
        ItemNameType6[4] = "Giáp tay 5";
        ItemNameType6[5] = "Giáp tay 6";
        ItemNameType6[6] = "Giáp tay 7";
        ItemNameType6[7] = "Giáp tay 8";
        ItemNameType6[8] = "Giáp tay 9";
        ItemNameType6[9] = "Giáp tay 10";

        #endregion

        #region Tên item type 7 

        ItemNameType7[0] = "Găng tay 1";
        ItemNameType7[1] = "Găng tay 2";
        ItemNameType7[2] = "Găng tay 3";
        ItemNameType7[3] = "Găng tay 4";
        ItemNameType7[4] = "Găng tay 5";

        #endregion

        #region Tên item type 8 

        ItemNameType8[0] = "Khiên 1";
        ItemNameType8[1] = "Khiên 2";
        ItemNameType8[2] = "Khiên 3";
        ItemNameType8[3] = "Khiên 4";
        ItemNameType8[4] = "Khiên 5";

        #endregion

        #region Tên item type 10 

        ItemNameType10[0] = "Bình máu 1";
        ItemNameType10[1] = "Bình máu 2";
        ItemNameType10[2] = "Bình máu 3";
        ItemNameType10[3] = "Bình máu 4";
        ItemNameType10[4] = "Bình máu 5";
        ItemNameType10[5] = "Bình máu 6";
        ItemNameType10[6] = "Bình máu 7";
        ItemNameType10[7] = "Bình máu 8";
        ItemNameType10[8] = "Bình máu 9";

        ItemNameType10[9] = "Đá cường hóa cấp 1";
        ItemNameType10[10] = "Đá cường hóa cấp 2";
        ItemNameType10[11] = "Đá cường hóa cấp 3";
        ItemNameType10[12] = "Đá cường hóa cấp 4";
        ItemNameType10[13] = "Đá cường hóa cấp 5";
        ItemNameType10[14] = "Đá cường hóa cấp 6";
        ItemNameType10[15] = "Đá cường hóa cấp 7";
        ItemNameType10[16] = "Đá cường hóa cấp 8";
        ItemNameType10[17] = "Đá cường hóa cấp 9";
        ItemNameType10[18] = "Đá cường hóa cấp 10";

        ItemNameType10[19] = "Đá nâng phẩm cấp 1";
        ItemNameType10[20] = "Đá nâng phẩm cấp 2";
        ItemNameType10[21] = "Đá nâng phẩm cấp 3";
        ItemNameType10[22] = "Đá nâng phẩm cấp 4";
        ItemNameType10[23] = "Đá nâng phẩm cấp 5";
        ItemNameType10[24] = "Đá nâng phẩm cấp 6";

        ItemNameType10[25] = "Lông phượng hoàng";
        ItemNameType10[26] = "Khuôn mẫu";

        #endregion

        #region Thông tin item type 10 

        ItemInforType10[0] = "Sử dụng trong chiến đấu, phục hồi 10% lượng máu đã mất cho toàn đội hình trong 10 giây";
        ItemInforType10[1] = "Sử dụng trong chiến đấu, phục hồi 20% lượng máu đã mất cho toàn đội hình trong 10 giây";
        ItemInforType10[2] = "Sử dụng trong chiến đấu, phục hồi 30% lượng máu đã mất cho toàn đội hình trong 10 giây";
        ItemInforType10[3] = "Sử dụng trong chiến đấu, phục hồi 10% tổng lượng máu cho toàn đội hình trong 5 giây";
        ItemInforType10[4] = "Sử dụng trong chiến đấu, phục hồi 20% tổng lượng máu cho toàn đội hình trong 5 giây";
        ItemInforType10[5] = "Sử dụng trong chiến đấu, phục hồi 30% tổng lượng máu cho toàn đội hình trong 5 giây";
        ItemInforType10[6] = "Sử dụng trong chiến đấu, phục hồi 30% tổng lượng máu và tăng 10% hút máu thích ứng cho toàn đội hình trong 10 giây";
        ItemInforType10[7] = "Sử dụng trong chiến đấu, phục hồi 30% tổng lượng máu và tăng 10% giáp, kháng phép cho toàn đội hình trong 10 giây";
        ItemInforType10[8] = "Sử dụng trong chiến đấu, phục hồi 30% tổng lượng máu và tăng 10% sát thương cho toàn đội hình trong 10 giây";

        ItemInforType10[9] = "Cường hóa trang bị lên cấp độ 1\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInforType10[10] = "Cường hóa trang bị lên cấp độ 2\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInforType10[11] = "Cường hóa trang bị lên cấp độ 3\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInforType10[12] = "Cường hóa trang bị lên cấp độ 4\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInforType10[13] = "Cường hóa trang bị lên cấp độ 5\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInforType10[14] = "Cường hóa trang bị lên cấp độ 6\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInforType10[15] = "Cường hóa trang bị lên cấp độ 7\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInforType10[16] = "Cường hóa trang bị lên cấp độ 8\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInforType10[17] = "Cường hóa trang bị lên cấp độ 9\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInforType10[18] = "Cường hóa trang bị lên cấp độ 10\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";

        ItemInforType10[19] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 1\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";
        ItemInforType10[20] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 2\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";
        ItemInforType10[21] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 3\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";
        ItemInforType10[22] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 4\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";
        ItemInforType10[23] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 5\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";
        ItemInforType10[24] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 6\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";

        ItemInforType10[25] = "Dùng để quay vòng quay may mắn, với vòng quay may mắn, bạn có thể nhận được bất kỳ đồ vật nào, từ giá trị thấp tới cực kỳ quý hiếm. Vật phẩm này có thể tìm thấy ở mọi khu vực";
        ItemInforType10[26] = "Dùng để tạo lỗ khảm ngọc vào trang bị";

        #endregion

        #region Tên item type 11 

        ItemNameType11[0] = "Lá ngón";
        ItemNameType11[1] = "Hoa cúc trắng";
        ItemNameType11[2] = "Cỏ gai";
        ItemNameType11[3] = "Quả mã tiền";
        ItemNameType11[4] = "Hoa súng";
        ItemNameType11[5] = "Nhựa cây";
        ItemNameType11[6] = "Hoa cúc xanh";
        ItemNameType11[7] = "Quả cherry";
        ItemNameType11[8] = "Hoa trúc đào";
        ItemNameType11[9] = "Gai xương rồng";
        ItemNameType11[10] = "Lá gai";
        ItemNameType11[11] = "Ớt đỏ";
        ItemNameType11[12] = "Lá con khỉ";
        ItemNameType11[13] = "Cỏ dại";

        ItemNameType11[14] = "Vải thô";
        ItemNameType11[15] = "Vải len";
        ItemNameType11[16] = "Vải lanh";
        ItemNameType11[17] = "Vải jeans";
        ItemNameType11[18] = "Vải cotton";
        ItemNameType11[19] = "Vải lụa";

        ItemNameType11[20] = "Quặng kim loại";
        ItemNameType11[21] = "Quặng đá";
        ItemNameType11[22] = "Quặng crom";
        ItemNameType11[23] = "Quặng đồng";
        ItemNameType11[24] = "Quặng vàng";
        ItemNameType11[25] = "Quặng hồng ngọc";
        ItemNameType11[26] = "Quặng ngọc lục bảo";
        ItemNameType11[27] = "Quặng thạch anh";
        ItemNameType11[28] = "Quặng đá mắt hổ";
        ItemNameType11[29] = "Quặng sapphire";
        ItemNameType11[30] = "Quặng kim cương";

        ItemNameType11[31] = "Nhánh cây";
        ItemNameType11[32] = "Tấm gỗ nhỏ";
        ItemNameType11[33] = "Tấm gỗ lớn";
        ItemNameType11[34] = "Gỗ thông";
        ItemNameType11[35] = "Gỗ trầm hương";
        ItemNameType11[36] = "Gỗ xoan";
        ItemNameType11[37] = "Gỗ lim";
        ItemNameType11[38] = "Cuốn thư cổ";
        ItemNameType11[39] = "Cuốn sách cổ";

        ItemNameType11[40] = "Pha lê lốc xoáy";
        ItemNameType11[41] = "Pha lê rực lửa";
        ItemNameType11[42] = "Pha lê thủy triều";
        ItemNameType11[43] = "Pha lê địa đàng";
        ItemNameType11[44] = "Lông vũ";

        #endregion

        #region Thông tin item type 11 

        ItemInforType11[0] = "Nguyên liệu chế tạo. Là loại lá cực kỳ quý hiếm có thể cứu sống được mạng người, dùng để chế tạo các vị thuốc đặc biệt";
        ItemInforType11[1] = "Nguyên liệu chế tạo";
        ItemInforType11[2] = "Nguyên liệu chế tạo";
        ItemInforType11[3] = "Nguyên liệu chế tạo";
        ItemInforType11[4] = "Nguyên liệu chế tạo";
        ItemInforType11[5] = "Nguyên liệu chế tạo";
        ItemInforType11[6] = "Nguyên liệu chế tạo";
        ItemInforType11[7] = "Nguyên liệu chế tạo";
        ItemInforType11[8] = "Nguyên liệu chế tạo";
        ItemInforType11[9] = "Nguyên liệu chế tạo";
        ItemInforType11[10] = "Nguyên liệu chế tạo";
        ItemInforType11[11] = "Nguyên liệu chế tạo";
        ItemInforType11[12] = "Nguyên liệu chế tạo";
        ItemInforType11[13] = "Nguyên liệu chế tạo";

        ItemInforType11[14] = "Nguyên liệu chế tạo";
        ItemInforType11[15] = "Nguyên liệu chế tạo";
        ItemInforType11[16] = "Nguyên liệu chế tạo";
        ItemInforType11[17] = "Nguyên liệu chế tạo";
        ItemInforType11[18] = "Nguyên liệu chế tạo";
        ItemInforType11[19] = "Nguyên liệu chế tạo";

        ItemInforType11[20] = "Nguyên liệu chế tạo";
        ItemInforType11[21] = "Nguyên liệu chế tạo";
        ItemInforType11[22] = "Nguyên liệu chế tạo";
        ItemInforType11[23] = "Nguyên liệu chế tạo";
        ItemInforType11[24] = "Nguyên liệu chế tạo";
        ItemInforType11[25] = "Nguyên liệu chế tạo";
        ItemInforType11[26] = "Nguyên liệu chế tạo";
        ItemInforType11[27] = "Nguyên liệu chế tạo";
        ItemInforType11[28] = "Nguyên liệu chế tạo";
        ItemInforType11[29] = "Nguyên liệu chế tạo";
        ItemInforType11[30] = "Nguyên liệu chế tạo";

        ItemInforType11[31] = "Nguyên liệu chế tạo";
        ItemInforType11[32] = "Nguyên liệu chế tạo";
        ItemInforType11[33] = "Nguyên liệu chế tạo";
        ItemInforType11[34] = "Nguyên liệu chế tạo";
        ItemInforType11[35] = "Nguyên liệu chế tạo";
        ItemInforType11[36] = "Nguyên liệu chế tạo";
        ItemInforType11[37] = "Nguyên liệu chế tạo";
        ItemInforType11[38] = "Nguyên liệu chế tạo, chứa các bí kíp để tạo nên các loại vũ khí và trang phục cơ bản";
        ItemInforType11[39] = "Nguyên liệu chế tạo, chứa các bí kíp để tạo nên các loại vũ khí và trang phục cao cấp";

        ItemInforType11[40] = "Nguyên liệu chế tạo";
        ItemInforType11[41] = "Nguyên liệu chế tạo";
        ItemInforType11[42] = "Nguyên liệu chế tạo";
        ItemInforType11[43] = "Nguyên liệu chế tạo";
        ItemInforType11[44] = "Nguyên liệu chế tạo";

        #endregion

        #region Tên item type 12

        ItemNameType12[0] = "Ngọc sát thương vật lý";
        ItemNameType12[1] = "Ngọc xuyên kháng phép";
        ItemNameType12[2] = "Ngọc xuyên giáp";
        ItemNameType12[3] = "Ngọc phản sát thương";
        ItemNameType12[4] = "Ngọc gia tăng năng lượng";
        ItemNameType12[5] = "Ngọc hồi phục năng lượng";
        ItemNameType12[6] = "Ngọc năng lượng";
        ItemNameType12[7] = "Ngọc hút máu phép";
        ItemNameType12[8] = "Ngọc gia tăng phép thuật";
        ItemNameType12[9] = "Ngọc phép thuật";
        ItemNameType12[10] = "Ngọc gia tăng kháng phép";
        ItemNameType12[11] = "Ngọc kháng phép";
        ItemNameType12[12] = "Ngọc kháng hiệu ứng";
        ItemNameType12[13] = "Ngọc máu hồi phục";
        ItemNameType12[14] = "Ngọc gia tăng máu";
        ItemNameType12[15] = "Ngọc máu";
        ItemNameType12[16] = "Ngọc tăng lượng vàng rơi ra";
        ItemNameType12[17] = "Ngọc gia tăng giáp";
        ItemNameType12[18] = "Ngọc giáp";
        ItemNameType12[19] = "Ngọc giảm thời gian hồi chiêu";
        ItemNameType12[20] = "Ngọc kháng hệ nước";
        ItemNameType12[21] = "Ngọc phòng thủ hoàn hảo";
        ItemNameType12[22] = "Ngọc kháng hệ lửa";
        ItemNameType12[23] = "Ngọc kháng hệ đất";
        ItemNameType12[24] = "Ngọc tỷ lệ x3 đòn đánh";
        ItemNameType12[25] = "Ngọc tỷ lệ x2 đòn đánh";
        ItemNameType12[26] = "Ngọc sát thương hệ nước";
        ItemNameType12[27] = "Ngọc tốc độ đánh";
        ItemNameType12[28] = "Ngọc hút máu";
        ItemNameType12[29] = "Ngọc sát thương hoàn hảo";
        ItemNameType12[30] = "Ngọc sát thương hệ lửa";
        ItemNameType12[31] = "Ngọc sát thương hệ đất";
        ItemNameType12[32] = "Ngọc chí mạng";
        ItemNameType12[33] = "Ngọc gia tăng sát thương vật lý";

        #endregion

        #region Thông tin item type 12

        ItemInforType12[0] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[1] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[2] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[3] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[4] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[5] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[6] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[7] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[8] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[9] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[10] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[11] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[12] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[13] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[14] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[15] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[16] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[17] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[18] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[19] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[20] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[21] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[22] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[23] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[24] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[25] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[26] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[27] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[28] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[29] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[30] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[31] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[32] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";
        ItemInforType12[33] = "Chỉ số ngẫu nhiên khi khảm vào trang bị";

        #endregion

        #region Danh sách tên item 

        //Item trang bị
        ItemName[1] = "Dao ngắn 1";
        ItemName[2] = "Dao ngắn 2";
        ItemName[3] = "Dao ngắn 3";
        ItemName[4] = "Dao ngắn 4";
        ItemName[5] = "Dao ngắn 5";
        ItemName[6] = "Dao ngắn 6";
        ItemName[7] = "Dao ngắn 7";
        ItemName[8] = "Kiếm 1";
        ItemName[9] = "Kiếm 2";
        ItemName[10] = "Kiếm 3";
        ItemName[11] = "Kiếm 4";
        ItemName[12] = "Kiếm 5";
        ItemName[13] = "Kiếm 6";
        ItemName[14] = "Kiếm 7";
        ItemName[15] = "Kiếm 8";

        ItemName[20] = "Rìu 1";
        ItemName[21] = "Rìu 2";
        ItemName[22] = "Rìu 3";
        ItemName[23] = "Rìu 4";
        ItemName[24] = "Rìu 5";
        ItemName[25] = "Rìu 6";
        ItemName[26] = "Rìu 7";
        ItemName[27] = "Rìu 8";
        ItemName[28] = "Rìu 9";
        ItemName[29] = "Rìu 10";
        ItemName[30] = "Rìu 11";
        ItemName[31] = "Rìu 12";

        ItemName[40] = "Búa 1";
        ItemName[41] = "Búa 2";
        ItemName[42] = "Búa 3";
        ItemName[43] = "Búa 4";
        ItemName[44] = "Búa 5";
        ItemName[45] = "Búa 6";
        ItemName[46] = "Búa 7";
        ItemName[47] = "Búa 8";
        ItemName[48] = "Búa 9";
        ItemName[49] = "Búa 10";
        ItemName[50] = "Búa 11";
        ItemName[51] = "Búa 12";

        ItemName[60] = "Trượng 1";
        ItemName[61] = "Trượng 2";
        ItemName[62] = "Trượng 3";
        ItemName[63] = "Trượng 4";
        ItemName[64] = "Trượng 5";
        ItemName[65] = "Trượng 6";
        ItemName[66] = "Trượng 7";
        ItemName[67] = "Trượng 8";

        ItemName[80] = "Chùy 1";
        ItemName[81] = "Chùy 2";
        ItemName[82] = "Chùy 3";
        ItemName[83] = "Chùy 4";
        ItemName[84] = "Chùy 5";
        ItemName[85] = "Chùy 6";
        ItemName[86] = "Chùy 7";
        ItemName[87] = "Chùy 8";
        ItemName[88] = "Chùy 9";
        ItemName[89] = "Chùy 10";
        ItemName[90] = "Chùy 11";

        ItemName[100] = "Cung 1";
        ItemName[101] = "Cung 2";
        ItemName[102] = "Cung 3";
        ItemName[103] = "Cung 4";
        ItemName[104] = "Cung 5";
        ItemName[105] = "Nỏ 1";
        ItemName[106] = "Nỏ 2";
        ItemName[107] = "Nỏ 3";
        ItemName[108] = "Nỏ 4";

        ItemName[120] = "Súng 1";
        ItemName[121] = "Súng 2";
        ItemName[122] = "Súng 3";
        ItemName[123] = "Súng 4";
        ItemName[124] = "Súng 5";
        ItemName[125] = "Súng 6";
        ItemName[126] = "Súng 7";
        ItemName[127] = "Súng 8";

        ItemName[140] = "Giáp 1";
        ItemName[141] = "Giáp 2";
        ItemName[142] = "Giáp 3";
        ItemName[143] = "Giáp 4";
        ItemName[144] = "Giáp 5";
        ItemName[145] = "Giáp 6";
        ItemName[146] = "Giáp 7";
        ItemName[147] = "Giáp 8";
        ItemName[148] = "Giáp 9";
        ItemName[149] = "Giáp 10";

        ItemName[150] = "Áo choàng 1";
        ItemName[151] = "Áo choàng 2";
        ItemName[152] = "Áo choàng 3";
        ItemName[153] = "Áo choàng 4";
        ItemName[154] = "Áo choàng 5";
        ItemName[155] = "Áo choàng 6";

        ItemName[160] = "Đai lưng 1";
        ItemName[161] = "Đai lưng 2";
        ItemName[162] = "Đai lưng 3";
        ItemName[163] = "Đai lưng 4";
        ItemName[164] = "Đai lưng 5";
        ItemName[165] = "Đai lưng 6";

        ItemName[170] = "Giáp tay 1";
        ItemName[171] = "Giáp tay 2";
        ItemName[172] = "Giáp tay 3";
        ItemName[173] = "Giáp tay 4";
        ItemName[174] = "Giáp tay 5";
        ItemName[175] = "Giáp tay 6";
        ItemName[176] = "Giáp tay 7";
        ItemName[177] = "Giáp tay 8";
        ItemName[178] = "Giáp tay 9";
        ItemName[179] = "Giáp tay 10";

        ItemName[180] = "Găng tay 1";
        ItemName[181] = "Găng tay 2";
        ItemName[182] = "Găng tay 3";
        ItemName[183] = "Găng tay 4";
        ItemName[184] = "Găng tay 5";

        ItemName[185] = "Nhẫn 1";
        ItemName[186] = "Nhẫn 2";
        ItemName[187] = "Nhẫn 3";
        ItemName[188] = "Nhẫn 4";
        ItemName[189] = "Nhẫn 5";

        ItemName[190] = "Khiên 1";
        ItemName[191] = "Khiên 2";
        ItemName[192] = "Khiên 3";
        ItemName[193] = "Khiên 4";
        ItemName[194] = "Khiên 5";

        ItemName[200] = "Bình máu 1";
        ItemName[201] = "Bình máu 2";
        ItemName[202] = "Bình máu 3";
        ItemName[203] = "Bình máu 4";
        ItemName[204] = "Bình máu 5";
        ItemName[205] = "Bình máu 6";
        ItemName[206] = "Bình máu 7";
        ItemName[207] = "Bình máu 8";
        ItemName[208] = "Bình máu 9";

        ItemName[210] = "Đá cường hóa cấp 1";
        ItemName[211] = "Đá cường hóa cấp 2";
        ItemName[212] = "Đá cường hóa cấp 3";
        ItemName[213] = "Đá cường hóa cấp 4";
        ItemName[214] = "Đá cường hóa cấp 5";
        ItemName[215] = "Đá cường hóa cấp 6";
        ItemName[216] = "Đá cường hóa cấp 7";
        ItemName[217] = "Đá cường hóa cấp 8";
        ItemName[218] = "Đá cường hóa cấp 9";
        ItemName[219] = "Đá cường hóa cấp 10";

        ItemName[220] = "Đá nâng phẩm cấp 1";
        ItemName[221] = "Đá nâng phẩm cấp 2";
        ItemName[222] = "Đá nâng phẩm cấp 3";
        ItemName[223] = "Đá nâng phẩm cấp 4";
        ItemName[224] = "Đá nâng phẩm cấp 5";
        ItemName[225] = "Đá nâng phẩm cấp 6";

        ItemName[230] = "Lông phượng hoàng";

        ///Item nhiệm vụ
        ItemName[300] = "Lá ngón";
        ItemName[301] = "Hoa cúc trắng";
        ItemName[302] = "Cỏ gai";
        ItemName[303] = "Quả mã tiền";
        ItemName[304] = "Hoa súng";
        ItemName[305] = "Nhựa cây";
        ItemName[306] = "Hoa cúc xanh";
        ItemName[307] = "Quả cherry";
        ItemName[308] = "Hoa trúc đào";
        ItemName[309] = "Gai xương rồng";
        ItemName[310] = "Lá gai";
        ItemName[311] = "Ớt đỏ";
        ItemName[312] = "Lá con khỉ";
        ItemName[313] = "Cỏ dại";

        ItemName[320] = "Vải thô";
        ItemName[321] = "Vải len";
        ItemName[322] = "Vải lanh";
        ItemName[323] = "Vải jeans";
        ItemName[324] = "Vải cotton";
        ItemName[325] = "Vải lụa";

        ItemName[330] = "Quặng kim loại";
        ItemName[331] = "Quặng đá";
        ItemName[332] = "Quặng crom";
        ItemName[333] = "Quặng đồng";
        ItemName[334] = "Quặng vàng";
        ItemName[335] = "Quặng hồng ngọc";
        ItemName[336] = "Quặng ngọc lục bảo";
        ItemName[337] = "Quặng thạch anh";
        ItemName[338] = "Quặng đá mắt hổ";
        ItemName[339] = "Quặng sapphire";
        ItemName[340] = "Quặng kim cương";

        ItemName[341] = "Nhánh cây";
        ItemName[342] = "Tấm gỗ nhỏ";
        ItemName[343] = "Tấm gỗ lớn";
        ItemName[344] = "Gỗ thông";
        ItemName[345] = "Gỗ trầm hương";
        ItemName[346] = "Gỗ xoan";
        ItemName[347] = "Gỗ lim";
        ItemName[348] = "Cuốn thư cổ";
        ItemName[349] = "Cuốn sách cổ";

        ItemName[350] = "Pha lê lốc xoáy";
        ItemName[351] = "Pha lê rực lửa";
        ItemName[352] = "Pha lê thủy triều";
        ItemName[353] = "Pha lê địa đàng";
        ItemName[354] = "Lông vũ";

        ItemName[500] = "Sách phù thủy Himear";
        ItemName[501] = "Áo choàng Himear";
        ItemName[502] = "Đai lưng Himear";
        ItemName[503] = "Găng tay Himear";
        ItemName[504] = "Giáp trụ Himear";
        ItemName[505] = "Giày Himear";
        #endregion

        #region Thông tin item 

        ItemInfor[200] = "Sử dụng trong chiến đấu, phục hồi 10% lượng máu đã mất cho toàn đội hình trong 10 giây";
        ItemInfor[201] = "Sử dụng trong chiến đấu, phục hồi 20% lượng máu đã mất cho toàn đội hình trong 10 giây";
        ItemInfor[202] = "Sử dụng trong chiến đấu, phục hồi 30% lượng máu đã mất cho toàn đội hình trong 10 giây";
        ItemInfor[203] = "Sử dụng trong chiến đấu, phục hồi 10% tổng lượng máu cho toàn đội hình trong 5 giây";
        ItemInfor[204] = "Sử dụng trong chiến đấu, phục hồi 20% tổng lượng máu cho toàn đội hình trong 5 giây";
        ItemInfor[205] = "Sử dụng trong chiến đấu, phục hồi 30% tổng lượng máu cho toàn đội hình trong 5 giây";
        ItemInfor[206] = "Sử dụng trong chiến đấu, phục hồi 30% tổng lượng máu và tăng 10% hút máu thích ứng cho toàn đội hình trong 10 giây";
        ItemInfor[207] = "Sử dụng trong chiến đấu, phục hồi 30% tổng lượng máu và tăng 10% giáp, kháng phép cho toàn đội hình trong 10 giây";
        ItemInfor[208] = "Sử dụng trong chiến đấu, phục hồi 30% tổng lượng máu và tăng 10% sát thương cho toàn đội hình trong 10 giây";

        ItemInfor[210] = "Cường hóa trang bị lên cấp độ 1\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInfor[211] = "Cường hóa trang bị lên cấp độ 2\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInfor[212] = "Cường hóa trang bị lên cấp độ 3\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInfor[213] = "Cường hóa trang bị lên cấp độ 4\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInfor[214] = "Cường hóa trang bị lên cấp độ 5\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInfor[215] = "Cường hóa trang bị lên cấp độ 6\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInfor[216] = "Cường hóa trang bị lên cấp độ 7\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInfor[217] = "Cường hóa trang bị lên cấp độ 8\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInfor[218] = "Cường hóa trang bị lên cấp độ 9\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";
        ItemInfor[219] = "Cường hóa trang bị lên cấp độ 10\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% chỉ số cơ bản";

        ItemInfor[220] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 1\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";
        ItemInfor[221] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 2\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";
        ItemInfor[222] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 3\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";
        ItemInfor[223] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 4\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";
        ItemInfor[224] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 5\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";
        ItemInfor[225] = "Sử dụng đá này để nâng phẩm cho trang bị lên cấp độ 6\nSau mỗi cấp, sức mạnh của trang bị tăng thêm 10% của (chỉ số cơ bản + chỉ số cấp độ)";

        ItemInfor[230] = "Dùng để quay vòng quay may mắn, với vòng quay may mắn, bạn có thể nhận được bất kỳ đồ vật nào, từ giá trị thấp tới cực kỳ quý hiếm. Vật phẩm này có thể tìm thấy ở mọi khu vực";

        //ItemInfor nhiệm vụ
        ItemInfor[300] = "Nguyên liệu chế tạo. Là loại lá cực kỳ quý hiếm có thể cứu sống được mạng người, dùng để chế tạo các vị thuốc đặc biệt, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[301] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[302] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[303] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[304] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[305] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[306] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[307] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[308] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[309] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[310] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[311] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[312] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[313] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];

        ItemInfor[320] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[321] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[322] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[323] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[324] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[325] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];

        ItemInfor[330] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];
        ItemInfor[331] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];
        ItemInfor[332] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];
        ItemInfor[333] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];
        ItemInfor[334] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];
        ItemInfor[335] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];
        ItemInfor[336] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];
        ItemInfor[337] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];
        ItemInfor[338] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];
        ItemInfor[339] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];
        ItemInfor[340] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];

        ItemInfor[341] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[342] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[343] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[344] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[345] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[346] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[347] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[168] + " hoặc " + lang[169];
        ItemInfor[348] = "Nguyên liệu chế tạo, chứa các bí kíp để tạo nên các loại vũ khí và trang phục cơ bản, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];
        ItemInfor[349] = "Nguyên liệu chế tạo, chứa các bí kíp để tạo nên các loại vũ khí và trang phục cao cấp, có thể tìm thấy tại " + lang[173] + " hoặc " + lang[174];

        ItemInfor[350] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[170] + " hoặc " + lang[172];
        ItemInfor[351] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[170] + " hoặc " + lang[172];
        ItemInfor[352] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[170] + " hoặc " + lang[172];
        ItemInfor[353] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[170] + " hoặc " + lang[172];
        ItemInfor[354] = "Nguyên liệu chế tạo, có thể tìm thấy tại " + lang[170] + " hoặc " + lang[172];

        ItemInfor[500] = "Thuộc bộ trang bị Himear, nếu mang đủ trang bị, sẽ kích hoạt các dòng sau";
        ItemInfor[501] = "Thuộc bộ trang bị Himear, nếu mang đủ trang bị, sẽ kích hoạt các dòng sau";
        ItemInfor[502] = "Thuộc bộ trang bị Himear, nếu mang đủ trang bị, sẽ kích hoạt các dòng sau";
        ItemInfor[503] = "Thuộc bộ trang bị Himear, nếu mang đủ trang bị, sẽ kích hoạt các dòng sau";
        ItemInfor[504] = "Thuộc bộ trang bị Himear, nếu mang đủ trang bị, sẽ kích hoạt các dòng sau";
        ItemInfor[505] = "Thuộc bộ trang bị Himear, nếu mang đủ trang bị, sẽ kích hoạt các dòng sau";
        ItemInfor[506] = "+5% sức mạnh phép thuật";
        ItemInfor[507] = "+5% hút máu phép";
        ItemInfor[508] = "+5% giáp và kháng phép";

        #endregion

        #region Giới thiệu skill 

        HeroSkillDescription[0] = "Xoay người và tung ra liên tục 4 phi tiêu gây 150% sát thương vật lý mỗi phi tiêu khi trúng mục tiêu gần nhất";
        HeroSkillDescription[1] = "Lướt kiếm trong phạm vi diện rộng có thể gây sát thương toàn bộ đội hình đối thủ với 175% sát thương vật lý";
        HeroSkillDescription[2] = "Tung ra một boomerang gây 135% sát thương vật lý với toàn bộ đối thủ khi va chạm";
        HeroSkillDescription[3] = "Xoay người chém rất mạnh xuống đất gây 200% sát thương vật lý với các mục tiêu trong phạm vi";
        HeroSkillDescription[4] = "Biến hóa cây gậy khổng lồ, đập mạnh xuống đất với sát thương diện rộng, gây 200% sát thương vật lý và làm choáng các mục tiêu trong phạm vi";
        HeroSkillDescription[5] = "Bắn một tia sáng xuyên bản đồ, gây 200% sát thương phép thuật";
        HeroSkillDescription[6] = "Phun ra một dải lửa xuyên các mục tiêu gây liên tục 20% sát thương phép thuật và thiêu đốt mục tiêu trong 5 giây";
        HeroSkillDescription[7] = "Bắn 1 quả tên lửa gây 250% sát thương vật lý với mục tiêu đầu tiên trúng phải";
        HeroSkillDescription[8] = "Trong 3 giây, hồi máu cho đồng đội trong phạm vi diện rộng bằng với 300% sức mạnh phép thuật";
        HeroSkillDescription[9] = "Bản thân miễn nhiễm sát thương, tạo ra một khiên chắn, chặn các sát thương từ xa của đối phương";

        #endregion

        #region Giới thiệu nội tại 

        HeroIntrinsic[0] = "Mỗi đòn đánh thứ 3 không cách nhau quá 5 giây giữa 2 đón đánh sẽ gây thêm 1 lần sát thương và bay xuyên đối thủ";
        HeroIntrinsic[1] = "Dưới 50% máu, tốc độ đánh tăng 1.5 lần, dưới 30% máu, tốc độ đánh tăng 2 lần, dưới 10% máu, tốc độ đánh tăng 3 lần";
        HeroIntrinsic[2] = "Mỗi đòn đánh thứ 3 cách nhau không quá 5 giây giữa 2 đòn đánh sẽ bắn thêm 2 phi tiêu nữa";
        HeroIntrinsic[3] = "Đòn đánh thường lên mục tiêu sẽ gây thêm sát thương vật lý bằng 10% lượng máu đối phương, hiệu ứng này sẽ không xuất hiện trong 5 giây";
        HeroIntrinsic[4] = "Khi hỗ trợ hoặc hạ gục đối phương, hồi lại 20% máu đã tổn thất";
        HeroIntrinsic[5] = "Mỗi đòn đánh thường có xác suất 10% làm chậm đối phương trúng đòn";
        HeroIntrinsic[6] = "Đòn sát thương cuối cùng của kỹ năng sẽ gây hiệu ứng thiêu đốt cho các mục tiêu";
        HeroIntrinsic[7] = "Khi hỗ trợ hoặc hạ gục đối phương, tốc độ đánh sẽ được tăng gấp đôi trong 5 giây";
        HeroIntrinsic[8] = "Đòn đánh thường va chạm vào đồng đội sẽ hồi phục cho đồng đội lượng máu bằng với 10% sức mạnh phép thuật";
        HeroIntrinsic[9] = "Với mỗi lần chịu sát thương thứ 5, giáp và kháng phép sẽ được tăng gấp đôi";

        #endregion

        #region Hướng dẫn 
        IntroductionTitle[0] = "Hướng dẫn chế tạo vật phẩm";
        IntroductionTitle[1] = "Hướng dẫn bản đồ thế giới";
        #endregion

        #region Nội dung hướng dẫn 
        IntroductionDescriptions[0] = "Tìm đủ nguyên liệu để chế tạo vật phẩm mà bạn muốn";
        IntroductionDescriptions[0] += "\nMỗi nguyên liệu sẽ tìm được ở những vùng đất khác nhau";
        IntroductionDescriptions[0] += "\nVật phẩm trang bị khi chế tạo, sẽ cho ra các chỉ số và các dòng ngẫu nhiên, vật phẩm yêu cầu nguyên liệu chế tạo càng nhiều, sẽ cho ra chỉ số càng cao";
        IntroductionDescriptions[0] += "\nTỉ lệ chế tạo các vật phẩm là 100%";
        IntroductionDescriptions[0] += "\nCác vật phẩm sau khi chế tạo mà không thay đổi chỉ số đều có thể đem bán ở chợ đen";

        IntroductionDescriptions[1] = "Mỗi vùng đất mà bạn lựa chọn khám phá, sẽ có những đối thủ khác nhau tùy theo độ khó";
        IntroductionDescriptions[1] += "\nVật phẩm rơi ra ở những vùng đất cũng có thể khác nhau";
        IntroductionDescriptions[1] += "\nSau mỗi trận đấu bạn chiến thắng hoặc bỏ cuộc giữa chừng, kẻ thù tại vùng đất đó sẽ mạnh mẽ hơn một chút, vì chúng rút kinh nghiệm đấy";
        IntroductionDescriptions[1] += "\nNếu kẻ thù tại vùng đất bạn khám phá quá mạnh, bạn có thể thuê một đội quân sát thủ để làm giảm sức mạnh của kẻ thù xuống thông qua chức năng 'Thuê sát thủ'";
        IntroductionDescriptions[1] += "\nBạn sẽ có cơ hội thu phục những kẻ thù khác nếu như danh sách tướng của bạn chưa có kẻ đó, nhưng hãy cẩn thận, chúng mạnh lắm đấy";
        IntroductionDescriptions[1] += "\nCó 2 chế độ khám phá vùng đất, ngẫu nhiên và xem trước";
        IntroductionDescriptions[1] += "\nVới chế độ xem trước bạn có thể biết được mình sẽ chạm trán với đối thủ nào";
        IntroductionDescriptions[1] += "\nVới chế độ ngẫu nhiên, bạn sẽ không biết được mình sẽ chạm trán với những gì, phần thưởng nhận được trong chế độ này sẽ cao hơn";

        #endregion
    }
    // Update is called once per frame
    private static void Language_EN () {
        //0-99: information game
        //100-199: player name near
        //200-299: player near descript
        //300-399: player name magic
        //400-499: player magic descript
        //500-599: player name far
        //600-699: player far descript

        #region Danh sách tên item 

        //Item trang bị
        ItemName[1] = "Knife 1";
        ItemName[2] = "Knife 2";
        ItemName[3] = "Knife 3";
        ItemName[4] = "Knife 4";
        ItemName[5] = "Knife 5";
        ItemName[6] = "Knife 6";
        ItemName[7] = "Knife 7";
        ItemName[8] = "Sword 1";
        ItemName[9] = "Sword 2";
        ItemName[10] = "Sword 3";
        ItemName[11] = "Sword 4";
        ItemName[12] = "Sword 5";
        ItemName[13] = "Sword 6";
        ItemName[14] = "Sword 7";
        ItemName[15] = "Sword 8";

        ItemName[20] = "Axe 1";
        ItemName[21] = "Axe 2";
        ItemName[22] = "Axe 3";
        ItemName[23] = "Axe 4";
        ItemName[24] = "Axe 5";
        ItemName[25] = "Axe 6";
        ItemName[26] = "Axe 7";
        ItemName[27] = "Axe 8";
        ItemName[28] = "Axe 9";
        ItemName[29] = "Axe 10";
        ItemName[30] = "Axe 11";
        ItemName[31] = "Axe 12";

        ItemName[40] = "Hammer 1";
        ItemName[41] = "Hammer 2";
        ItemName[42] = "Hammer 3";
        ItemName[43] = "Hammer 4";
        ItemName[44] = "Hammer 5";
        ItemName[45] = "Hammer 6";
        ItemName[46] = "Hammer 7";
        ItemName[47] = "Hammer 8";
        ItemName[48] = "Hammer 9";
        ItemName[49] = "Hammer 10";
        ItemName[50] = "Hammer 11";
        ItemName[51] = "Hammer 12";

        ItemName[60] = "Trượng 1";
        ItemName[61] = "Trượng 2";
        ItemName[62] = "Trượng 3";
        ItemName[63] = "Trượng 4";
        ItemName[64] = "Trượng 5";
        ItemName[65] = "Trượng 6";
        ItemName[66] = "Trượng 7";
        ItemName[67] = "Trượng 8";

        ItemName[80] = "Chùy 1";
        ItemName[81] = "Chùy 2";
        ItemName[82] = "Chùy 3";
        ItemName[83] = "Chùy 4";
        ItemName[84] = "Chùy 5";
        ItemName[85] = "Chùy 6";
        ItemName[86] = "Chùy 7";
        ItemName[87] = "Chùy 8";
        ItemName[88] = "Chùy 9";
        ItemName[89] = "Chùy 10";
        ItemName[90] = "Chùy 11";

        ItemName[100] = "Cung 1";
        ItemName[101] = "Cung 2";
        ItemName[102] = "Cung 3";
        ItemName[103] = "Cung 4";
        ItemName[104] = "Cung 5";
        ItemName[105] = "Nỏ 1";
        ItemName[106] = "Nỏ 2";
        ItemName[107] = "Nỏ 3";
        ItemName[108] = "Nỏ 4";

        ItemName[120] = "Súng 1";
        ItemName[121] = "Súng 2";
        ItemName[122] = "Súng 3";
        ItemName[123] = "Súng 4";
        ItemName[124] = "Súng 5";
        ItemName[125] = "Súng 6";
        ItemName[126] = "Súng 7";
        ItemName[127] = "Súng 8";

        ItemName[140] = "Giáp 1";
        ItemName[141] = "Giáp 2";
        ItemName[142] = "Giáp 3";
        ItemName[143] = "Giáp 4";
        ItemName[144] = "Giáp 5";
        ItemName[145] = "Giáp 6";
        ItemName[146] = "Giáp 7";
        ItemName[147] = "Giáp 8";
        ItemName[148] = "Giáp 9";
        ItemName[149] = "Giáp 10";

        ItemName[150] = "Áo choàng 1";
        ItemName[151] = "Áo choàng 2";
        ItemName[152] = "Áo choàng 3";
        ItemName[153] = "Áo choàng 4";
        ItemName[154] = "Áo choàng 5";
        ItemName[155] = "Áo choàng 6";

        ItemName[160] = "Đai lưng 1";
        ItemName[161] = "Đai lưng 2";
        ItemName[162] = "Đai lưng 3";
        ItemName[163] = "Đai lưng 4";
        ItemName[164] = "Đai lưng 5";
        ItemName[165] = "Đai lưng 6";

        ItemName[170] = "Giáp tay 1";
        ItemName[171] = "Giáp tay 2";
        ItemName[172] = "Giáp tay 3";
        ItemName[173] = "Giáp tay 4";
        ItemName[174] = "Giáp tay 5";
        ItemName[175] = "Giáp tay 6";
        ItemName[176] = "Giáp tay 7";
        ItemName[177] = "Giáp tay 8";
        ItemName[178] = "Giáp tay 9";
        ItemName[179] = "Giáp tay 10";

        ItemName[180] = "Găng tay 1";
        ItemName[181] = "Găng tay 2";
        ItemName[182] = "Găng tay 3";
        ItemName[183] = "Găng tay 4";
        ItemName[184] = "Găng tay 5";

        ItemName[185] = "Nhẫn 1";
        ItemName[186] = "Nhẫn 2";
        ItemName[187] = "Nhẫn 3";
        ItemName[188] = "Nhẫn 4";
        ItemName[189] = "Nhẫn 5";

        ItemName[190] = "Khiên 1";
        ItemName[191] = "Khiên 2";
        ItemName[192] = "Khiên 3";
        ItemName[193] = "Khiên 4";
        ItemName[194] = "Khiên 5";

        ItemName[200] = "Health potion 1";
        ItemName[201] = "Health potion 2";
        ItemName[202] = "Health potion 3";
        ItemName[203] = "Health potion 4";
        ItemName[204] = "Health potion 5";
        ItemName[205] = "Health potion 6";
        ItemName[206] = "Health potion 7";
        ItemName[207] = "Health potion 8";
        ItemName[208] = "Health potion 9";

        ItemName[210] = "Gem upgrade lv 1";
        ItemName[211] = "Gem upgrade lv 2";
        ItemName[212] = "Gem upgrade lv 3";
        ItemName[213] = "Gem upgrade lv 4";
        ItemName[214] = "Gem upgrade lv 5";
        ItemName[215] = "Gem upgrade lv 6";
        ItemName[216] = "Gem upgrade lv 7";
        ItemName[217] = "Gem upgrade lv 8";
        ItemName[218] = "Gem upgrade lv 9";
        ItemName[219] = "Gem upgrade lv 10";

        ItemName[220] = "Gem up quality lv 1";
        ItemName[221] = "Gem up quality lv 2";
        ItemName[222] = "Gem up quality lv 3";
        ItemName[223] = "Gem up quality lv 4";
        ItemName[224] = "Gem up quality lv 5";
        ItemName[225] = "Gem up quality lv 6";

        ItemName[230] = "Phoenix Feather";

        //Item nhiệm vụ
        ItemName[300] = "Gelsemium elegans";
        ItemName[301] = "White chrysanthemum";
        ItemName[302] = "Thorn grass";
        ItemName[303] = "Strychnos nux-vomica";
        ItemName[304] = "Water lily";
        ItemName[305] = "SAP";
        ItemName[306] = "Blue chrysanthemum";
        ItemName[307] = "Cherry";
        ItemName[308] = "Oleander ";
        ItemName[309] = "Cactus spines";
        ItemName[310] = "Hemp leaves";
        ItemName[311] = "Red pepper";
        ItemName[312] = "Monkey leaves";
        ItemName[313] = "Weed";

        ItemName[320] = "Raw cloth";
        ItemName[321] = "Wool cloth";
        ItemName[322] = "Linen cloth";
        ItemName[323] = "Jeans cloth";
        ItemName[324] = "Cotton cloth";
        ItemName[325] = "Silk cloth";

        ItemName[330] = "Metal ores";
        ItemName[331] = "Rock ore";
        ItemName[332] = "Crom ore";
        ItemName[333] = "Copper ore";
        ItemName[334] = "Gold ore";
        ItemName[335] = "Ruby ore";
        ItemName[336] = "Emerald ore";
        ItemName[337] = "Quartz ore";
        ItemName[338] = "Tiger eye ores";
        ItemName[339] = "Sapphire ore";
        ItemName[340] = "Diamond ore";

        ItemName[341] = "Bough";
        ItemName[342] = "Clapboard";
        ItemName[343] = "Large wooden board";
        ItemName[344] = "Pine";
        ItemName[345] = "Agarwood";
        ItemName[346] = "Oval wood";
        ItemName[347] = "Ironwood";
        ItemName[348] = "Ancient letter";
        ItemName[349] = "Ancient book";

        ItemName[350] = "Crystal whirlwind";
        ItemName[351] = "Crystal blazing fire";
        ItemName[352] = "Crystal tide";
        ItemName[353] = "Crystal heaven";
        ItemName[354] = "Feather";

        ItemName[500] = "Himear wizard book";
        ItemName[501] = "Himear cloak";
        ItemName[502] = "Himear belt";
        ItemName[503] = "Himear loves";
        ItemName[504] = "Himear bracelet";
        ItemName[505] = "Himear boot";
        #endregion

        #region Phần ngôn ngữ tổng quan 

        lang[0] = "";
        lang[1] = "";
        lang[2] = "Comming Soon...!";
        lang[11] = "Infor";
        lang[12] = "Equip";
        lang[13] = "Skill";
        lang[14] = "Special";
        lang[15] = "Story";
        lang[16] = "Fighter";
        lang[17] = "Assassin";
        lang[18] = "Support";
        lang[19] = "Tanker";
        lang[20] = "Archer";
        lang[21] = "Magician";
        lang[22] = "Values";
        lang[23] = "Level: ";
        lang[24] = "Health";
        lang[25] = "Mana";
        lang[26] = "Atk Physic";
        lang[27] = "Atk Magic";
        lang[28] = "Armor";
        lang[29] = "Magic Resist";
        lang[30] = "Life steal physic";
        lang[31] = "Life steal magic";
        lang[32] = "Lethality";
        lang[33] = "Magic penetration";
        lang[34] = "Health regen per " + ItemCoreSetting.SecondHeathRegen.ToString () + " second";
        lang[35] = "Mana regen";
        lang[36] = "Critical";
        lang[37] = "+ h.ứng gây ra";
        lang[38] = "Tenacity (%)";
        lang[39] = "Attack speed";
        lang[40] = "%";
        lang[41] = "/s";
        lang[42] = "/lvl)";
        lang[43] = "Cooldown reduction";
        lang[45] = "Skill cooldown";
        lang[46] = "Mana skill";
        lang[47] = "+ ";
        lang[48] = "Remove";

        //Setting --Window setting
        lang[50] = "Settings";
        lang[51] = "Graphics";
        lang[52] = "Very low";
        lang[53] = "Low";
        lang[54] = "Normal";
        lang[55] = "Hight";
        lang[56] = "Audio";
        lang[57] = "Music";
        lang[58] = "Sound";
        lang[59] = "Languages";
        //--------Button
        lang[60] = "OK";
        lang[61] = "Chọn NV";
        lang[62] = "Feedback";
        lang[63] = "Exit game";
        lang[64] = "Title...";
        lang[65] = "Content...";
        //--------Window feedback
        lang[66] = "Send";
        lang[67] = "Cancel";
        lang[68] = "Email...";
        lang[69] = "";
        lang[70] = "Please input title";
        lang[71] = "Please input content";
        lang[72] = "<color=green>Thanks your feedback!</color>";
        lang[73] = "Phản hồi của bạn chưa được gửi đi, kiểm tra lại mạng xem sao";
        //---------Inventory
        lang[74] = "Use: ";
        lang[75] = "Quantity: ";
        lang[76] = "Price: ";
        lang[77] = "Không thao tác được khi đang ở chế độ lựa chọn vật phẩm";
        lang[78] = "Fast Equip";
        lang[79] = "Fast Remove";
        lang[80] = "Remove success";

        //Scene Room
        lang[100] = "Combat";
        lang[101] = "Phát hiện thấy phiên bản {0} trên máy chủ, bạn có muốn tải và cập nhật phiên bản mới ?";
        lang[102] = "Are you want exit game?";
        lang[103] = "<color=green>Applied language, please change scene!</color>";
        lang[104] = "Nhập mã phòng";
        lang[110] = " tạo thành công phòng ";
        lang[111] = "Đã vào phòng ";
        lang[112] = " đã kết nối";
        lang[113] = "Bạn cùng phòng đã thoát";
        lang[114] = "Chờ đồng đội sẵn sàng";
        lang[115] = "Sẵn sàng";
        lang[116] = "Do not action";
        lang[117] = "Can not connected to server";
        lang[118] = "Team";
        lang[119] = "Personal";

        lang[120] = "Can not connect to server...";
        lang[121] = "Please input username";
        lang[122] = "Your name";
        lang[123] = "Your squad needs at least 3 hero";
        lang[124] = "Save battery";
        lang[125] = " - ON";
        lang[126] = " - OFF";
        lang[127] = "Chat box system is loading...!";
        lang[128] = "Can not add equip item, slot is full";
        lang[129] = "Equiped";

        //Scene Inventory
        lang[130] = "Confirm sell {0} item with {1} gold";
        lang[131] = "Confirm sell this item with {0} gold";
        lang[132] = "View";
        lang[133] = "all";
        lang[134] = "Equip";
        lang[135] = "Use";
        lang[136] = "Quest";
        lang[137] = "Select";
        lang[138] = "Sell";
        lang[139] = "Sort";
        lang[140] = "Inventory not enough slots";
        lang[141] = "<color=green>Craft success!</color>";
        lang[142] = "<color=red>Eror - Do not craft item</color>";
        lang[143] = "<color=red>Not enough resources to craft</color>";
        lang[144] = "Bag";
        lang[145] = "Craft";
        lang[146] = "Heroes";
        lang[147] = "Craft all";
        lang[148] = "Required";
        lang[149] = "<color=red>Not enough resources</color>";

        lang[150] = "Earth";
        lang[151] = "Water";
        lang[152] = "Fire";
        lang[153] = "Wind";

        lang[154] = "Upgrade";
        lang[155] = "Up quality";
        lang[156] = "Disassemble";
        lang[157] = "<color=green>Upgrade successfully</color>";
        lang[158] = "<color=red>This item is max level</color>";
        lang[159] = "<color=green>Up quality successfully</color>";
        lang[160] = "<color=red>This item is max quality</color>";
        lang[161] = "Skill button in the right";
        lang[162] = "System settings";
        lang[163] = "Battle settings";
        lang[164] = "Slow motion when use skill";
        lang[165] = "Intrinsic and skill";
        lang[166] = "<color=green>Intrinsic</color>";
        lang[167] = "<color=orange>Skill</color>";
        lang[168] = "<color=#454523>The Forest</color>";
        lang[169] = "<color=#BDAD62>Meadow</color>";
        lang[170] = "<color=#FF5410>Volcano</color>";
        lang[171] = "Snow Mountain";
        lang[172] = "<color=red>Hell</color>";
        lang[173] = "<color=green>Poisonous Cave</color>";
        lang[174] = "<color=#3F42AF>Ghost Cave</color>";
        lang[175] = "<color=red>Inventory slot is limited</color>";
        lang[176] = "Win";
        lang[177] = "Lose";
        lang[178] = "EXP";
        lang[179] = "Pause\nYou want leave battle?";
        lang[180] = "You want watch video to expand inventory?";
        lang[181] = "Wait for next video to load";
        lang[182] = "Watch video for reward";
        lang[183] = "Watch";
        lang[184] = "Let's go";
        lang[185] = "Equip physic";
        lang[186] = "Equip magic";
        lang[187] = "Equip defense";
        lang[188] = "Item use";
        lang[189] = "Equip special";
        lang[190] = "You received <color=#FF5410>{0}</color>";
        lang[191] = "<color=red>Do not enough Phoenix feather</color>";
        lang[192] = "Auto spin";
        lang[193] = "Stop";
        lang[194] = "Introduction";
        lang[195] = "- Required for 1 lucky spin is " + Module.SpinItemQuantity.ToString () + " <color=red>Phoenix Feather</color>";
        lang[195] += "\n\n- Phoenix Feather can be found in every region";
        lang[195] += "\n\n- Item reward can is any item, from low value to extremely rare.";
        lang[195] += "\n\n- Caution: Sort your inventory before spin";
        lang[196] += "Values of item is random when crafting";
        lang[197] = "Enter";
        lang[198] = "Hire Assassins";
        lang[199] = "Hire assassins with {0} gems";

        lang[200] = "Health";
        lang[201] = "Mana";
        lang[202] = "Atk";
        lang[203] = "Magic";
        lang[204] = "Armor";
        lang[205] = "Magic resist";
        lang[206] = "Health regen per " + ItemCoreSetting.SecondHeathRegen.ToString () + " second";
        lang[207] = "Mana regen";
        lang[208] = "Damage earth";
        lang[209] = "Damage water";
        lang[210] = "Damage fire";
        lang[211] = "Earth Defense";
        lang[212] = "Water Defense";
        lang[213] = "Fire Defense";
        lang[214] = "Attack speed";
        lang[215] = "Skill cooldown";
        lang[216] = "Life steal physic";
        lang[217] = "Life steal magic";
        lang[218] = "Lethality";
        lang[219] = "Magic penetration";
        lang[220] = "Critical";
        lang[221] = "Tenacity";
        lang[222] = "Cooldown reduction";
        lang[223] = "Damage excellent";
        lang[224] = "Excellent Defense";
        lang[225] = "Double damage ratio";
        lang[226] = "Triple damage ratio";
        lang[227] = "Reflect Damage";
        lang[228] = "Reward plus";
        lang[229] = "Mana for atk";
        lang[230] = "Mana for skill";
        lang[231] = "Health per level";
        lang[232] = "Atk per level";
        lang[233] = "Magic per level";
        lang[234] = "Armor per level";
        lang[235] = "Magic resist per level";
        lang[236] = "Health regen per level";
        lang[237] = "Mana regen per level";
        lang[238] = "Cooldown reduction per level";
        lang[250] = "You not enough gems";
        lang[251] = "Hire assassins successfully!";
        lang[252] = "Preview";
        lang[253] = "Random";
        lang[254] = "Survival";
        lang[255] = "This mode allows you to know in advance the enemy squad, so you can choose the tactics and equipment that best suit you.";
        lang[256] = "You will not know who you will encounter in this mode, of course, the reward you get will be higher.";
        lang[257] = "Fight until you fall, the leaderboard will always be waiting for you.";
        lang[258] = "Gold: ";
        lang[259] = "Gem: ";
        lang[260] = ". Can find at ";
        lang[261] = "Remove";
        lang[262] = "Information";
        lang[263] = "Equip";
        lang[264] = "Change equip";
        lang[265] = "Select your equip";
        lang[266] = "Can't change the item for yourself";
        lang[267] = "Characters are not yet equipped";
        lang[268] = "Fast change equip success!";
        lang[269] = "Break ball";
        lang[270] = "Hammer: ";
        lang[271] = "Spin";
        lang[272] = "Break ball is a feature that requires a network connection. Every day, you will have 5 hammers to break the ball"; //Hướng dẫn break ball, lấy từ server
        lang[273] = "Can not break this item";
        lang[274] = "Inventory";
        lang[275] = "Reward ";
        lang[276] = " golds";
        lang[277] = " gems";
        lang[278] = "Confirm break this item?";
        lang[279] = "Online Reward";
        lang[280] = "Claim";
        lang[281] = "Team is full";
        lang[282] = "Choose item type to craft";
        lang[283] = "Choose item to craft";
        lang[284] = "Resources need to craft";
        lang[285] = "With items use, you can craft many";
        lang[286] = "Filter class hero";
        lang[287] = "Hero list";
        lang[288] = "Choose functions";
        lang[289] = "Hero detail informations";
        lang[290] = "You can equip for hero in this";
        lang[291] = "Skill and Intrinsic information";
        lang[292] = "Hero's story";
        lang[293] = "Special effects that this hero possesses";
        lang[294] = "Start";
        lang[295] = "Limit jewel socket";
        lang[296] = "Please select item";
        lang[297] = "Insert jewel";
        lang[298] = "Break jewel";
        lang[299] = "Create socket";
        lang[300] = "<color=red>Confirm break this jewel?</color>";
        lang[301] = "Jewel destroyed";
        lang[302] = "Socket";
        lang[303] = "Can not create socket";
        lang[304] = "Do not enough gold";
        lang[305] = "Hell Tower";
        lang[306] = "Confirm leave stage?";
        lang[307] = "You need select a card or choose pass";
        lang[308] = "This card not match";
        lang[309] = "Style Options";
        lang[310] = "Fast select card";
        lang[311] = "Select color";
        lang[312] = "OK";
        lang[313] = "Pass";
        lang[314] = "Exit";
        lang[315] = "Play";
        lang[316] = "Ranking";
        lang[317] = "Place a bet";
        lang[318] = "Players";
        lang[319] = "Options";
        lang[320] = "Background color";
        lang[321] = "Player ";
        lang[322] = "Card game";
        lang[323] = "Fast pass round";
        lang[324] = "Show card available";
        lang[325] = "Card number";
        lang[326] = "These cards are divided equally into 4 colors: red, yellow, green, blue. They are numbered from 0 to 9";
        lang[327] = "Functional card";
        lang[328] = "Skip card: skip round of next player";
        lang[329] = "Reverse card: When this card is throw, it will be reversed. The default when the game is open is clockwise";
        lang[330] = "Card +2: forced next player get 2 card";
        lang[331] = "Change color card: allow you choose a color for next round";
        lang[332] = "Extension cards";
        lang[333] = "These are cards that extend from the basic cards to add some fun to the game";
        lang[334] = "Skip card x2: Ignore 2 consecutive turns, if there are only 2 players in the table, it will only work as a skip card";
        lang[335] = "Tornado card: When playing this card, the next opponent is forced to draw until it has picked a card of the same color as the tornado card, or the color changed card, and lost its turn";
        lang[336] = "Discard card: When playing this card, you are allowed to discharge all cards of the same color";
        lang[337] = "Unexpected attack card: Change the color of round at will, forcing the opponent to get another 4 cards, but it is allowed to choose an opponent to attack.";
        lang[338] = "Random card: Force the opponent to draw randomly from 1-9 cards and lose their turn";
        lang[339] = "At the start of the game, the hand will be played clockwise, the first player to play a card of the same color as the required, The next player needs to play a card that continues with the first player's card. Each player is allowed to play only one card at a time in his or her turn based on the following principle: Players play their cards down using a card of the same number or color of the previous player's card (including Both number and function cards have one color.) Or players can play cards with the function of changing colors.\n\n" +
            "- If the card cannot be dealt then the player must draw a card. If the card is in accordance with the rules of the card, the player may play that card as well. If not, the player will lose a turn.\n" +
            "- Players take turns to play their cards in turn until the player has dealt all the cards in list.\n\n" +
            "A game ends when someone throw all the cards. Then proceed to count the points of the remaining players. The winner will get all these points. These points can be converted into rewards later.\n\n" +
            "The method of calculating the score in the game is as follows:\n" +
            "- Add up the points in the cards to the loser." +
            "- Cards with numbers (0-9) are counted equal to the number on the card." +
            "- Functional cards such as +2, Skip card, Reverse card, etc. are counted 20 points." +
            "- Cards that Change Color, Change Color +4 ... are counted 50 points";
        lang[340] = "Basic";
        lang[341] = "Extension";
        lang[342] = "Introduction";
        lang[343] = "Change color +4: allow you choose a color for next round, forced next player get 4 card and skip their round";
        lang[344] = "Informations";
        lang[345] = "Fast get card";
        lang[346] = "Please input your name";
        lang[347] = "Your name has special character";
        //Vote setup
        lang[348] = "What function you want see in the next version?";
        lang[349] = "Vote";
        lang[350] = "Chatbox";
        lang[351] = "Chatbox allows you to chat with everyone, requires your device to connect to the internet";
        lang[352] = "Add 1 new hero";
        lang[353] = "Add 1 new hero to list";
        lang[354] = "Thanks for your vote";
        //=============================================
        lang[355] = "It seems this is the first time play, do you want a little tutorial?";
        lang[356] = "Buy";
        lang[357] = "Buy successfully";
        lang[358] = "Add 1 inventory slot with {0} gems?";
        lang[359] = "Inventory slot +1";

        //Thông tin chỉ số
        lang[700] = "+{0} Attack physic";
        lang[701] = "+{0} Magic";
        lang[702] = "+{0} Health";
        lang[703] = "+{0} Mana";
        lang[704] = "+{0} Armor";
        lang[705] = "+{0} Magic Resist";
        lang[706] = "<color=darkblue>+{0} Health regen per " + ItemCoreSetting.SecondHeathRegen.ToString () + " second</color>";
        lang[707] = "<color=darkblue>+{0} Mana regen</color>";
        lang[708] = "<color=darkblue>+{0} Damage earth</color>";
        lang[709] = "<color=darkblue>+{0} Damage water</color>";
        lang[710] = "<color=darkblue>+{0} Damage fire</color>";
        lang[711] = "<color=darkblue>+{0} Against eather damage</color>";
        lang[712] = "<color=darkblue>+{0} Against water damage</color>";
        lang[713] = "<color=darkblue>+{0} Against fire damage</color>";
        lang[714] = "<color=darkblue>+{0}% Attack Speed</color>";
        lang[715] = "<color=darkblue>+{0}% Life Steal Physic</color>";
        lang[716] = "<color=darkblue>+{0}% Life Steal Magic</color>";
        lang[717] = "<color=darkblue>+{0}% Lethality</color>";
        lang[718] = "<color=darkblue>+{0}% MagicPenetration</color>";
        lang[719] = "<color=darkblue>+{0}% Critical</color>";
        lang[720] = "<color=darkblue>+{0}% Tenacity</color>";
        lang[721] = "<color=darkblue>+{0}% Cooldown Reduction</color>";
        lang[722] = "<color=darkblue>+{0}% Attack Plus</color>";
        lang[723] = "<color=darkblue>+{0}% Magic Plus</color>";
        lang[724] = "<color=darkblue>+{0}% Health Plus</color>";
        lang[725] = "<color=darkblue>+{0}% Mana Plus</color>";
        lang[726] = "<color=red>+{0}% Damage Excellent</color>";
        lang[727] = "<color=red>+{0}% Excellent Defense</color>";
        lang[728] = "<color=red>+{0}% Double Damage</color>";
        lang[729] = "<color=red>+{0}% Triple Damage</color>";
        lang[730] = "<color=red>+{0}% Reflect Damage</color>";
        lang[731] = "<color=red>+{0}% Reward plus in battle</color>";
        lang[732] = "<color=purple>+{0} Jewel Socket</color>";
        lang[733] = "<color=purple>- (Empty)</color>";
        lang[734] = "<color=darkblue>+{0}% Armor</color>";
        lang[735] = "<color=darkblue>+{0}% Magic Resist</color>";

        #endregion

        #region Thông tin item 

        ItemInfor[200] = "Use in battle, restores 10% of missing health for all in 10 second";
        ItemInfor[201] = "Use in battle, restores 10% of missing health for all in 10 second";
        ItemInfor[202] = "Use in battle, restores 10% of missing health for all in 10 second";
        ItemInfor[203] = "Use in battle, restores 10% total amount of health for all in 5 second";
        ItemInfor[204] = "Use in battle, restores 20% total amount of health for all in 5 second";
        ItemInfor[205] = "Use in battle, restores 30% total amount of health for all in 5 second";
        ItemInfor[206] = "Use in battle, restores 30% total amount of health and buff 10% Health regen for all in 10 second";
        ItemInfor[207] = "Use in battle, restores 30% total amount of health and buff 10% Armor, Magic Resist for all in 10 second";
        ItemInfor[208] = "Use in battle, restores 30% total amount of health and buff 10% damage for all in 10 second";

        ItemInfor[210] = "Use this gem for upgrade item equip to level 1\nValues of item add 10% base value per level";
        ItemInfor[211] = "Use this gem for upgrade item equip to level 2\nValues of item add 10% base value per level";
        ItemInfor[212] = "Use this gem for upgrade item equip to level 3\nValues of item add 10% base value per level";
        ItemInfor[213] = "Use this gem for upgrade item equip to level 4\nValues of item add 10% base value per level";
        ItemInfor[214] = "Use this gem for upgrade item equip to level 5\nValues of item add 10% base value per level";
        ItemInfor[215] = "Use this gem for upgrade item equip to level 6\nValues of item add 10% base value per level";
        ItemInfor[216] = "Use this gem for upgrade item equip to level 7\nValues of item add 10% base value per level";
        ItemInfor[217] = "Use this gem for upgrade item equip to level 8\nValues of item add 10% base value per level";
        ItemInfor[218] = "Use this gem for upgrade item equip to level 9\nValues of item add 10% base value per level";
        ItemInfor[219] = "Use this gem for upgrade item equip to level 10\nValues of item add 10% base value per level";

        ItemInfor[220] = "Use this gem for change item equip color to level 1\nValues of item add 10% (base value + level value) per level color";
        ItemInfor[221] = "Use this gem for change item equip color to level 2\nValues of item add 10% (base value + level value) per level color";
        ItemInfor[222] = "Use this gem for change item equip color to level 3\nValues of item add 10% (base value + level value) per level color";
        ItemInfor[223] = "Use this gem for change item equip color to level 4\nValues of item add 10% (base value + level value) per level color";
        ItemInfor[224] = "Use this gem for change item equip color to level 5\nValues of item add 10% (base value + level value) per level color";
        ItemInfor[225] = "Use this gem for change item equip color to level 6\nValues of item add 10% (base value + level value) per level color";

        ItemInfor[230] = "Used to spin the lucky wheel, with the lucky wheel, you can get any item, from low value to extremely rare. This item can be found in every region";

        //ItemInfor nhiệm vụ
        ItemInfor[300] = "Manufacturing materials";
        ItemInfor[301] = "Manufacturing materials";
        ItemInfor[302] = "Manufacturing materials";
        ItemInfor[303] = "Manufacturing materials";
        ItemInfor[304] = "Manufacturing materials";
        ItemInfor[305] = "Manufacturing materials";
        ItemInfor[306] = "Manufacturing materials";
        ItemInfor[307] = "Manufacturing materials";
        ItemInfor[308] = "Manufacturing materials";
        ItemInfor[309] = "Manufacturing materials";
        ItemInfor[310] = "Manufacturing materials";
        ItemInfor[311] = "Manufacturing materials";
        ItemInfor[312] = "Manufacturing materials";
        ItemInfor[313] = "Manufacturing materials";

        ItemInfor[320] = "Manufacturing materials";
        ItemInfor[321] = "Manufacturing materials";
        ItemInfor[322] = "Manufacturing materials";
        ItemInfor[323] = "Manufacturing materials";
        ItemInfor[324] = "Manufacturing materials";
        ItemInfor[325] = "Manufacturing materials";

        ItemInfor[330] = "Manufacturing materials";
        ItemInfor[331] = "Manufacturing materials";
        ItemInfor[332] = "Manufacturing materials";
        ItemInfor[333] = "Manufacturing materials";
        ItemInfor[334] = "Manufacturing materials";
        ItemInfor[335] = "Manufacturing materials";
        ItemInfor[336] = "Manufacturing materials";
        ItemInfor[337] = "Manufacturing materials";
        ItemInfor[338] = "Manufacturing materials";
        ItemInfor[339] = "Manufacturing materials";
        ItemInfor[340] = "Manufacturing materials";

        ItemInfor[341] = "Manufacturing materials";
        ItemInfor[342] = "Manufacturing materials";
        ItemInfor[343] = "Manufacturing materials";
        ItemInfor[344] = "Manufacturing materials";
        ItemInfor[345] = "Manufacturing materials";
        ItemInfor[346] = "Manufacturing materials";
        ItemInfor[347] = "Manufacturing materials";
        ItemInfor[348] = "Manufacturing materials, contains tips for creating basic weapons and outfits";
        ItemInfor[349] = "Manufacturing materials, contains tips for creating high-quality weapons and outfits";

        ItemInfor[350] = "Manufacturing materials";
        ItemInfor[351] = "Manufacturing materials";
        ItemInfor[352] = "Manufacturing materials";
        ItemInfor[353] = "Manufacturing materials";
        ItemInfor[354] = "Manufacturing materials";

        ItemInfor[500] = "In Heimer gear sets, if fully equipped, will activate the following effects";
        ItemInfor[501] = "In Heimer gear sets, if fully equipped, will activate the following effects";
        ItemInfor[502] = "In Heimer gear sets, if fully equipped, will activate the following effects";
        ItemInfor[503] = "In Heimer gear sets, if fully equipped, will activate the following effects";
        ItemInfor[504] = "In Heimer gear sets, if fully equipped, will activate the following effects";
        ItemInfor[505] = "In Heimer gear sets, if fully equipped, will activate the following effects";
        ItemInfor[506] = "+5% Atk Magic";
        ItemInfor[507] = "+5% Life steal magic";
        ItemInfor[508] = "+5% Armor and Magic Resist";
        #endregion

        #region Giới thiệu skill 

        HeroSkillDescription[0] = "Rotate and launch four darts that deal 150% of physical damage each dart when hitting the nearest target";
        HeroSkillDescription[1] = "Surfing sword in a wide area can damage the entire enemy squad with 175% physical damage";
        HeroSkillDescription[2] = "Launches a boomerang that deals 135% physical damage to all opponents upon impact";
        HeroSkillDescription[3] = "Rotate very strong people to the ground, dealing 200% physical damage to targets within range";
        HeroSkillDescription[4] = "Transform the giant stick, smash into the ground with wide damage, 200% physical damage and stunning targets within range";
        HeroSkillDescription[5] = "Shoot a light beam across the map, dealing 200% magic damage";
        HeroSkillDescription[6] = "Spray out a range of fire through targets that inflict 20% of magic damage and burn targets for 5 seconds";
        HeroSkillDescription[7] = "Shoot a missile that deals 250% of physical damage with the first target hit";
        HeroSkillDescription[8] = "For 3 seconds, heal allies within a wide area with 300% of magic power";
        HeroSkillDescription[9] = "You are immune to damage, create a shield, block the enemy's remote damage";

        #endregion

        #region Giới thiệu nội tại 

        HeroIntrinsic[0] = "Each 3rd attack that is not more than 5 seconds apart between 2 strikes will deal 1 more damage and fly through the opponent";
        HeroIntrinsic[1] = "Comming Soon...!";
        HeroIntrinsic[2] = "Each 3rd attack spaced no more than 5 seconds apart between 2 attacks will fire 2 more darts";
        HeroIntrinsic[3] = "Comming Soon...!";
        HeroIntrinsic[4] = "Comming Soon...!";
        HeroIntrinsic[5] = "Each a normal attack has a 10% chance to slow down the opponent";
        HeroIntrinsic[6] = "Comming Soon...!";
        HeroIntrinsic[7] = "Comming Soon...!";
        HeroIntrinsic[8] = "A normal attack that collides with a teammate will heal the ally's health equal to 10% of the magic power";
        HeroIntrinsic[9] = "Comming Soon...!";

        #endregion

        #region Hướng dẫn 
        IntroductionTitle[0] = "Craft introduction";
        IntroductionTitle[1] = "Exploration introduction";
        #endregion

        #region Nội dung hướng dẫn 
        IntroductionDescriptions[0] = "Find resource and craft an item you want";
        IntroductionDescriptions[0] += "\nResource can found on every different region";
        IntroductionDescriptions[0] += "\nItem equip have random values, high values with item need many resources";
        IntroductionDescriptions[0] += "\nRatio craft successfully is 100%";
        IntroductionDescriptions[0] += "\nItem crafted can transfer to Black market";

        IntroductionDescriptions[1] = "Each region you choose to explore, there will be different opponents depending on the difficulty";
        IntroductionDescriptions[1] += "\nItems that drop in different places may also vary";
        IntroductionDescriptions[1] += "\nAfter each battle you win or give up halfway, the enemies in that land will be a little stronger";
        IntroductionDescriptions[1] += "\nIf the enemy in the area you explore is too strong, you can hire an assassin army to reduce the enemy's power through the 'Hire Assassin' function.";
        IntroductionDescriptions[1] += "\nYou will have the opportunity to recruit other enemies if your list of generals does not have them, but be careful, they are very strong.";
        IntroductionDescriptions[1] += "\nThere are 2 modes of land exploration, random and preview";
        IntroductionDescriptions[1] += "\nWith the preview mode, you can see your enemy team";
        IntroductionDescriptions[1] += "\nWith random mode, you will not know what you will encounter, the reward received in this mode will be higher";
        #endregion

        #region Tên item type 0 

        ItemNameType0[0] = "Knife 1";
        ItemNameType0[1] = "Knife 2";
        ItemNameType0[2] = "Knife 3";
        ItemNameType0[3] = "Knife 4";
        ItemNameType0[4] = "Knife 5";
        ItemNameType0[5] = "Knife 6";
        ItemNameType0[6] = "Knife 7";
        ItemNameType0[7] = "Sword 1";
        ItemNameType0[8] = "Sword 2";
        ItemNameType0[9] = "Sword 3";
        ItemNameType0[10] = "Sword 4";
        ItemNameType0[11] = "Sword 5";
        ItemNameType0[12] = "Sword 6";
        ItemNameType0[13] = "Sword 7";
        ItemNameType0[14] = "Sword 8";

        ItemNameType0[15] = "Rìu 1";
        ItemNameType0[16] = "Rìu 2";
        ItemNameType0[17] = "Rìu 3";
        ItemNameType0[18] = "Rìu 4";
        ItemNameType0[19] = "Rìu 5";
        ItemNameType0[20] = "Rìu 6";
        ItemNameType0[21] = "Rìu 7";
        ItemNameType0[22] = "Rìu 8";
        ItemNameType0[23] = "Rìu 9";
        ItemNameType0[24] = "Rìu 10";
        ItemNameType0[25] = "Rìu 11";
        ItemNameType0[26] = "Rìu 12";

        ItemNameType0[27] = "Búa 1";
        ItemNameType0[28] = "Búa 2";
        ItemNameType0[29] = "Búa 3";
        ItemNameType0[30] = "Búa 4";
        ItemNameType0[31] = "Búa 5";
        ItemNameType0[32] = "Búa 6";
        ItemNameType0[33] = "Búa 7";
        ItemNameType0[34] = "Búa 8";
        ItemNameType0[35] = "Búa 9";
        ItemNameType0[36] = "Búa 10";
        ItemNameType0[37] = "Búa 11";
        ItemNameType0[38] = "Búa 12";

        ItemNameType0[39] = "Chùy 1";
        ItemNameType0[40] = "Chùy 2";
        ItemNameType0[41] = "Chùy 3";
        ItemNameType0[42] = "Chùy 4";
        ItemNameType0[43] = "Chùy 5";
        ItemNameType0[44] = "Chùy 6";
        ItemNameType0[45] = "Chùy 7";
        ItemNameType0[46] = "Chùy 8";
        ItemNameType0[47] = "Chùy 9";
        ItemNameType0[48] = "Chùy 10";
        ItemNameType0[49] = "Chùy 11";
        ItemNameType0[50] = "Cung 1";
        ItemNameType0[51] = "Cung 2";
        ItemNameType0[52] = "Cung 3";
        ItemNameType0[53] = "Cung 4";
        ItemNameType0[54] = "Cung 5";
        ItemNameType0[55] = "Nỏ 1";
        ItemNameType0[56] = "Nỏ 2";
        ItemNameType0[57] = "Nỏ 3";
        ItemNameType0[58] = "Nỏ 4";
        ItemNameType0[59] = "Súng 1";
        ItemNameType0[60] = "Súng 2";
        ItemNameType0[61] = "Súng 3";
        ItemNameType0[62] = "Súng 4";
        ItemNameType0[63] = "Súng 5";
        ItemNameType0[64] = "Súng 6";
        ItemNameType0[65] = "Súng 7";
        ItemNameType0[66] = "Súng 8";

        #endregion

        #region Tên item type 1 

        ItemNameType1[0] = "Trượng 1";
        ItemNameType1[1] = "Trượng 2";
        ItemNameType1[2] = "Trượng 3";
        ItemNameType1[3] = "Trượng 4";
        ItemNameType1[4] = "Trượng 5";
        ItemNameType1[5] = "Trượng 6";
        ItemNameType1[6] = "Trượng 7";
        ItemNameType1[7] = "Trượng 8";

        #endregion

        #region Tên item type 2 

        ItemNameType2[0] = "Áo choàng 1";
        ItemNameType2[1] = "Áo choàng 2";
        ItemNameType2[2] = "Áo choàng 3";
        ItemNameType2[3] = "Áo choàng 4";
        ItemNameType2[4] = "Áo choàng 5";
        ItemNameType2[5] = "Áo choàng 6";

        #endregion

        #region Tên item type 3 

        ItemNameType3[0] = "Ring 1";
        ItemNameType3[1] = "Ring 2";
        ItemNameType3[2] = "Ring 3";
        ItemNameType3[3] = "Ring 4";
        ItemNameType3[4] = "Ring 5";

        #endregion

        #region Tên item type 4 

        ItemNameType4[0] = "Armor 1";
        ItemNameType4[1] = "Armor 2";
        ItemNameType4[2] = "Armor 3";
        ItemNameType4[3] = "Armor 4";
        ItemNameType4[4] = "Armor 5";
        ItemNameType4[5] = "Armor 6";
        ItemNameType4[6] = "Armor 7";
        ItemNameType4[7] = "Armor 8";
        ItemNameType4[8] = "Armor 9";
        ItemNameType4[9] = "Armor 10";

        #endregion

        #region Tên item type 5 

        ItemNameType5[0] = "Đai lưng 1";
        ItemNameType5[1] = "Đai lưng 2";
        ItemNameType5[2] = "Đai lưng 3";
        ItemNameType5[3] = "Đai lưng 4";
        ItemNameType5[4] = "Đai lưng 5";
        ItemNameType5[5] = "Đai lưng 6";

        #endregion

        #region Tên item type 6 

        ItemNameType6[0] = "Giáp tay 1";
        ItemNameType6[1] = "Giáp tay 2";
        ItemNameType6[2] = "Giáp tay 3";
        ItemNameType6[3] = "Giáp tay 4";
        ItemNameType6[4] = "Giáp tay 5";
        ItemNameType6[5] = "Giáp tay 6";
        ItemNameType6[6] = "Giáp tay 7";
        ItemNameType6[7] = "Giáp tay 8";
        ItemNameType6[8] = "Giáp tay 9";
        ItemNameType6[9] = "Giáp tay 10";

        #endregion

        #region Tên item type 7 

        ItemNameType7[0] = "Găng tay 1";
        ItemNameType7[1] = "Găng tay 2";
        ItemNameType7[2] = "Găng tay 3";
        ItemNameType7[3] = "Găng tay 4";
        ItemNameType7[4] = "Găng tay 5";

        #endregion

        #region Tên item type 8 

        ItemNameType8[0] = "Shield 1";
        ItemNameType8[1] = "Shield 2";
        ItemNameType8[2] = "Shield 3";
        ItemNameType8[3] = "Shield 4";
        ItemNameType8[4] = "Shield 5";

        #endregion

        #region Tên item type 10 

        ItemNameType10[0] = "Health potion 1";
        ItemNameType10[1] = "Health potion 2";
        ItemNameType10[2] = "Health potion 3";
        ItemNameType10[3] = "Health potion 4";
        ItemNameType10[4] = "Health potion 5";
        ItemNameType10[5] = "Health potion 6";
        ItemNameType10[6] = "Health potion 7";
        ItemNameType10[7] = "Health potion 8";
        ItemNameType10[8] = "Health potion 9";

        ItemNameType10[9] = "Gem upgrade lv 1";
        ItemNameType10[10] = "Gem upgrade lv 2";
        ItemNameType10[11] = "Gem upgrade lv 3";
        ItemNameType10[12] = "Gem upgrade lv 4";
        ItemNameType10[13] = "Gem upgrade lv 5";
        ItemNameType10[14] = "Gem upgrade lv 6";
        ItemNameType10[15] = "Gem upgrade lv 7";
        ItemNameType10[16] = "Gem upgrade lv 8";
        ItemNameType10[17] = "Gem upgrade lv 9";
        ItemNameType10[18] = "Gem upgrade lv 10";

        ItemNameType10[19] = "Gem up quality lv 1";
        ItemNameType10[20] = "Gem up quality lv 2";
        ItemNameType10[21] = "Gem up quality lv 3";
        ItemNameType10[22] = "Gem up quality lv 4";
        ItemNameType10[23] = "Gem up quality lv 5";
        ItemNameType10[24] = "Gem up quality lv 6";

        ItemNameType10[25] = "Phoenix Feather";
        ItemNameType10[26] = "Socket model";

        #endregion

        #region Thông tin item type 10 

        ItemInforType10[0] = "Use in battle, restores 10% of missing health for all in 10 second";
        ItemInforType10[1] = "Use in battle, restores 10% of missing health for all in 10 second";
        ItemInforType10[2] = "Use in battle, restores 10% of missing health for all in 10 second";
        ItemInforType10[3] = "Use in battle, restores 10% total amount of health for all in 5 second";
        ItemInforType10[4] = "Use in battle, restores 20% total amount of health for all in 5 second";
        ItemInforType10[5] = "Use in battle, restores 30% total amount of health for all in 5 second";
        ItemInforType10[6] = "Use in battle, restores 30% total amount of health and buff 10% Health regen for all in 10 second";
        ItemInforType10[7] = "Use in battle, restores 30% total amount of health and buff 10% Armor, Magic Resist for all in 10 second";
        ItemInforType10[8] = "Use in battle, restores 30% total amount of health and buff 10% damage for all in 10 second";

        ItemInforType10[9] = "Use this gem for upgrade item equip to level 1\nValues of item add 10% base value per level";
        ItemInforType10[10] = "Use this gem for upgrade item equip to level 2\nValues of item add 10% base value per level";
        ItemInforType10[11] = "Use this gem for upgrade item equip to level 3\nValues of item add 10% base value per level";
        ItemInforType10[12] = "Use this gem for upgrade item equip to level 4\nValues of item add 10% base value per level";
        ItemInforType10[13] = "Use this gem for upgrade item equip to level 5\nValues of item add 10% base value per level";
        ItemInforType10[14] = "Use this gem for upgrade item equip to level 6\nValues of item add 10% base value per level";
        ItemInforType10[15] = "Use this gem for upgrade item equip to level 7\nValues of item add 10% base value per level";
        ItemInforType10[16] = "Use this gem for upgrade item equip to level 8\nValues of item add 10% base value per level";
        ItemInforType10[17] = "Use this gem for upgrade item equip to level 9\nValues of item add 10% base value per level";
        ItemInforType10[18] = "Use this gem for upgrade item equip to level 10\nValues of item add 10% base value per level";

        ItemInforType10[19] = "Use this gem for change item equip color to level 1\nValues of item add 10% (base value + level value) per level color";
        ItemInforType10[20] = "Use this gem for change item equip color to level 2\nValues of item add 10% (base value + level value) per level color";
        ItemInforType10[21] = "Use this gem for change item equip color to level 3\nValues of item add 10% (base value + level value) per level color";
        ItemInforType10[22] = "Use this gem for change item equip color to level 4\nValues of item add 10% (base value + level value) per level color";
        ItemInforType10[23] = "Use this gem for change item equip color to level 5\nValues of item add 10% (base value + level value) per level color";
        ItemInforType10[24] = "Use this gem for change item equip color to level 6\nValues of item add 10% (base value + level value) per level color";

        ItemInforType10[25] = "Used to spin the lucky wheel, with the lucky wheel, you can get any item, from low value to extremely rare. This item can be found in every region";
        ItemInforType10[26] = "Used to create a socket in equip item";

        #endregion

        #region Tên item type 11 

        ItemNameType11[0] = "Gelsemium elegans";
        ItemNameType11[1] = "White chrysanthemum";
        ItemNameType11[2] = "Thorn grass";
        ItemNameType11[3] = "Strychnos nux-vomica";
        ItemNameType11[4] = "Water lily";
        ItemNameType11[5] = "SAP";
        ItemNameType11[6] = "Blue chrysanthemum";
        ItemNameType11[7] = "Cherry";
        ItemNameType11[8] = "Oleander ";
        ItemNameType11[9] = "Cactus spines";
        ItemNameType11[10] = "Hemp leaves";
        ItemNameType11[11] = "Red pepper";
        ItemNameType11[12] = "Monkey leaves";
        ItemNameType11[13] = "Weed";

        ItemNameType11[14] = "Raw cloth";
        ItemNameType11[15] = "Wool cloth";
        ItemNameType11[16] = "Linen cloth";
        ItemNameType11[17] = "Jeans cloth";
        ItemNameType11[18] = "Cotton cloth";
        ItemNameType11[19] = "Silk cloth";

        ItemNameType11[20] = "Metal ores";
        ItemNameType11[21] = "Rock ore";
        ItemNameType11[22] = "Crom ore";
        ItemNameType11[23] = "Copper ore";
        ItemNameType11[24] = "Gold ore";
        ItemNameType11[25] = "Ruby ore";
        ItemNameType11[26] = "Emerald ore";
        ItemNameType11[27] = "Quartz ore";
        ItemNameType11[28] = "Tiger eye ores";
        ItemNameType11[29] = "Sapphire ore";
        ItemNameType11[30] = "Diamond ore";

        ItemNameType11[31] = "Bough";
        ItemNameType11[32] = "Clapboard";
        ItemNameType11[33] = "Large wooden board";
        ItemNameType11[34] = "Pine";
        ItemNameType11[35] = "Agarwood";
        ItemNameType11[36] = "Oval wood";
        ItemNameType11[37] = "Ironwood";
        ItemNameType11[38] = "Ancient letter";
        ItemNameType11[39] = "Ancient book";

        ItemNameType11[40] = "Crystal whirlwind";
        ItemNameType11[41] = "Crystal blazing fire";
        ItemNameType11[42] = "Crystal tide";
        ItemNameType11[43] = "Crystal heaven";
        ItemNameType11[44] = "Feather";

        #endregion

        #region Thông tin item type 11 

        ItemInforType11[0] = "Manufacturing materials";
        ItemInforType11[1] = "Manufacturing materials";
        ItemInforType11[2] = "Manufacturing materials";
        ItemInforType11[3] = "Manufacturing materials";
        ItemInforType11[4] = "Manufacturing materials";
        ItemInforType11[5] = "Manufacturing materials";
        ItemInforType11[6] = "Manufacturing materials";
        ItemInforType11[7] = "Manufacturing materials";
        ItemInforType11[8] = "Manufacturing materials";
        ItemInforType11[9] = "Manufacturing materials";
        ItemInforType11[10] = "Manufacturing materials";
        ItemInforType11[11] = "Manufacturing materials";
        ItemInforType11[12] = "Manufacturing materials";
        ItemInforType11[13] = "Manufacturing materials";

        ItemInforType11[14] = "Manufacturing materials";
        ItemInforType11[15] = "Manufacturing materials";
        ItemInforType11[16] = "Manufacturing materials";
        ItemInforType11[17] = "Manufacturing materials";
        ItemInforType11[18] = "Manufacturing materials";
        ItemInforType11[19] = "Manufacturing materials";

        ItemInforType11[20] = "Manufacturing materials";
        ItemInforType11[21] = "Manufacturing materials";
        ItemInforType11[22] = "Manufacturing materials";
        ItemInforType11[23] = "Manufacturing materials";
        ItemInforType11[24] = "Manufacturing materials";
        ItemInforType11[25] = "Manufacturing materials";
        ItemInforType11[26] = "Manufacturing materials";
        ItemInforType11[27] = "Manufacturing materials";
        ItemInforType11[28] = "Manufacturing materials";
        ItemInforType11[29] = "Manufacturing materials";
        ItemInforType11[30] = "Manufacturing materials";

        ItemInforType11[31] = "Manufacturing materials";
        ItemInforType11[32] = "Manufacturing materials";
        ItemInforType11[33] = "Manufacturing materials";
        ItemInforType11[34] = "Manufacturing materials";
        ItemInforType11[35] = "Manufacturing materials";
        ItemInforType11[36] = "Manufacturing materials";
        ItemInforType11[37] = "Manufacturing materials";
        ItemInforType11[38] = "Manufacturing materials, contains tips for creating basic weapons and outfits";
        ItemInforType11[39] = "Manufacturing materials, contains tips for creating high-quality weapons and outfits";

        ItemInforType11[40] = "Manufacturing materials";
        ItemInforType11[41] = "Manufacturing materials";
        ItemInforType11[42] = "Manufacturing materials";
        ItemInforType11[43] = "Manufacturing materials";
        ItemInforType11[44] = "Manufacturing materials";

        #endregion

        #region Tên item type 12

        ItemNameType12[0] = "Attack jewel";
        ItemNameType12[1] = "Magic Penetration jewel";
        ItemNameType12[2] = "Lethality jewel";
        ItemNameType12[3] = "Reflect Damage jewel";
        ItemNameType12[4] = "Mana Plus jewel";
        ItemNameType12[5] = "Mana Regen jewel";
        ItemNameType12[6] = "Mana jewel";
        ItemNameType12[7] = "Life Steal Magic jewel";
        ItemNameType12[8] = "Magic Plus jewel";
        ItemNameType12[9] = "Magic jewel";
        ItemNameType12[10] = "Magic Resist Plus jewel";
        ItemNameType12[11] = "Magic Resist jewel";
        ItemNameType12[12] = "Tenacity jewel";
        ItemNameType12[13] = "Health Regen jewel";
        ItemNameType12[14] = "Health Plus jewel";
        ItemNameType12[15] = "Health jewel";
        ItemNameType12[16] = "Reward Plus jewel";
        ItemNameType12[17] = "Armor Plus jewel";
        ItemNameType12[18] = "Armor jewel";
        ItemNameType12[19] = "Cooldown Reduction jewel";
        ItemNameType12[20] = "Water Defense jewel";
        ItemNameType12[21] = "Excellent Defense jewel";
        ItemNameType12[22] = "Fire Defense jewel";
        ItemNameType12[23] = "Earth Defense jewel";
        ItemNameType12[24] = "TripleDamage jewel";
        ItemNameType12[25] = "Double Damage jewel";
        ItemNameType12[26] = "Water Damage jewel";
        ItemNameType12[27] = "Atk Speed jewel";
        ItemNameType12[28] = "Life Steal Physic jewel";
        ItemNameType12[29] = "Excellent Damage jewel";
        ItemNameType12[30] = "Fire Damage jewel";
        ItemNameType12[31] = "Earth Damage jewel";
        ItemNameType12[32] = "Critical jewel";
        ItemNameType12[33] = "Attack Plus jewel";

        #endregion

        #region Thông tin item type 12

        ItemInforType12[0] =  "Random value when insert to item equip";
        ItemInforType12[1] =  "Random value when insert to item equip";
        ItemInforType12[2] =  "Random value when insert to item equip";
        ItemInforType12[3] =  "Random value when insert to item equip";
        ItemInforType12[4] =  "Random value when insert to item equip";
        ItemInforType12[5] =  "Random value when insert to item equip";
        ItemInforType12[6] =  "Random value when insert to item equip";
        ItemInforType12[7] =  "Random value when insert to item equip";
        ItemInforType12[8] =  "Random value when insert to item equip";
        ItemInforType12[9] =  "Random value when insert to item equip";
        ItemInforType12[10] = "Random value when insert to item equip";
        ItemInforType12[11] = "Random value when insert to item equip";
        ItemInforType12[12] = "Random value when insert to item equip";
        ItemInforType12[13] = "Random value when insert to item equip";
        ItemInforType12[14] = "Random value when insert to item equip";
        ItemInforType12[15] = "Random value when insert to item equip";
        ItemInforType12[16] = "Random value when insert to item equip";
        ItemInforType12[17] = "Random value when insert to item equip";
        ItemInforType12[18] = "Random value when insert to item equip";
        ItemInforType12[19] = "Random value when insert to item equip";
        ItemInforType12[20] = "Random value when insert to item equip";
        ItemInforType12[21] = "Random value when insert to item equip";
        ItemInforType12[22] = "Random value when insert to item equip";
        ItemInforType12[23] = "Random value when insert to item equip";
        ItemInforType12[24] = "Random value when insert to item equip";
        ItemInforType12[25] = "Random value when insert to item equip";
        ItemInforType12[26] = "Random value when insert to item equip";
        ItemInforType12[27] = "Random value when insert to item equip";
        ItemInforType12[28] = "Random value when insert to item equip";
        ItemInforType12[29] = "Random value when insert to item equip";
        ItemInforType12[30] = "Random value when insert to item equip";
        ItemInforType12[31] = "Random value when insert to item equip";
        ItemInforType12[32] = "Random value when insert to item equip";
        ItemInforType12[33] = "Random value when insert to item equip";

        #endregion

    }
}