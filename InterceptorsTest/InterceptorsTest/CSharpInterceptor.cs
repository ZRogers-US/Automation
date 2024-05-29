namespace System.Runtime.Compilerservices
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal sealed class InterceptsLocationAttribute : Attribute
    {
        public InterceptsLocationAttribute(string filePath, int line, int character)
        {

        }
    }
}