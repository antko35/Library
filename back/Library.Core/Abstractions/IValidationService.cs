namespace Library.Core.Abstractions
{
    public interface IValidationService
    {
        Task ValidateAsync<T>(T instance);
    }
}