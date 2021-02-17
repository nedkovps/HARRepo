using System;
using System.Collections.Generic;
using System.Text;

namespace HARRepo.FileManager.Logic.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {
        }

        public NotFoundException(string message) : base(message)
        {

        }
    }
}
