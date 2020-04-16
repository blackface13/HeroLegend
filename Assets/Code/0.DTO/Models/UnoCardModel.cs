using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Code._0.DTO.Models
{
    //Thiết kế bộ bài uno bao gồm các fields
    //- ColorType(int: 0-4, 4 màu và 4 là đặc biệt-các lá bài đa năng)
    //- NumberID(int: 0-9 là giá trị các quân bài thường, 10: cấm lượt, 11: đổi chiều, 12: +2, 13: cấm lượt x2, 14: bão tố, 15: xả bài, 16: đổi bài)
    //- PointCard: (int: các lá bài thường có số điểm tương ứng ghi trên bài, các lá xoay, skip, +2: 20d, đổi màu và đổi màu +4: 50d)
    //- CardImg: Tên hình ảnh card
    public class UnoCardModel
    {
        public int ColorType ;
        public int NumberID ;
        public int CardPoint ;
        public string CardImg ;

        public UnoCardModel() { }
        public UnoCardModel Clone()
        {
            return (UnoCardModel)this.MemberwiseClone();
        }
        public UnoCardModel(int colorType, int numberID, int cardPoint, string cardImg)
        {
            ColorType = colorType;
            NumberID  = numberID ;
            CardPoint = cardPoint;
            CardImg = cardImg;
        }
    }
}
