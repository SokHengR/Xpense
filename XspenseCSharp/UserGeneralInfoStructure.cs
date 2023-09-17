using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XspenseCSharp
{
    enum TransectionTypeEnum
    {
        expense, income
    }
    struct UserGeneralInfoStruct
    {
        public List<WalletStruct> wallet { get; set; }
        public List<CategoryStruct> category { get; set; }
        public List<CurrencyStruct> currency { get; set; }
    }
    struct TransectionStruct
    {
        public TransectionTypeEnum type { get; set; }
        public string uuid { get; set; }
        public DateTime date { get; set; }
        public float price { get; set; }
        public CategoryStruct category { get; set; }
    }
    struct CategoryStruct
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
    struct CurrencyStruct
    {
        public string uuid { get; set; }
        public string full_name { get; set; }
        public string code_name { get; set; }
        public float exchange_rate { get; set; }
    }
    struct WalletStruct
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public CurrencyStruct currency { get; set; }
        public List<TransectionStruct> transection { get; set; }
    }
}
