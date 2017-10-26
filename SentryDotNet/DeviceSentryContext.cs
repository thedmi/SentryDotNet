namespace SentryDotNet
{
    /// <summary>
    /// This describes the device that caused the event. This is most appropriate for mobile applications.
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/contexts/
    /// </summary>
    public class DeviceSentryContext : ISentryContext
    {
        public string Type => ContextTypes.Device;

        /// <summary>
        /// The name of the device. This is typically a hostname.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The family of the device. This is normally the common part of model names across generations. For instance iPhone would
        /// be a reasonable family, so would be Samsung Galaxy.
        /// </summary>
        public string Family { get; set; }

        /// <summary>
        /// The model name. This for instance can be Samsung Galaxy S3.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// An internal hardware revision to identify the device exactly.
        /// </summary>
        public string ModelId { get; set; }

        /// <summary>
        /// The CPU architecture.
        /// </summary>
        public string Arch { get; set; }

        /// <summary>
        /// If the device has a battery this can be an integer defining the battery level (in the range 0-100).
        /// </summary>
        public int BatteryLevel { get; set; }

        /// <summary>
        /// This can be a string portrait or landscape to define the orientation of a device.
        /// </summary>
        public string Orientation { get; set; }
    }
}