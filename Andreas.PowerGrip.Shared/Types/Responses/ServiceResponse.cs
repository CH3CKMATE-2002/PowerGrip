namespace Andreas.PowerGrip.Shared.Types.Responses;

public class ServiceResponse<T>
{
    public bool Success { get; set; } = true;

    public T? Data { get; set; } = default;

    public string Title { get; set; } = string.Empty;

    public List<ServiceError> Errors { get; set; } = [];

    public bool HasError() => Errors.Count > 0;

    public bool HasError(ErrorKind errorKind) => Errors.Any(error => error.Kind == errorKind);

    public ServiceResponse<T> AddError(ServiceError error)
    {
        Errors.Add(error);
        return this;
    }

    public static ServiceResponse<T> SuccessResponse(string title, T? data = default) => new()
    {
        Title = title,
        Success = true,
        Data = data
    };
    
    public static ServiceResponse<T> ErrorResponse(string title, IEnumerable<ServiceError> errors) => new() 
    { 
        Success = false, 
        Title = title, 
        Errors = errors.ToList()
    };

    public static ServiceResponse<T> ErrorResponse(string title, ServiceError error) => new() 
    { 
        Success = false, 
        Title = title, 
        Errors = [error]
    };

    public static implicit operator ServiceResponse(ServiceResponse<T> response) => new()
    {
        Success = response.Success,
        Data = response.Data,
        Title = response.Title,
        Errors = response.Errors
    };

    public ServiceResponse<TOut> MutateData<TOut>(Func<T?, TOut> mutator) => new()
    {
        Title = this.Title,
        Success = this.Success,
        Data = mutator(Data),
        Errors = this.Errors,
    };

    public ServiceResponse<TOut> ReplaceData<TOut>(TOut? data) => new()
    {
        Title = this.Title,
        Success = this.Success,
        Data = data,
        Errors = this.Errors,
    };

    public ServiceResponse<TOut> MutateDroppingData<TOut>()=> new()
    {
        Title = this.Title,
        Success = this.Success,
        Data = default,
        Errors = this.Errors,
    };
}

public class ServiceResponse : ServiceResponse<object> {}