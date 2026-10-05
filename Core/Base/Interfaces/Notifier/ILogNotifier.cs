namespace Base
{
    /// <summary>
    ///     Sends logs via notifier. This interface defines methods for logging messages of different severity levels, such as success, error, warning, info, process info, and process error. Each method takes a message ID and an array of arguments to format the message.
    /// </summary>
    public interface ILogNotifier
    {
        /// <summary>
        /// Logs a success message identified by the specified message ID, formatted with the provided arguments.
        /// </summary>
        /// <param name="messageId">The unique identifier for the message to be logged.</param>
        /// <param name="args">An array of objects that represent the arguments to format the message.</param>
        /// <returns>A task representing the asynchronous operation of logging the message.</returns>
        Task Success(int messageId, params object[] args);

        /// <summary>
        /// Logs a error message identified by the specified message ID, formatted with the provided arguments.
        /// </summary>
        /// <param name="messageId">The unique identifier for the message to be logged.</param>
        /// <param name="args">An array of objects that represent the arguments to format the message.</param>
        /// <returns>A task representing the asynchronous operation of logging the message.</returns>
        Task Error(int messageId, params object[] args);


        /// <summary>
        /// Logs a warning message identified by the specified message ID, formatted with the provided arguments.
        /// </summary>
        /// <param name="messageId">The unique identifier for the message to be logged.</param>
        /// <param name="args">An array of objects that represent the arguments to format the message.</param>
        /// <returns>A task representing the asynchronous operation of logging the message.</returns>
        Task Warning(int messageId, params object[] args);

        /// <summary>
        /// Logs a info message identified by the specified message ID, formatted with the provided arguments.
        /// </summary>
        /// <param name="messageId">The unique identifier for the message to be logged.</param>
        /// <param name="args">An array of objects that represent the arguments to format the message.</param>
        /// <returns>A task representing the asynchronous operation of logging the message.</returns>
        Task Info(int messageId, params object[] args);

        /// <summary>
        /// Logs a process info message identified by the specified message ID, formatted with the provided arguments.
        /// </summary>
        /// <param name="messageId">The unique identifier for the message to be logged.</param>
        /// <param name="args">An array of objects that represent the arguments to format the message.</param>
        /// <returns>A task representing the asynchronous operation of logging the message.</returns>
        Task Process(int messageId, params object[] args);

        /// <summary>
        /// Logs a process error message identified by the specified message ID, formatted with the provided arguments.
        /// </summary>
        /// <param name="messageId">The unique identifier for the message to be logged.</param>
        /// <param name="args">An array of objects that represent the arguments to format the message.</param>
        /// <returns>A task representing the asynchronous operation of logging the message.</returns>
        Task ProcessError(int messageId, params object[] args);
    }
}
