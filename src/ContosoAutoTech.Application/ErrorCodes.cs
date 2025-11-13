namespace ContosoAutoTech.Application;

public class ErrorCodes
{
    public static (Guid Code, string Message) BeNullOrEmpty = (Guid.Parse("7C254CFC-19BB-4A64-A640-28E6F2127277"), "The '{property}' must not be null or empty");
    public static (Guid Code, string Message) EntityNotFound = (Guid.Parse("143C4702-5E1A-493E-BF1C-91321431C366"), "Entity '{entityName}' with Id '{id}' not found");
    public static (Guid Code, string Message) NullValue = (Guid.Parse("1D238A51-F3A5-4C20-8D4F-CEC9C2848634"), "The '{property}' must not be null");
    public static (Guid Code, string Message) GreaterThanZero = (Guid.Parse("6C8E0B49-880D-4ACE-88AB-EA4DAE3362E8"), "The '{property}' must be greater to 0");
    public static (Guid Code, string Message) Unexpected = (Guid.Parse("C43B8E75-54F6-4546-A545-FDC9B7564745"), "Unexpected error");
    public static (Guid Code, string Message) AiInferenceError = (Guid.Parse("8F2A4D3E-1B5C-4E7F-9A2D-6C8E9B1F3A7C"), "An error occurred during AI inference");
}