using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoTagger
{
    public static class ExtensionMethods
    {
        public static string ToStringExtended(this object obj)
        {
            var type = obj.GetType();

            var props = type.GetProperties();
            var fields = type.GetFields();

            var sb = new StringBuilder("[");

            // Header
            sb.Append(type.Name).Append(": ");

            foreach (var p in props)
            {
                sb.Append(p.Name).Append("=").Append(p.GetValue(obj, null)).Append(", ");
            }

            foreach (var f in fields)
            {
                sb.Append(f.Name).Append("=").Append(f.GetValue(obj)).Append(", ");
            }

            // Tail
            sb.Append("]");

            return sb.ToString();
        }
    }
}
