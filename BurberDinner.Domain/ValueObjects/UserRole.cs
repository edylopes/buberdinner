

using System.Text.Json.Serialization;
using BurberDinner.Domain.Exceptions;

namespace BurberDinner.Domain.ValueObjects
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleType
    {
        User,
        Admin,
        Premium
    }
    internal record UserRole
    {
        public RoleType Value { get; }

        private UserRole(RoleType value) => Value = value;

        public static UserRole Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("User role cannot be empty");

            return Enum.TryParse<RoleType>("value", true, out var roleType)
                ? new UserRole(roleType)
                : throw new UserRoleNotAllowedException(value);

        }

        public override string ToString() => Value.ToString();
    }

}