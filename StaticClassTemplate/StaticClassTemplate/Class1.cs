using System;
using System.Text;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using System.Collections.Generic;

namespace StaticClassTemplate
{
    public class ChangeEventArgs : EventArgs
    {
        public SimplSharpString key { get; set; }
        // public ushort digitalValue { get; set; }
        // public ushort analogValue { get; set; }
        public SimplSharpString stringValue { get; set; }
        public ChangeEventArgs() { }
    }

    public class SubmoduleEventHandler
    {
        public delegate void SubmoduleHandler(SimplSharpString key, SimplSharpString value);
        public SubmoduleHandler OnSubmoduleEvent { set; get; }
        string key;

        public SubmoduleEventHandler()
        {
            MainClass.submoduleEvent += new MainClass.EventHandler(MainClass_submoduleEvent);
        }

        public void Initialize(SimplSharpString key)
        {
            this.key = key.ToString();
        }

        void MainClass_submoduleEvent(object sender, ChangeEventArgs args)
        {
            if (args.key.ToString().Equals(this.key))
            {
                OnSubmoduleEvent(args.key, args.stringValue);
            }
        }
    }

    public static class MainClass
    {
        public static ChangeEventArgs changeEventArgs = new ChangeEventArgs();
        public static List<SimplSharpString> submoduleIds = new List<SimplSharpString>();
        public delegate void EventHandler(object sender, ChangeEventArgs args);
        public static event EventHandler submoduleEvent;
        public static void onSubmoduleEvent(object sender, ChangeEventArgs args)
        {
            if (submoduleEvent != null)
            {                
                submoduleEvent(sender, args);
            }
        }                
        
        public static void InitializeSubmodule(SimplSharpString key)
        {
            submoduleIds.Add(key);
        }

        public static void SubmoduleRx(SimplSharpString key, SimplSharpString value)
        {
            try
            {
                changeEventArgs.key = key;
                changeEventArgs.stringValue = value;
                onSubmoduleEvent(null, changeEventArgs);
            }
            catch (Exception e)
            {
                ErrorLog.Error("StaticClassTemplate SubmoduleRx Exception: " + e.Message);
            }
        }             
    }
}
