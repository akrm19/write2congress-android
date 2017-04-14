using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel.Interface
{
    public interface IMyLogger
    {
        void Debug(string msg);
        void Debug(string msg, Exception e);
        void Debug(string format, params object[] args);

        void Error(string msg);
        void Error(string msg, Exception e);
        void Error(string format, params object[] args);

        void Info(string msg);
        void Info(string msg, Exception e);
        void Info(string format, params object[] args);

        void Warn(string msg);
        void Warn(string msg, Exception e);
        void Warn(string format, params object[] args);
    }
}
