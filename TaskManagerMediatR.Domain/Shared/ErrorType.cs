namespace TaskManagerMediatR.Domain.Shared
{
    public enum ErrorType
    {
        Failure,      
        Validation,   
        NotFound,     
        Conflict,    
        Unauthorized, 
        Forbidden
    }
}
