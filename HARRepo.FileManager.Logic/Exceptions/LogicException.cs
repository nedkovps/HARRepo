using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.Exceptions
{
    public class LogicException : Exception
    {
        public LogicException(string message) : base(message)
        {

        }
    }
}
