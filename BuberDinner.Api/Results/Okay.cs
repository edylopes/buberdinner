namespace BuberDinner.Api.Results;

public static class Okay
{
    public static ResponseResult<T> Ok<T>(T payload) => new(payload);
    public static ResponseResult<T> Created<T>(T payload, string location) =>
          new(payload, isNewResource: true, location: location);
    public static ResponseResult<T> NoContent<T>() => new(default!, statusCode: 204);
}
