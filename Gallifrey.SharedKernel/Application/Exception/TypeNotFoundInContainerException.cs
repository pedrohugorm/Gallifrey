namespace Gallifrey.SharedKernel.Application.Exception
{
    /// <summary>
    /// When some <typeparamref name="T"/> is required, but fails to load raises this exception
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeNotFoundInContainerException<T> : System.Exception
    {
        public TypeNotFoundInContainerException()
            : base(string.Format("Could not find {0} in IoC container", typeof (T).FullName))
        {
        }
    }
}
