using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Assette.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

public static class Constants
    {
        //Account Codes for Test
        public static readonly string TestAccountCode1 = "TEST11";
        public static readonly string TestAccountCode2 = "TEST22";
        public static readonly string TestAccountCode3 = "TEST33";
        public static readonly string TestAccountCode4 = "TEST44";
        public static readonly string TestAccountCode5 = "TEST55";
        public static readonly string TestAccountCode6 = "TEST66";

        public static readonly string TestAccountCode7 = "TEST77";
        public static readonly string TestAccountCode8 = "TEST88";
        public static readonly string TestAccountCode9 = "TEST99";
        public static readonly string TestAccountCode10 = "TEST1010";
        public static readonly string TestAccountCode11 = "TEST1111";
        public static readonly string TestAccountCode12 = "TEST1212";
        public static readonly string TestAccountCode13 = "TEST1313";

        //Account translation names for test
        public static readonly string TestAccountCode1_Name = "TEST11_TestName";
        public static readonly string TestAccountCode2_Name = "TEST22_TestName";
        public static readonly string TestAccountCode3_Name = "TEST33_TestName";
        public static readonly string TestAccountCode4_Name = "TEST44_TestName";
        public static readonly string TestAccountCode5_Name = "TEST55_TestName";
        public static readonly string TestAccountCode6_Name = "TEST66_TestName";

        public static readonly string TestAccountCode7_Name = "TEST77_TestName";
        public static readonly string TestAccountCode8_Name = "TEST88_TestName";
        public static readonly string TestAccountCode9_Name = "TEST99_TestName";
        public static readonly string TestAccountCode10_Name = "TEST1010_TestName";
        public static readonly string TestAccountCode11_Name = "TEST1111_TestName";
        public static readonly string TestAccountCode12_Name = "TEST1212_TestName";
        public static readonly string TestAccountCode13_Name = "TEST1313_TestName";

        public static readonly string CultureCodeEnglish = "en-US";
    }

namespace AssetteAPIClientSDKUnitTests
{
    [TestClass]
    //This Test class will provide only the test flow.
    //TODO Assette: Depricated please use a file per method to test
    public class DepricatedAssetteApiClientTests
    {
        AssetteApiClient _assetteApiClient;

        [TestInitialize]
        public void Init()
        {
            AssetteApiSettings _assetteApiSettings = new AssetteApiSettings
            {
                AuthUrl = @"https://app.stg.assette.com/services/id/auth/token",
                BaseAddress = @"https://app.stg.assette.com/app/admin",
                Credentials = new AssetteApiCredentials
                {
                    ClientId = "ADDN",
                    ClientSecret = "3D5AC9A019200A5A4D0F0D8A38C8CAB0A8BFF5DC894FBCE0EE18B9B138937A3BF1A9AEAEC8E779A8384990B4D5DD2C7FA5F89D0E5FBB9DFDA1311BDA73A62EE3",
                    UserName = @"ADDN\assetteuser",
                    Password = "assette2client"
                }
            };

            _assetteApiClient = new AssetteApiClient(_assetteApiSettings);

        }

        #region CreateOrUpdateAccounts

