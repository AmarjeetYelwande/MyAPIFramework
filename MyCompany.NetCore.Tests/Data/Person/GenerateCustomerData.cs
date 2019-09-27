using Bogus;
using System;
using System.Collections.Generic;

namespace MyCompany.NetCore.Tests.Person
{
    public class GenerateCustomerData : IPerson
    {
        private readonly Faker faker;
        private GenerateCustomerData()
        {
            faker = new Faker("en_GB");
        }
        public string Title
        {
            get
            {
                return Title;
            }

            set
            {
                faker.Name.Prefix();
            }
        }
        public string FirstName
        {
            get
            {
                return FirstName;
            }

            set
            {
                faker.Name.FirstName();
            }
        }
        public string MiddleName
        {
            get
            {
                return MiddleName;
            }

            set
            {
                faker.Name.FirstName();
            }
        }
        public string LastName
        {
            get
            {
                return LastName;
            }

            set
            {
                faker.Name.LastName();
            }
        }
        public List<string> Address
        {
            get
            {
                return Address;
            }

            set
            {
                faker.Address.FullAddress();
            }
        }
        public string TelephoneNumber
        {
            get
            {
                return TelephoneNumber;
            }

            set
            {
                faker.Phone.PhoneNumber();
            }
        }
        public DateTime DateOfBirth
        {
            get
            {
                return DateOfBirth;
            }

            set
            {
                faker.Date.Between(DateTime.Now.AddYears(-90), DateTime.Now.AddYears(-20));
            }
        }
        public int BankAccountNumber
        {
            get
            {
                return BankAccountNumber;
            }

            set
            {
                faker.Finance.Account();
            }
        }
        public int SortCode
        {
            get
            {
                return SortCode;
            }

            set
            {
                faker.Finance.Random.Int(100000, 999999);
            }
        }
        public string PaymentCardType
        {
            get
            {
                return PaymentCardType;
            }

            set
            {
                faker.Finance.Random.Word();
            }
        }
        public int PaymentCardNumber
        {
            get
            {
                return PaymentCardNumber;
            }

            set
            {
                faker.Finance.CreditCardNumber();
            }
        }
        public DateTime PaymentCardExpiryDate
        {
            get
            {
                return PaymentCardExpiryDate;
            }

            set
            {
                DateTime.Now.AddYears(+2);
            }
        }
        public int PaymentCardCvvPin
        {
            get
            {
                return PaymentCardCvvPin;
            }

            set
            {
                faker.Finance.CreditCardCvv();
            }
        }
        public int NoClaimsYears
        {
            get
            {
                return NoClaimsYears;
            }

            set
            {
                faker.Vehicle.Random.Int(100, 999);
            }
        }
        public int NumberOfDrivers
        {
            get
            {
                return NumberOfDrivers;
            }

            set
            {
                faker.Vehicle.Random.Int(1, 5);
            }
        }
    }

}
