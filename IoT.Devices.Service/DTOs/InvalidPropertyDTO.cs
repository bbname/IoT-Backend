namespace IoT.Devices.Service.DTOs
{
    public class InvalidPropertyDTO
    {
        public InvalidPropertyDTO(string propertyName, string validationMessage)
        {
            PropertyName = propertyName;
            ValidationMessage = validationMessage;
        }

        public string PropertyName { get; }
        public string ValidationMessage { get; }
    }
}