        [TestMethod]
        [TestCategory("CreateOrUpdateAccountsAsync")]
        public async Task WhenValidAccounts_ShouhldReturnArrayOfSuccessResults()
        {
            IEnumerable<AccountToCreateDto> accountToCreateDto = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode1,
                    IsActive = true,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode1_Name
                        }
                    }
                }
            };

            //API Response
            var result = await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToCreateDto);

            //Get Records from DB
            var AccountInDb = await _assetteApiClient.GetAccountAsync(Constants.TestAccountCode1);

            //Check Response records
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(Constants.TestAccountCode1, result.Data.FirstOrDefault().Data.AccountCode);

            //Check with Database records
            Assert.AreEqual(Constants.TestAccountCode1, AccountInDb.Data.Code);
            Assert.AreEqual(Constants.TestAccountCode1_Name,
                AccountInDb.Data.TranslatableNames.Where(x => x.CultureCode == Constants.CultureCodeEnglish).FirstOrDefault().Name);
        }

        [TestMethod]
        public async Task CreateBulkOfAccountsTest()
        {
            IEnumerable<AccountToCreateDto> accountToCreateDtos = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode2,
                    IsActive = true,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode2_Name
                        }
                    }
                },
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode3,
                    IsActive = true,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode3_Name
                        }
                    }
                }
            };

            //API Response
            var result = await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToCreateDtos);

            //Get Records from DB
            var accountInDbFirst = await _assetteApiClient.GetAccountAsync(Constants.TestAccountCode2);
            var accountInDbSecond = await _assetteApiClient.GetAccountAsync(Constants.TestAccountCode3);

            //Check Response records
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(1, result.Data.Where(x => x.Data.AccountCode == Constants.TestAccountCode2).Count());
            Assert.AreEqual(1, result.Data.Where(x => x.Data.AccountCode == Constants.TestAccountCode3).Count());

            //Check with Database records
            Assert.AreEqual(Constants.TestAccountCode2, accountInDbFirst.Data.Code);
            Assert.AreEqual(Constants.TestAccountCode2_Name,
                accountInDbFirst.Data.TranslatableNames.Where(x => x.CultureCode == Constants.CultureCodeEnglish).FirstOrDefault().Name);

            Assert.AreEqual(Constants.TestAccountCode3, accountInDbSecond.Data.Code);
            Assert.AreEqual(Constants.TestAccountCode3_Name,
                accountInDbSecond.Data.TranslatableNames.Where(x => x.CultureCode == Constants.CultureCodeEnglish).FirstOrDefault().Name);
        }

        [TestMethod]
        public async Task UpdateAnAccountTest()
        {
            //Create an Account
            IEnumerable<AccountToCreateDto> accountToCreateDtoFirst = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode4,
                    IsActive = true,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode4_Name
                        }
                    }
                }
            };

            var resultFirst = await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToCreateDtoFirst);

            //Create an Account with same Account Code
            IEnumerable<AccountToCreateDto> accountToCreateDtoSecond = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode4,
                    IsActive = true,
                    OpenDate = Convert.ToDateTime("2019-01-01"),
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = "Modified Name"
                        }
                    }
                }
            };

            var resultUpdatedAccount = await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToCreateDtoSecond);

            //Get updated account from DB
            var updatedAccountInDb = await _assetteApiClient.GetAccountAsync(Constants.TestAccountCode4);

            //Check Response records
            Assert.AreEqual(true, resultUpdatedAccount.Success);
            Assert.AreEqual(Constants.TestAccountCode4, resultUpdatedAccount.Data.FirstOrDefault().Data.AccountCode);

            //Check with Database records
            Assert.AreEqual(Constants.TestAccountCode4, updatedAccountInDb.Data.Code);
            Assert.AreEqual(Convert.ToDateTime("2019-01-01"), updatedAccountInDb.Data.OpenDate);
            Assert.AreEqual("Modified Name",
                updatedAccountInDb.Data.TranslatableNames.Where(x => x.CultureCode == Constants.CultureCodeEnglish).FirstOrDefault().Name);

        }

        [TestMethod]
        public async Task UpdateMoreAccountTest()
        {
            //Create Accounts
            IEnumerable<AccountToCreateDto> accountToCreateDtos = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode5,
                    IsActive = true,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode5_Name
                        }
                    }
                },
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode6,
                    IsActive = true,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode6_Name
                        }
                    }
                }
            };

            await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToCreateDtos);

            //Update created Accounts
            IEnumerable<AccountToCreateDto> accountToUpdates = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode5,
                    IsActive = true,
                    OpenDate = Convert.ToDateTime("2019-01-01"),
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = "Modified Name1"
                        }
                    }
                },
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode6,
                    IsActive = true,
                    OpenDate = Convert.ToDateTime("2019-05-05"),
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = "Modified Name2"
                        }
                    }
                }
            };

            //API Response
            var updatedAccountsResult = await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToUpdates);

            //Get Records from DB
            var updatedAccountInDbFirst = await _assetteApiClient.GetAccountAsync(Constants.TestAccountCode5);
            var updatedAccountInDbSecond = await _assetteApiClient.GetAccountAsync(Constants.TestAccountCode6);


            //Check Response records
            Assert.AreEqual(true, updatedAccountsResult.Success);
            Assert.AreEqual(Constants.TestAccountCode5, updatedAccountsResult.Data.FirstOrDefault().Data.AccountCode);
            Assert.AreEqual(Constants.TestAccountCode6, updatedAccountsResult.Data.Skip(1).First().Data.AccountCode);

            //Check with Database records
            Assert.AreEqual(Constants.TestAccountCode5, updatedAccountInDbFirst.Data.Code);
            Assert.AreEqual(Convert.ToDateTime("2019-01-01"), updatedAccountInDbFirst.Data.OpenDate);
            Assert.AreEqual("Modified Name1",
                updatedAccountInDbFirst.Data.TranslatableNames.Where(x => x.CultureCode == Constants.CultureCodeEnglish).FirstOrDefault().Name);

            Assert.AreEqual(Constants.TestAccountCode6, updatedAccountInDbSecond.Data.Code);
            Assert.AreEqual(Convert.ToDateTime("2019-05-05"), updatedAccountInDbSecond.Data.OpenDate);
            Assert.AreEqual("Modified Name2",
                updatedAccountInDbSecond.Data.TranslatableNames.Where(x => x.CultureCode == Constants.CultureCodeEnglish).FirstOrDefault().Name);

        }

        #endregion

        #region GetAccount

        [TestMethod]
        public async Task GetAccountForExistingAccountCodeTest()
        {
            //Create an Account
            IEnumerable<AccountToCreateDto> accountToCreateDto = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode7,
                    IsActive = true,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode7_Name
                        }
                    }
                }
            };

            await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToCreateDto);

            //Get an account using account code
            //API Response
            var result = await _assetteApiClient.GetAccountAsync(Constants.TestAccountCode7);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(Constants.TestAccountCode7, result.Data.Code);
            Assert.AreEqual(true, result.Data.IsActive);
            Assert.AreEqual(Constants.TestAccountCode7_Name,
                    result.Data.TranslatableNames.Where(x => x.CultureCode == Constants.CultureCodeEnglish).FirstOrDefault().Name);

        }

        [TestMethod]
        public async Task GetAccountForNotExistAccountCodeTest()
        {
            try
            {
                var accountDtoResult = await _assetteApiClient.GetAccountAsync("ABCDE334swerrtttTesteetttt");
            }
            catch (ApiException ex)
            {
                Assert.AreEqual((int)HttpStatusCode.NotFound, ex.StatusCode);
            }
        }

        #endregion

        #region Delete Account

        [TestMethod]
        public async Task DeleteAccountForExistingAccountCodeTest()
        {
            //Create an Account
            IEnumerable<AccountToCreateDto> accountToCreateDto = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode8,
                    IsActive = true,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode8_Name
                        }
                    }
                }
            };

            await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToCreateDto);

            //Delete created account
            //API Response
            await _assetteApiClient.RemoveAccountAsync(Constants.TestAccountCode8);

            //Get Records from DB
            var AccountInDb = await _assetteApiClient.GetAccountAsync(Constants.TestAccountCode8);

            //Check with Database records
            Assert.AreEqual(Constants.TestAccountCode8, AccountInDb.Data.Code);
            Assert.AreEqual(false, AccountInDb.Data.IsActive);


        }

        [TestMethod]
        public async Task DeleteAccountForNotExistingAccountCode()
        {
            try
            {
                await _assetteApiClient.RemoveAccountAsync("ABC111eww3344222121111");
            }
            catch (ApiException ex)
            {
                Assert.AreEqual((int)HttpStatusCode.NotFound, ex.StatusCode);
            }

        }

        #endregion

        #region Search Account

        [TestMethod]
        public async Task SearchAccountWithAccountCode()
        {
            //Create an Account
            IEnumerable<AccountToCreateDto> accountToCreateDto = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode9,
                    IsActive = true,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode9_Name
                        }
                    }
                }
            };

            await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToCreateDto);
            //await _assetteApiClient.RemoveDocumentTypeAsync();

            //Create Serch filter
            SieveModel sieveModel = new SieveModel
            {
                Filters = "Code@=" + Constants.TestAccountCode9,
                Sorts = "Code",
                Page = 1,
                PageSize = 10

            };

            //API Response
            var result = await _assetteApiClient.SearchAccountsAsync(sieveModel);

            //Check Response records
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(1, result.Data.Count());
            Assert.AreEqual(Constants.TestAccountCode9, result.Data.FirstOrDefault().Code);

        }

        [TestMethod]
        public async Task SearchAccountWithListOfAccountCodes()
        {
            IEnumerable<AccountToCreateDto> accountToCreateDtos = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode10,
                    IsActive = true,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode10_Name
                        }
                    }
                },
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode11,
                    IsActive = true,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode11_Name
                        }
                    }
                }
            };


            await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToCreateDtos);

            //Create Search filter
            SieveModel sieveModel = new SieveModel
            {
                Filters = "Code==" + Constants.TestAccountCode10 + "|" + Constants.TestAccountCode11,
                Sorts = "Code",
                Page = 1,
                PageSize = 10

            };

            //API Response
            var result = await _assetteApiClient.SearchAccountsAsync(sieveModel);

            //Check Response records
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(2, result.Data.Count());
            Assert.AreEqual(1, result.Data.Where(x => x.Code == Constants.TestAccountCode10).Count());
            Assert.AreEqual(1, result.Data.Where(x => x.Code == Constants.TestAccountCode11).Count());
            Assert.AreEqual(Constants.TestAccountCode10, result.Data.FirstOrDefault().Code);

        }

        [TestMethod]
        public async Task SearchAccountWithOpenDate()
        {
            //Create an Account
            IEnumerable<AccountToCreateDto> accountToCreateDto = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode12,
                    IsActive = true,
                    OpenDate = Convert.ToDateTime("2000-01-01"),
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode12_Name
                        }
                    }
                }
            };

            await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToCreateDto);

            //Create Serch filter
            SieveModel sieveModel = new SieveModel
            {
                Filters = "OpenDate<" + Convert.ToDateTime("2000-01-02"),
                Sorts = "Code",
                Page = 1,
                PageSize = 10

            };

            //API Response
            var result = await _assetteApiClient.SearchAccountsAsync(sieveModel);

            //Check Response records
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(Convert.ToDateTime("2000-01-01"), result.Data.Where(x=> x.Code == Constants.TestAccountCode12).FirstOrDefault().OpenDate);

        }

        [TestMethod]
        public async Task SearchAccountWithIsActive()
        {
            //Create an Account
            IEnumerable<AccountToCreateDto> accountToCreateDto = new List<AccountToCreateDto>
            {
                new AccountToCreateDto
                {
                    Code = Constants.TestAccountCode13,
                    IsActive = false,
                    OpenDate = DateTime.Now,
                    TranslatableNames = new List <TranslatableName>
                    {
                        new TranslatableName
                        {
                            CultureCode = Constants.CultureCodeEnglish,
                            Name = Constants.TestAccountCode13_Name
                        }
                    }
                }
            };

            await _assetteApiClient.CreateOrUpdateAccountsAsync(accountToCreateDto);

            //Create Serch filter
            SieveModel sieveModel = new SieveModel
            {
                Filters = "IsActive==" + false,
                Sorts = "Code",
                Page = 1,
                PageSize = 10

            };

            //API Response
            var result = await _assetteApiClient.SearchAccountsAsync(sieveModel);

            //Check Response records
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(false, result.Data.Where(x => x.Code == Constants.TestAccountCode13).FirstOrDefault().IsActive);

        }
        #endregion

    }
}
