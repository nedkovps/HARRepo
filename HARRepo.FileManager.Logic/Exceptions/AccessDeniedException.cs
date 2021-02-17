using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException() : base()
        {

        }

        public AccessDeniedException(string message) : base(message)
        {

        }
    }
}
