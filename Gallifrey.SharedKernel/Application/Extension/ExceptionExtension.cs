using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Gallifrey.SharedKernel.Application.Extension
{
    public static class ExceptionExtension
    {
        /// <summary>
        /// Flattens the hierarchy of the exception. It will get all Inner Exceptions and show a list of them in order to ease showing those messages
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static IEnumerable<System.Exception> FlattenHierarchy(this System.Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            var innerException = exception;
            do
            {
                yield return innerException;
                innerException = innerException.InnerException;
            }
            while (innerException != null);
        }

        /// <summary>
        /// Exception to a detailed string with Type namespace, message(, and stacktrace if DEBUG MODE is ON)
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="isIncludeStack"></param>
        /// <returns></returns>
        public static string ToDetailedMessage(this System.Exception exception, bool isIncludeStack = true)
        {
            return JsonConvert.SerializeObject(exception.ToSimpleExceptionDetail(isIncludeStack));
        }

        public static IEnumerable<StackFrame> ToStackFrameArray(this System.Exception exception)
        {
            return new StackTrace(exception, true).GetFrames();
        }

        public static SimpleExceptionDetail ToSimpleExceptionDetail(this System.Exception exception, bool isIncludeStack = true)
        {
            return new SimpleExceptionDetail(exception, isIncludeStack);
        }
    }
}
