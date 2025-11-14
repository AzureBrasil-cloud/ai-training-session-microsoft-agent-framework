namespace ContosoAutoTech.Application;

public static class Errors
{
    public static readonly Error None = new(Guid.Empty, string.Empty);
    public static readonly Error NullValue = new(ErrorCodes.NullValue.Code, ErrorCodes.NullValue.Message);
    public static Error NotBeNullOrEmpty(string property) => 
        new(
            ErrorCodes.BeNullOrEmpty.Code, 
            ErrorCodes.BeNullOrEmpty.Message,
            new Dictionary<string, object>
            {
                {"property", property}
            });
    
    public static Error EntityNotFound(string entityName, string id) => 
        new(
            ErrorCodes.EntityNotFound.Code, 
            ErrorCodes.EntityNotFound.Message,
            new Dictionary<string, object>
            {
                {"entityName", entityName},
                {"id", id},
            });
    
    public static Error GreaterThanZero(string property) =>
        new(
            ErrorCodes.GreaterThanZero.Code,
            ErrorCodes.GreaterThanZero.Message,
            new Dictionary<string, object>
            {
                { "property", property }
            });
    
    public static Error Unexpected() =>
        new(
            ErrorCodes.Unexpected.Code,
            ErrorCodes.Unexpected.Message,
            new Dictionary<string, object>());
    
    public static Error AiInferenceError() =>
        new(
            ErrorCodes.AiInferenceError.Code,
            ErrorCodes.AiInferenceError.Message,
            new Dictionary<string, object>());
}