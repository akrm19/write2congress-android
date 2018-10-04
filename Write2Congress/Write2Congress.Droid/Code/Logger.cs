using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Java.Lang;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Droid.Code
{
    public class Logger : IMyLogger
    {
        private string _tag;
        private string _msgFormat;

        public Logger (string className)
        {
            _tag = "Write2Congress";
            //TODO RM: See why this is null sometime
            //_tag = Application.Context.GetString(Resource.String.app_name) ?? "Write2Congress";
            
            //if (string.IsNullOrWhiteSpace(_tag))
            //    _tag = "Write2Congress";

            _msgFormat = $"{className}: ";
        }

        public void Debug(string msg)
        {
            Log.Debug(_tag, _msgFormat + msg);
        }

        public void Debug(string msg, System.Exception e)
        {
            Log.Debug(_tag, $"{_msgFormat} {msg}.{System.Environment.NewLine}{e.ToString()}");
        }

        public void Debug(string format, params object[] args)
        {
            Log.Debug(_tag, _msgFormat + format, args);
        }

        public void Debug(Throwable tr, string msg)
        {
            Log.Debug(_tag, tr, _msgFormat + msg);
        }
        public void Debug(Throwable tr, string format, params object[] args)
        {
            Log.Debug(_tag, tr, _msgFormat + format, args);
        }

        public void Error(string msg)
        {
            Log.Error(_tag, _msgFormat + msg);
        }

        public void Error(string msg, System.Exception e)
        {
            Log.Error(_tag, $"{_msgFormat} {msg}.{System.Environment.NewLine}{e.ToString()}");
        }

        public void Error(Throwable tr, string msg)
        {
            Log.Error(_tag, tr, _msgFormat + msg);
        }
        public void Error(string format, params object[] args)
        {
            Log.Error(_tag, _msgFormat + format, args);
        }

        public void Error(Throwable tr, string format, params object[] args)
        {
            Log.Error(_tag, tr, _msgFormat + format, args);
        }

        public void Info(string msg)
        {
            Log.Info(_tag, _msgFormat + msg);
        }

        public void Info(string msg, System.Exception e)
        {
            Log.Info(_tag, $"{_msgFormat} {msg}.{System.Environment.NewLine}{e.ToString()}");
        }

        public void Info(string format, params object[] args)
        {
            Log.Info(_tag, _msgFormat + format, args);
        }

        public void Info(Throwable tr, string msg)
        {
            Log.Info(_tag, tr, _msgFormat + msg);
        }

        public void Info(Throwable tr, string format, params object[] args)
        {
            Log.Info(_tag, tr, _msgFormat + format, args);
        }

        public void Verbose( string msg)
        {
            Log.Verbose(_tag, _msgFormat + msg);
        }

        public void Verbose(string format, params object[] args)
        {
            Log.Verbose(_tag, _msgFormat + format, args);
        }

        public void Verbose(Throwable tr, string msg)
        {
            Log.Verbose(_tag, tr, _msgFormat + msg);
        }

        public void Verbose(Throwable tr, string format, params object[] args)
        {
            Log.Verbose(_tag, tr, _msgFormat + format, args);
        }
        public void Warn(string msg)
        {
            Log.Warn(_tag, _msgFormat + msg);
        }

        public void Warn(string msg, System.Exception e)
        {
            Log.Warn(_tag, $"{_msgFormat} {msg}.{System.Environment.NewLine}{e.ToString()}");
        }

        public void Warn(Throwable tr)
        {
            Log.Warn(_tag, tr);
        }

        public void Warn(Throwable tr, string msg)
        {
            Log.Warn(_tag, tr, _msgFormat + msg);
        }

        public void Warn(string format, params object[] args)
        {
            Log.Warn(_tag, _msgFormat + format, args);
        }

        public void Warn(Throwable tr, string format, params object[] args)
        {
            Log.Warn(_tag, tr, _msgFormat + format, args);
        }
    }
}