namespace Gallifrey.RestApi.Application.Domain.Model
{
    public interface IResponse<T>
    {
        T Response { set; get; }
    }
}