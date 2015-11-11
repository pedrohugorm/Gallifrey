using System;
using System.Collections.Generic;
using System.Linq;

namespace Gallifrey.SharedKernel.Application.Exception
{
    /// <summary>
    /// When more than one <typeparamref name="T"/> is found, and it should have been only one, throw this exception
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MoreThanOneFoundInContainerException<T> : System.Exception
    {
        public MoreThanOneFoundInContainerException(IEnumerable<object> itemsFound)
            : base(
                string.Format("More than one {0} found. Define only one. Items found: {1}{2}",
                    typeof (T).FullName,
                    Environment.NewLine,
                    string.Join(Environment.NewLine, itemsFound.Select(r => r.GetType().FullName))))
        {
        }
    }
}