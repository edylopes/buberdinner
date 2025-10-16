

using OneOf;

namespace BuberDinner.Application.Common.Extensions
{

    public static class OneOfExtensiopns
    {
        public static bool IsSuccess(this object response)
        {
            var type = response.GetType();

            // Tenta pegar a propriedade IsT0 via reflection de forma concisa
            var isT0Prop = type.GetProperty("IsT0");

            return type.IsGenericType &&
             type.GetGenericTypeDefinition() == typeof(OneOf<,>)
               ? isT0Prop != null && (bool)isT0Prop.GetValue(response)!
               : false;

        }
        public static bool IsTOSuccess(this object response)
        {
            var prop = response.GetType().GetProperty("IsT0");
            return prop != null && (bool)prop.GetValue(response)!;
        }

    }


}