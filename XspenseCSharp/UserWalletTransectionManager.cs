using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

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
                // Create Default Currency
                CurrencyStruct defaultCurrency = new CurrencyStruct();
                defaultCurrency.uuid = Guid.NewGuid().ToString();
                defaultCurrency.full_name = "US Dollar";
                defaultCurrency.code_name = "USD";
                defaultCurrency.exchange_rate = 1;

                // Create Default Wallet
                WalletStruct defaultWallet = new WalletStruct();
                defaultWallet.uuid = Guid.NewGuid().ToString();
                defaultWallet.name = "Default Wallet";
                defaultWallet.currency_id = defaultCurrency.uuid;
                defaultWallet.transection = new List<TransectionStruct>();

                // Create Default Category
                CategoryStruct defaultCategory = new CategoryStruct();
                defaultCategory.uuid = Guid.NewGuid().ToString();
                defaultCategory.name = "Food";
                defaultCategory.description = "Meal such as Breakfast, Lunch and Dinner.";

                UserGeneralInfoStruct userGeneralInfo = new UserGeneralInfoStruct();
                userGeneralInfo.wallet = new List<WalletStruct>() { defaultWallet };
                userGeneralInfo.currency = new List<CurrencyStruct>() { defaultCurrency };
                userGeneralInfo.category = new List<CategoryStruct>() { defaultCategory };
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
        public List<TransectionPresentStruct> transectionToPresent(UserGeneralInfoStruct userGeneral, List<TransectionStruct> transectionList)
        {
            List<TransectionPresentStruct> presentStructList = new List<TransectionPresentStruct>();
            foreach (TransectionStruct eachTransection in transectionList)
            {
                TransectionPresentStruct tempPresentStruct = new TransectionPresentStruct();
                tempPresentStruct.uuid = eachTransection.uuid;
                tempPresentStruct.wallet_id = eachTransection.wallet_id;
                tempPresentStruct.category_id = eachTransection.category_id;
                tempPresentStruct.Type = eachTransection.type;
                tempPresentStruct.Price = eachTransection.price;

                // find wallet
                foreach (WalletStruct eachWallet in userGeneral.wallet)
                {
                    if (eachWallet.uuid == eachTransection.wallet_id)
                    {
                        tempPresentStruct.Wallet = eachWallet.name;
                        // find currency
                        foreach (CurrencyStruct eachCurrency in userGeneral.currency)
                        {
                            if (eachCurrency.uuid == eachWallet.currency_id)
                            {
                                tempPresentStruct.Currency = eachCurrency.code_name;
                            }
                        }
                    }
                }
                // find category
                foreach (CategoryStruct eachCategory in userGeneral.category)
                {
                    if (eachCategory.uuid == eachTransection.category_id)
                    {
                        tempPresentStruct.Category = eachCategory.name;
                    }
                }
                tempPresentStruct.Date = eachTransection.date;
                presentStructList.Add(tempPresentStruct);
            }
            return presentStructList;
        }


        // totally ------------------------
        public float getTotalIncome()
        {
            UserGeneralInfoStruct originalStruct = UserWalletTransectionManager.shared.loadStructFromFile(NativeFileManager.shared.GetUserToken());
            float totalIncome = 0;
            for (int i = 0; i < originalStruct.wallet.Count; i++)
            {
                foreach (TransectionStruct eachTransection in originalStruct.wallet[i].transection)
                {
                    if (eachTransection.type == TransectionTypeEnum.Income)
                    {
                        foreach (CurrencyStruct eachCurrency in originalStruct.currency)
                        {
                            if (eachCurrency.uuid == originalStruct.wallet[i].currency_id) {
                                totalIncome += eachTransection.price / eachCurrency.exchange_rate;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            return totalIncome;
        }
        public float getTotalExpense()
        {
            UserGeneralInfoStruct originalStruct = UserWalletTransectionManager.shared.loadStructFromFile(NativeFileManager.shared.GetUserToken());
            float totalExpense = 0;
            for (int i = 0; i < originalStruct.wallet.Count; i++)
            {
                foreach (TransectionStruct eachTransection in originalStruct.wallet[i].transection)
                {
                    if (eachTransection.type == TransectionTypeEnum.Expense)
                    {
                        foreach (CurrencyStruct eachCurrency in originalStruct.currency)
                        {
                            if (eachCurrency.uuid == originalStruct.wallet[i].currency_id)
                            {
                                totalExpense += eachTransection.price / eachCurrency.exchange_rate;
                                break;
                            }
                        }
                    }
                }
            }
            return totalExpense;
        }
        // wallet -------------------------
        public List<WalletPresentStruct> WalletToWalletPresent(UserGeneralInfoStruct originalStruct)
        {
            List<WalletPresentStruct> tempWalletList = new List<WalletPresentStruct>();
            foreach (WalletStruct eachWallet in originalStruct.wallet)
            {
                WalletPresentStruct newWalletPresent = new WalletPresentStruct();
                newWalletPresent.uuid = eachWallet.uuid;
                newWalletPresent.Name = eachWallet.name;
                newWalletPresent.currency_id = eachWallet.currency_id;
                foreach (CurrencyStruct eachCurrency in originalStruct.currency)
                {
                    if (eachCurrency.uuid == eachWallet.currency_id)
                    {
                        newWalletPresent.Currency = eachCurrency.code_name;
                        newWalletPresent.Exchange = eachCurrency.exchange_rate.ToString();
                        break;
                    }
                }
                tempWalletList.Add(newWalletPresent);
            }
            return tempWalletList;
        }
        public void deleteWallet(UserGeneralInfoStruct originalStruct, string walletId)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            foreach (WalletStruct eachWallet in originalStruct.wallet)
            {
                if (eachWallet.uuid == walletId)
                {
                    newStruct.wallet.Remove(eachWallet);
                    saveStructToFile(newStruct, fileManagerNative.GetUserToken());
                    return;
                }
            }
        }
        public void addWallet(UserGeneralInfoStruct originalStruct, WalletStruct theWallet)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            newStruct.wallet.Add(theWallet);
            saveStructToFile(newStruct, fileManagerNative.GetUserToken());
        }
        public void editWallet(UserGeneralInfoStruct originalStruct, WalletStruct theWallet)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            for (int i = 0; i < originalStruct.wallet.Count; i++)
            {
                if (originalStruct.wallet[i].uuid == theWallet.uuid)
                {
                    newStruct.wallet[i] = theWallet;
                    break;
                }
            }
            saveStructToFile(newStruct, fileManagerNative.GetUserToken());
        }
        // transection -------------------------
        public void deleteTransection(UserGeneralInfoStruct originalStruct, TransectionPresentStruct theTransection)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            TransectionStruct originalTransection = new TransectionStruct();

            // find original transection 
            for (int i = 0; i < originalStruct.wallet.Count; i++)
            {
                foreach (TransectionStruct eachTransection in originalStruct.wallet[i].transection)
                {
                    if (eachTransection.uuid == theTransection.uuid)
                    {
                        originalTransection = eachTransection;
                        break;
                    }
                }
            }

            // remove transection from the chain
            for (int i = 0; i < originalStruct.wallet.Count; i++)
            {
                if (originalStruct.wallet[i].uuid == originalTransection.wallet_id)
                {
                    newStruct.wallet[i].transection.Remove(originalTransection);
                    break;
                }
            }
            saveStructToFile(newStruct, fileManagerNative.GetUserToken());
        }
        public void addTransection(UserGeneralInfoStruct originalStruct, TransectionStruct theTransection)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            for (int i = 0; i < originalStruct.wallet.Count; i++)
            {
                if (originalStruct.wallet[i].uuid == theTransection.wallet_id)
                {
                    newStruct.wallet[i].transection.Add(theTransection);
                    break;
                }
            }
            saveStructToFile(newStruct, fileManagerNative.GetUserToken());
        }
        public void editTransection(UserGeneralInfoStruct originalStruct, TransectionPresentStruct theTransection)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            TransectionStruct originalTransection = new TransectionStruct();
            int walletIndex = 0;

            // find original transection 
            for (int i = 0; i < originalStruct.wallet.Count; i++)
            {
                foreach (TransectionStruct eachTransection in originalStruct.wallet[i].transection)
                {
                    if (eachTransection.uuid == theTransection.uuid)
                    {
                        walletIndex = i;
                        originalTransection = eachTransection;
                        break;
                    }
                }
            }

            // changing original data
            originalTransection.type = theTransection.Type;
            originalTransection.uuid = theTransection.uuid;
            originalTransection.date = theTransection.Date;
            originalTransection.price = theTransection.Price;
            originalTransection.category_id = theTransection.category_id;
            originalTransection.wallet_id = theTransection.wallet_id;

            // update transection in the chain
            for (int i = 0; i < originalStruct.wallet[walletIndex].transection.Count; i++)
            {
                if (originalStruct.wallet[walletIndex].transection[i].uuid == originalTransection.uuid)
                {
                    newStruct.wallet[walletIndex].transection[i] = originalTransection;
                    break;
                }
            }
            saveStructToFile(newStruct, fileManagerNative.GetUserToken());
        }
        public List<TransectionStruct> getTransectionFilter(UserGeneralInfoStruct userGeneral, PickDateEnum filterType)
        {
            List<TransectionStruct> allTransection = new List<TransectionStruct>();
            foreach (WalletStruct eachEle in userGeneral.wallet)
            {
                foreach (TransectionStruct eachTran in eachEle.transection)
                {
                    switch (filterType)
                    {
                        case PickDateEnum.today:
                            if (eachTran.date.Date == DateTime.Now.Date)
                                allTransection.Add(eachTran);
                            break;
                        case PickDateEnum.yesterday:
                            if (eachTran.date.Date == DateTime.Today.AddDays(-1).Date)
                                allTransection.Add(eachTran);
                            break;
                        case PickDateEnum.this_week:
                            if (eachTran.date.Date >= DateTime.Today.AddDays(-7).Date)
                                allTransection.Add(eachTran);
                            break;
                        case PickDateEnum.this_month:
                            if (eachTran.date.Date >= DateTime.Today.AddDays(-30).Date)
                                allTransection.Add(eachTran);
                            break;
                        case PickDateEnum.this_year:
                            if (eachTran.date.Date >= DateTime.Today.AddDays(-360).Date)
                                allTransection.Add(eachTran);
                            break;
                        default:
                            break;
                    }
                }
            }
            return allTransection;
        }
        // Category -----------------------------------------------------------
        public void deleteCategory(UserGeneralInfoStruct originalStruct, CategoryStruct theCategory)
        {
            foreach(WalletStruct eachWallet in originalStruct.wallet)
            {
                foreach(TransectionStruct eachTransection in eachWallet.transection)
                {
                    if(eachTransection.category_id == theCategory.uuid)
                    {
                        MessageBox.Show("You can only delete category that isn't using.");
                        return;
                    }
                }
            }

            UserGeneralInfoStruct newStruct = originalStruct;
            newStruct.category.Remove(theCategory);
            saveStructToFile(newStruct, fileManagerNative.GetUserToken());
        }
        public void addCategory(UserGeneralInfoStruct originalStruct, CategoryStruct theCategory)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            newStruct.category.Add(theCategory);
            saveStructToFile(newStruct, fileManagerNative.GetUserToken());
        }
        public void editCategory(UserGeneralInfoStruct originalStruct, CategoryStruct theCategory)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            for (int i = 0; i < originalStruct.category.Count; i++)
            {
                if (originalStruct.category[i].uuid == theCategory.uuid)
                {
                    newStruct.category[i] = theCategory;
                    break;
                }
            }
            saveStructToFile(newStruct, fileManagerNative.GetUserToken());
        }

        // Currency -----------------------------------------------------------
        public void deleteCurrency(UserGeneralInfoStruct originalStruct, CurrencyStruct theCurrency)
        {
            foreach (WalletStruct eachWallet in originalStruct.wallet)
            {
                if (eachWallet.currency_id == theCurrency.uuid)
                {
                    MessageBox.Show("You can only delete currency that isn't using.");
                    return;
                }
            }

            UserGeneralInfoStruct newStruct = originalStruct;
            newStruct.currency.Remove(theCurrency);
            saveStructToFile(newStruct, fileManagerNative.GetUserToken());
        }
        public void addCurrency(UserGeneralInfoStruct originalStruct, CurrencyStruct theCurrency)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            newStruct.currency.Add(theCurrency);
            saveStructToFile(newStruct, fileManagerNative.GetUserToken());
        }
        public void editCurrency(UserGeneralInfoStruct originalStruct, CurrencyStruct theCurrency)
        {
            UserGeneralInfoStruct newStruct = originalStruct;
            for (int i = 0; i < originalStruct.currency.Count; i++)
            {
                if (originalStruct.currency[i].uuid == theCurrency.uuid)
                {
                    newStruct.currency[i] = theCurrency;
                    break;
                }
            }
            saveStructToFile(newStruct, fileManagerNative.GetUserToken());
        }
    }
}