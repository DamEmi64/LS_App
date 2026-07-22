namespace Communication.Infrastructure
{
    public class Constants
    {
        // Discord Interaction types
        public const int TYPE_PING = 1;
        public const int TYPE_APPLICATION_COMMAND = 2;
        public const int TYPE_MESSAGE_COMPONENT = 3;
        public const int TYPE_APPLICATION_COMMAND_AUTOCOMPLETE = 4;
        public const int TYPE_MODAL_SUBMIT = 5;

        // Interaction response types
        public const int RESPONSE_PONG = 1;
        public const int RESPONSE_CHANNEL_MESSAGE_WITH_SOURCE = 4;
        public const int RESPONSE_DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE = 5;
        public const int RESPONSE_DEFERRED_UPDATE_MESSAGE = 6;
        public const int RESPONSE_UPDATE_MESSAGE = 7;
    }
}
