using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class DebugCommandBase
{
    [SerializeField] string commandID;
    [SerializeField] string commandDescription;
    [SerializeField] string commandFormat;


    public string CommandID { get { return commandID; } }
    public string CommandDescription { get { return commandDescription; } }
    public string CommandFormat { get { return commandFormat; } }


    public DebugCommandBase(string id, string desc, string format)
    {
        this.commandID = id;
        this.commandDescription = desc;
        this.commandFormat = format;
    }


}


[Serializable]
public class DebugCommand : DebugCommandBase
{
    Action command;


    public DebugCommand(string id, string desc, string format, Action command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}


[Serializable]
public class DebugCommand<T1> : DebugCommandBase
{
    Action<T1> command;


    public DebugCommand(string id, string desc, string format, Action<T1> command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}

[Serializable]
public class DebugCommand<T1, T2> : DebugCommandBase
{
    Action<T1, T2> command;


    public DebugCommand(string id, string desc, string format, Action<T1, T2> command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke(T1 valueOne, T2 valueTwo)
    {
        command.Invoke(valueOne, valueTwo);
    }
}

