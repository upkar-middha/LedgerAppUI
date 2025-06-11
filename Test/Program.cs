using LedgerAppUI.Services;
using System;
using System.Security.Authentication;
internal class Test
{
    public static void Main(string[] args)
    {
        string password = "upkarmiddha";
        string res = Authentication.Hash_using_SHA(password);
        Console.WriteLine(res);
    }
}
