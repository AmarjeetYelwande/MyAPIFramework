using System;
using System.Collections.Generic;

namespace MyCompany.NetCore.Tests.Person
{
    interface IPerson
    {
        string Title { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        List<string> Address { get; set; }
        string TelephoneNumber { get; set; }
        DateTime DateOfBirth { get; set; }
        int BankAccountNumber { get; set; }
        int SortCode { get; set; }
        int PaymentCardNumber { get; set; }
        string PaymentCardType { get; set; }
        DateTime PaymentCardExpiryDate { get; set; }
        int PaymentCardCvvPin { get; set; }
        int NoClaimsYears { get; set; }
        int NumberOfDrivers { get; set; }
    }
}
