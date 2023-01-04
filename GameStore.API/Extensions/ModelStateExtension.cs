using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GameStore.API.Extensions
{
    public static class ModelStateExtension
    {
        public static Dictionary<string, string[]> AllErrors(this ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    key => key.Key.ToCamelCase(),
                    value => value.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return errors;
        }
        private static string ToCamelCase(this string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }
            return name.Substring(0, 1).ToLower() + name.Substring(1);
        }
    }
}
