using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class NotifyTypes
    {
        public static DictionaryItem EmailSend => EntityDictionary.Item(40301, "Send Email Successed");
        public static DictionaryItem EmailGenerated => EntityDictionary.Item(40302, "Generation Email Successed");
    }
}
