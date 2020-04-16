using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Code._0.DTO.Models
{
    public class DataString
    {
        //0 = Thông tin tài khoản
        //1 = Danh sách tướng
        //2 = Inventory
        public int DataType ;
        public bool IsPut ;//Put hoặc get data 
        public string UserID ;
        public string Values ;
    }
}
