using Base;

namespace System.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class SystemNotifyTypes
    {
        public static DictionaryItem LoginSucceeded => EntityDictionary.Item(40701, "Login succeeded");
        public static DictionaryItem LoginFailed => EntityDictionary.Item(40702, "Login failed");
        public static DictionaryItem InvalidPassword => EntityDictionary.Item(40703, "Invalid Password");
        public static DictionaryItem LockedOut => EntityDictionary.Item(40704, "Account locked");
        public static DictionaryItem TwoFactorFailed => EntityDictionary.Item(40705, "Two factor failed");
        public static DictionaryItem RegisterSucceeded => EntityDictionary.Item(40706, "Register succeeded");
        public static DictionaryItem RegisterFailed => EntityDictionary.Item(40707, "Register failed");
        public static DictionaryItem PasswordTooShort => EntityDictionary.Item(40708, "Password too short");
        public static DictionaryItem PasswordRequiresNonAlphanumeric => EntityDictionary.Item(40709, "Password requires non alphanumeric");
        public static DictionaryItem PasswordRequiresDigit => EntityDictionary.Item(40710, "Password requires digit");
        public static DictionaryItem PasswordRequiresLower => EntityDictionary.Item(40711, "Password requires lowercase");
        public static DictionaryItem PasswordRequiresUpper => EntityDictionary.Item(40712, "Password requires upercase");
        public static DictionaryItem ProcessCancelled => EntityDictionary.Item(40713, "Process was cancelled");
    }
}