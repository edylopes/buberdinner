using System.Text.Json.Serialization;
namespace BuberDinner.Domain.ValueObjects
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleType
    {
        User,
        Admin,
        Premium,
    }

    public record UserRole
    {
        private RoleType Value { get; }

        private UserRole(RoleType value) => Value = value;

        public static UserRole Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("User role cannot be empty");

            return Enum.TryParse<RoleType>(value, true, out var roleType)
                ? new UserRole(roleType)
                : throw new InvalidOperationException(value);
        }

        public override string ToString() => Value.ToString();
    }
}
