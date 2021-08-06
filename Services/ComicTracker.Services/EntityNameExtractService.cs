namespace ComicTracker.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    using ComicTracker.Services.Contracts;

    public class EntityNameExtractService : IEntityNameExtractService
    {
        public string ExtractEntityTypeName(Type entityType)
        {
            var pattern = this.FormPattern();

            if (Regex.IsMatch(entityType.Name, pattern))
            {
                return Regex.Match(entityType.Name, pattern).Value;
            }

            return string.Empty;
        }

        private string FormPattern()
        {
            StringBuilder pattern = new StringBuilder();

            var assemblyName = "ComicTracker.Data.Models";
            var nameSpace = "ComicTracker.Data.Models.Entities";

            var asm = Assembly.Load(assemblyName);
            var classes = asm.GetTypes()
                .Where(t => t.Namespace == nameSpace)
                .Select(t => t.Name)
                .ToArray();

            for (int i = 0; i < classes.Length; i++)
            {
                pattern.Append(classes[i]);

                if (i != classes.Length - 1)
                {
                    pattern.Append("|");
                }
            }

            return pattern.ToString();
        }
    }
}
