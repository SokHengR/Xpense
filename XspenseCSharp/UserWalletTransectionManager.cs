using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace XspenseCSharp
{
    internal class UserWalletTransectionManager
    {
        public static UserWalletTransectionManager shared = new UserWalletTransectionManager();
        NativeFileManager fileManagerNative = NativeFileManager.shared;
        public UserGeneralInfoStruct loadStructFromFile(string fileName)
        {
            if (fileManagerNative.IsFileExists(fileName))
            {
                string jsonString = fileManagerNative.ReadTextFromFile(fileName);
                UserGeneralInfoStruct userGeneralInfo = JsonConvert.DeserializeObject<UserGeneralInfoStruct>(jsonString);
                return userGeneralInfo;
            }
            else
            {
                UserGeneralInfoStruct userGeneralInfo = new UserGeneralInfoStruct();
                userGeneralInfo.wallet = new List<WalletStruct>();
                userGeneralInfo.currency = new List<CurrencyStruct>();
                userGeneralInfo.category = new List<CategoryStruct>();
                string jsonString = JsonConvert.SerializeObject(userGeneralInfo);
                fileManagerNative.SaveTextToFile(jsonString, fileName);
                return userGeneralInfo;
            }
        }
        public void saveStructToFile(UserGeneralInfoStruct structData, string fileName)
        {
            string jsonString = JsonConvert.SerializeObject(structData);
            fileManagerNative.SaveTextToFile(jsonString, fileName);
        }
        // wallet -------------------------
        public UserGeneralInfoStruct deleteWallet(UserGeneralInfoStruct originalStruct, WalletStruct theWallet)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            newStruct.wallet.Remove(theWallet);
            return newStruct;
        }
        public UserGeneralInfoStruct addWallet(UserGeneralInfoStruct originalStruct, WalletStruct theWallet)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            newStruct.wallet.Add(theWallet);
            return newStruct;
        }
        public UserGeneralInfoStruct editWallet(UserGeneralInfoStruct originalStruct, WalletStruct theWallet)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            for (int i = 0; i < originalStruct.wallet.Count; i++)
            {
                if (originalStruct.wallet[i].uuid == theWallet.uuid)
                {
                    newStruct.wallet[i] = theWallet;
                }
            }
            return newStruct;
        }
        // transection -------------------------
        public WalletStruct deleteTransection(WalletStruct originalStruct, TransectionStruct theTransection)
        {
            WalletStruct newStruct = originalStruct;
            newStruct.transection.Remove(theTransection);
            return newStruct;
        }
        public WalletStruct addTransection(WalletStruct originalStruct, TransectionStruct theTransection)
        {
            WalletStruct newStruct = originalStruct;
            newStruct.transection.Add(theTransection);
            return newStruct;
        }
        public WalletStruct editTransection(WalletStruct originalStruct, TransectionStruct theTransection)
        {
            WalletStruct newStruct = originalStruct;
            for (int i = 0; i < originalStruct.transection.Count; i++)
            {
                if (originalStruct.transection[i].uuid == theTransection.uuid)
                {
                    newStruct.transection[i] = theTransection;
                }
            }
            return newStruct;
        }
        public List<TransectionStruct> getAllTransection(List<WalletStruct> wallets)
        {
            List<TransectionStruct> allTransection = new List<TransectionStruct>();
            foreach(WalletStruct eachEle in wallets)
            {
                foreach(TransectionStruct eachTran in eachEle.transection)
                {
                    allTransection.Add(eachTran);
                }
            }
            return allTransection;
        }
    }
}