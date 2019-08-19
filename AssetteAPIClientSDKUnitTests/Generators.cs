using Assette.Client;
using AutoFixture;
using AutoFixture.Kernel;
using System.Net.Mail;
using System.Reflection;

namespace Portal.SDK.Test
{
    public class Generators
    {
        public UserToCreateDto GenerateNewUser()
        {
            var fixture = new Fixture();
            
            fixture.Customize<UserToCreateDto>(c => c.With(x => x.Email, fixture.Create<MailAddress>().Address)
                                                    .With(x => x.UserCode,$"IND{fixture.Create<int>().ToString("D6")}"));

            //fixture.Customize<TranslatableName>(c => c.With(x => x.CultureCode,()=> { return Rand});

            var user = fixture.Create<UserToCreateDto>();
            return user;
        }

        public DocumentDto GenerateNewDocument()
        {
            var fixture = new Fixture();

            return fixture.Create<DocumentDto>();
        }

        public AccountToCreateDto GenerateNewAccount()
        {
            try
            {
                var fixture = new Fixture();

                var nameFr = fixture.Create<TranslatableName>();
                nameFr.CultureCode = "Fr-CA";
                var nameEn = fixture.Create<TranslatableName>();
                nameEn.CultureCode = "En-US";
                var names= new TranslatableName[] { nameEn, nameFr };

               fixture.Customize<AccountToCreateDto>(
                   c=>c.With(x => x.TranslatableNames, names)
                    .With(x => x.Code, $"ACT{fixture.Create<int>().ToString("D4")}"));
                var account = fixture.Create<AccountToCreateDto>();

                return account;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }


        public class CultureCodeBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                var propInfo = request as PropertyInfo;
                if (propInfo == null || propInfo.Name!="CultureCode" || propInfo.PropertyType!=typeof(string))
                {
                    return new NoSpecimen();
                }
                var sn = context.Resolve(typeof(CultureCode));
                return (string)sn;
            }
        }

        public static class CultureCode
        {

        }
        //public static DocumentDto GenerateNewDocument()
        //{
        //    var fixture = new Fixture();

        //    fixture.Customize<DocumentDto>(c => c.With(x => x.Email, fixture.Create<MailAddress>().Address)
        //                                            .With(x => x.userCode, $"IND{fixture.Create<int>().ToString("D6")}"));

        //    return fixture.Create<DocumentDto>();
        //}
    }
}
