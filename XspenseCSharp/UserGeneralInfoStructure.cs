using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XspenseCSharp
{
    public enum TransectionTypeEnum
    {
        expense, income
    }
    public enum PickDateEnum
    {
        today, yesterday, this_week, this_month, this_year
    }
    public struct UserGeneralInfoStruct
    {
        public List<WalletStruct> wallet { get; set; }
        public List<CategoryStruct> category { get; set; }
        public List<CurrencyStruct> currency { get; set; }
    }
    public struct TransectionStruct
    {
        public TransectionTypeEnum type { get; set; }
        public string uuid { get; set; }
        public DateTime date { get; set; }
        public float price { get; set; }
        public string category_id { get; set; }
        public string wallet_id {  get; set; }
    }
    public struct CategoryStruct
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
    public struct CurrencyStruct
    {
        public string uuid { get; set; }
        public string full_name { get; set; }
        public string code_name { get; set; }
        public float exchange_rate { get; set; }
    }
    public struct WalletStruct
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public string currency_id { get; set; }
        public List<TransectionStruct> transection { get; set; }
    }

    public struct TransectionPresentStruct
    {
        public TransectionTypeEnum Type { get; set; }
        public string uuid { get; set; }
        public string wallet_id { get; set; }
        public string category_id { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
        public string Wallet {  get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
    }
}
