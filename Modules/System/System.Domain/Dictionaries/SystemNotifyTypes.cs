using Base;

namespace System.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class SystemNotifyTypes
    {
        public static DictionaryItem LoginSucceeded => EntityDictionary.Item(3029, "Login succeeded");
        public static DictionaryItem LoginFailed => EntityDictionary.Item(3030, "Login failed");
        public static DictionaryItem InvalidPassword => EntityDictionary.Item(3031, "Invalid Password");
        public static DictionaryItem LockedOut => EntityDictionary.Item(3032, "Account locked");
        public static DictionaryItem TwoFactorFailed => EntityDictionary.Item(3033, "Two factor failed");
        public static DictionaryItem RegisterSucceeded => EntityDictionary.Item(3034, "Register succeeded");
        public static DictionaryItem RegisterFailed => EntityDictionary.Item(3035, "Register failed");
        public static DictionaryItem PasswordTooShort => EntityDictionary.Item(3038, "Password too short");
        public static DictionaryItem PasswordRequiresNonAlphanumeric => EntityDictionary.Item(3039, "Password requires non alphanumeric");
        public static DictionaryItem PasswordRequiresDigit => EntityDictionary.Item(3040, "Password requires digit");
        public static DictionaryItem PasswordRequiresLower => EntityDictionary.Item(3041, "Password requires lowercase");
        public static DictionaryItem PasswordRequiresUpper => EntityDictionary.Item(3042, "Password requires upercase");
        public static DictionaryItem ProcessCancelled => EntityDictionary.Item(3056, "Process was cancelled");
    }
}