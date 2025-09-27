namespace BuberDinner.Api.Results;

public static class HttpResults
{
    public static ResponseResult<T> Ok<T>(T payload) => new(payload);
    public static ResponseResult<T> Created<T>(T payload, string location) =>
          new(payload, isNewResource: true, location: location, statusCode: StatusCodes.Status201Created);
    public static ResponseResult<T> NoContent<T>() => new(default!);
}
