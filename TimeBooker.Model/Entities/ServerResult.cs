namespace TimeBooker.Model.Entities;

public class ServerResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
}

public class ServerResult<T> : ServerResult
{
    public T Data { get; set; }
}