using Base.Entities;

namespace System.Domain.Dictionaries
{
    [Dictionary("NotifyTypes")]
    public class SystemNotifyTypes
    {
        public static DictionaryItem LoginSucceeded => EntityDictionary.Item(1030, "Login succeeded");
        public static DictionaryItem LoginFailed => EntityDictionary.Item(1031, "Login failed");
        public static DictionaryItem InvalidPassword => EntityDictionary.Item(1032, "Invalid Password");
        public static DictionaryItem LockedOut => EntityDictionary.Item(1033, "Account locked");
        public static DictionaryItem TwoFactorFailed => EntityDictionary.Item(1034, "Two factor failed");
        public static DictionaryItem RegisterSucceeded => EntityDictionary.Item(1035, "Register succeeded");
        public static DictionaryItem RegisterFailed => EntityDictionary.Item(1036, "Register failed");
    }
}
