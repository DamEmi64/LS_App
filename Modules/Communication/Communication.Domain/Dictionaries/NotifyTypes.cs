using Base;

namespace Communication.Domain.Dictionaries
{
    [Dictionary("Notify types")]
    public class NotifyTypes
    {
        public static DictionaryItem EmailSend => EntityDictionary.Item(3036, "Send Email Successed");
        public static DictionaryItem EmailGenerated => EntityDictionary.Item(3037, "Generation Email Successed");
    }
}
