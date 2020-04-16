using Assets.Code._0.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Code._4.CORE.UnoCard
{
    public static class UnoCardSystem
    {
        #region Variables
        public static List<UnoCardModel> UnoCards;
        public static int MaxCardTemp = 10;//Số card tối đa hiển thị dưới bàn khi đã đánh, vượt quá số này sẽ tự động remove card
        public static int StartCardNumber = 7;//Số card mỗi người khi bắt đầu ván mới
        public static float RangeMinBetween2Card = 1.2f;//Khoảng cách tối thiếu giữa 2 card
        public static float RangeCardMoveUp = 1.2f;//Khoảng cách card di chuyển lên
        public static float DelayTimeCreateCard = 0.1f;//Khoảng thời gian giữa 2 lần tạo card mới để phát
        public static float DelayTimeMoveCard = 0.15f;//Khoảng thời di chuyển card tới chỗ player hoặc đánh
        public static float[] CardAnglePos = new float[] { 0, 30, 60, 90, 120, 150 }; //Các tọa độ xoay bài ngẫu nhiên khi vứt bài xuống bàn
        public static Vector3 SizeToTable = new Vector3(1f, 1f, 1f);//Kích thước của lá bài khi thả xuống bàn
        public static Vector3 SizeCardAI = new Vector3(1.3f, 1.3f, 1.3f);//Kích thước lá bài của các người chơi AI
        public static Vector3 SizeCardPlayer = new Vector3(1.5f, 1.5f, 1.5f);//Kích thước lá bài của Player
        public static List<Color32> ClrsCard;
        public static float TimeDelayGetCard = .15f;//Thời gian delay rút card trước khi đẩy vào bộ bài
        public static float TimeDelayAIAction = 1f;//Thời gian chờ đợi AI chơi
        public static float TimeDelayGameEnding = 1f;//Thời gian chờ trước khi end game
        public static int MinRandomPustCardToVictim = 1;//Thẻ bài rút ngẫu nhiên tối thiểu bao nhiêu lá
        public static int MaxRandomPustCardToVictim = 9;//Thẻ bài rút ngẫu nhiên tối đa bao nhiêu lá
        public static int[] BetLevel = new int[] { 500, 1000, 1500, 2000, 3000, 5000 };//Các mức độ cược
        public static int BetLevelSlot = 0;
        public static int[] PlayersNumber = new int[] { 2, 3, 4, 6 };//Số lượng ng chơi
        public static int PlayerNumberSlot = 0;
        #endregion

        public static void SetupUnoCard()
        {
            //Thoát nếu đã khởi tạo
            if (UnoCards != null)
                return;

            //Khởi tạo màu sắc card
            ClrsCard = new List<Color32>();
            ClrsCard.Add(new Color32(255, 85, 85, 255));//Đỏ
            ClrsCard.Add(new Color32(255, 170, 0, 255));//Vàng
            ClrsCard.Add(new Color32(85, 170, 85, 255));//Lục
            ClrsCard.Add(new Color32(85, 85, 253, 255));//Lam

            //Khởi tạo danh sách card
            UnoCards = new List<UnoCardModel>();

            //Thiết kế bộ bài uno bao gồm các fields
            //- ColorType(int: 0-5, 4 màu và 5 là đặc biệt-các lá bài đa năng)
            //- NumberID(int: 0-9 là giá trị các quân bài thường, 10: cấm lượt, 11: đổi chiều, 12: +2, 13: cấm lượt x2, 14: bão tố, 15: xả bài, 16: đổi bài)
            //- PointCard: (int: các lá bài thường có số điểm tương ứng ghi trên bài, các lá xoay, skip, +2: 20d, đổi màu và đổi màu +4: 50d)
            //- CardImg: Tên hình ảnh card

            //Bài màu vàng
            UnoCards.Add(new UnoCardModel(0, 0, 0, "UnoCard_0"));
            UnoCards.Add(new UnoCardModel(0, 1, 1, "UnoCard_1"));
            UnoCards.Add(new UnoCardModel(0, 2, 2, "UnoCard_2"));
            UnoCards.Add(new UnoCardModel(0, 3, 3, "UnoCard_3"));
            UnoCards.Add(new UnoCardModel(0, 4, 4, "UnoCard_4"));
            UnoCards.Add(new UnoCardModel(0, 5, 5, "UnoCard_5"));
            UnoCards.Add(new UnoCardModel(0, 6, 6, "UnoCard_6"));
            UnoCards.Add(new UnoCardModel(0, 7, 7, "UnoCard_7"));
            UnoCards.Add(new UnoCardModel(0, 8, 8, "UnoCard_8"));
            UnoCards.Add(new UnoCardModel(0, 9, 9, "UnoCard_9"));
            UnoCards.Add(new UnoCardModel(0, 10, 20, "UnoCard_10"));//cấm lượt 
            UnoCards.Add(new UnoCardModel(0, 11, 20, "UnoCard_11"));//đổi chiều
            UnoCards.Add(new UnoCardModel(0, 12, 20, "UnoCard_12"));//+2
            UnoCards.Add(new UnoCardModel(0, 13, 20, "UnoCard_13"));//cấm lượt x2
            UnoCards.Add(new UnoCardModel(0, 14, 20, "UnoCard_14"));//bão tố
            UnoCards.Add(new UnoCardModel(0, 15, 20, "UnoCard_15"));//xả bài
            //UnoCards.Add(new UnoCardModel(0, 16, 20, "UnoCard_16"));//đổi bài

            //Bài màu đỏ
            UnoCards.Add(new UnoCardModel(1, 0, 0, "UnoCard_17"));
            UnoCards.Add(new UnoCardModel(1, 1, 1, "UnoCard_18"));
            UnoCards.Add(new UnoCardModel(1, 2, 2, "UnoCard_19"));
            UnoCards.Add(new UnoCardModel(1, 3, 3, "UnoCard_20"));
            UnoCards.Add(new UnoCardModel(1, 4, 4, "UnoCard_21"));
            UnoCards.Add(new UnoCardModel(1, 5, 5, "UnoCard_22"));
            UnoCards.Add(new UnoCardModel(1, 6, 6, "UnoCard_23"));
            UnoCards.Add(new UnoCardModel(1, 7, 7, "UnoCard_24"));
            UnoCards.Add(new UnoCardModel(1, 8, 8, "UnoCard_25"));
            UnoCards.Add(new UnoCardModel(1, 9, 9, "UnoCard_26"));
            UnoCards.Add(new UnoCardModel(1, 10, 20, "UnoCard_27"));//cấm lượt
            UnoCards.Add(new UnoCardModel(1, 11, 20, "UnoCard_28"));//đổi chiều
            UnoCards.Add(new UnoCardModel(1, 12, 20, "UnoCard_29"));//+2
            UnoCards.Add(new UnoCardModel(1, 13, 20, "UnoCard_30"));//cấm lượt x2
            UnoCards.Add(new UnoCardModel(1, 14, 20, "UnoCard_31"));//bão tố
            UnoCards.Add(new UnoCardModel(1, 15, 20, "UnoCard_32"));//xả bài
            //UnoCards.Add(new UnoCardModel(1, 16, 20, "UnoCard_33"));//đổi bài

            //Bài màu xanh lá
            UnoCards.Add(new UnoCardModel(2, 0, 0, "UnoCard_34"));
            UnoCards.Add(new UnoCardModel(2, 1, 1, "UnoCard_35"));
            UnoCards.Add(new UnoCardModel(2, 2, 2, "UnoCard_36"));
            UnoCards.Add(new UnoCardModel(2, 3, 3, "UnoCard_37"));
            UnoCards.Add(new UnoCardModel(2, 4, 4, "UnoCard_38"));
            UnoCards.Add(new UnoCardModel(2, 5, 5, "UnoCard_39"));
            UnoCards.Add(new UnoCardModel(2, 6, 6, "UnoCard_40"));
            UnoCards.Add(new UnoCardModel(2, 7, 7, "UnoCard_41"));
            UnoCards.Add(new UnoCardModel(2, 8, 8, "UnoCard_42"));
            UnoCards.Add(new UnoCardModel(2, 9, 9, "UnoCard_43"));
            UnoCards.Add(new UnoCardModel(2, 10, 20, "UnoCard_44"));//cấm lượt
            UnoCards.Add(new UnoCardModel(2, 11, 20, "UnoCard_45"));//đổi chiều
            UnoCards.Add(new UnoCardModel(2, 12, 20, "UnoCard_46"));//+2
            UnoCards.Add(new UnoCardModel(2, 13, 20, "UnoCard_47"));//cấm lượt x2
            UnoCards.Add(new UnoCardModel(2, 14, 20, "UnoCard_48"));//bão tố
            UnoCards.Add(new UnoCardModel(2, 15, 20, "UnoCard_49"));//xả bài
            //UnoCards.Add(new UnoCardModel(2, 16, 20, "UnoCard_50"));//đổi bài

            //Bài màu xanh dương
            UnoCards.Add(new UnoCardModel(3, 0, 0, "UnoCard_51"));
            UnoCards.Add(new UnoCardModel(3, 1, 1, "UnoCard_52"));
            UnoCards.Add(new UnoCardModel(3, 2, 2, "UnoCard_53"));
            UnoCards.Add(new UnoCardModel(3, 3, 3, "UnoCard_54"));
            UnoCards.Add(new UnoCardModel(3, 4, 4, "UnoCard_55"));
            UnoCards.Add(new UnoCardModel(3, 5, 5, "UnoCard_56"));
            UnoCards.Add(new UnoCardModel(3, 6, 6, "UnoCard_57"));
            UnoCards.Add(new UnoCardModel(3, 7, 7, "UnoCard_58"));
            UnoCards.Add(new UnoCardModel(3, 8, 8, "UnoCard_59"));
            UnoCards.Add(new UnoCardModel(3, 9, 9, "UnoCard_60"));
            UnoCards.Add(new UnoCardModel(3, 10, 20, "UnoCard_61"));//cấm lượt
            UnoCards.Add(new UnoCardModel(3, 11, 20, "UnoCard_62"));//đổi chiều
            UnoCards.Add(new UnoCardModel(3, 12, 20, "UnoCard_63"));//+2
            UnoCards.Add(new UnoCardModel(3, 13, 20, "UnoCard_64"));//cấm lượt x2
            UnoCards.Add(new UnoCardModel(3, 14, 20, "UnoCard_65"));//bão tố
            UnoCards.Add(new UnoCardModel(3, 15, 20, "UnoCard_66"));//xả bài
            //UnoCards.Add(new UnoCardModel(3, 16, 20, "UnoCard_67"));//đổi bài

            //Bài đặc biệt
            UnoCards.Add(new UnoCardModel(4, 17, 50, "UnoCard_68"));//Đổi màu
            UnoCards.Add(new UnoCardModel(4, 18, 50, "UnoCard_69"));//Đổi màu +4
                                                                    //UnoCards.Add(new UnoCardModel(4, 19, 50, "UnoCard_70"));//Battle
            UnoCards.Add(new UnoCardModel(4, 20, 50, "UnoCard_71"));//Đổi màu +4 vào mục tiêu
            UnoCards.Add(new UnoCardModel(4, 21, 50, "UnoCard_72"));//Cộng ngẫu nhiên
            //UnoCards.Add(new UnoCardModel(4, 22, 50, "UnoCard_73"));//Up
        }
    }
}
