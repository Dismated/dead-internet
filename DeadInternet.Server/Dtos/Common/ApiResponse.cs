namespace DeadInternet.Server.Dtos.Common
{
    public class ApiResponse<T>(T data)
    {
        public T Data { get; set; } = data;
    }
}
