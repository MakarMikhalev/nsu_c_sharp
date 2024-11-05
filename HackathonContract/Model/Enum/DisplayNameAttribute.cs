namespace HackathonContract.Model;

using System;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
sealed class DisplayNameAttribute(string name) : Attribute
{
    public string Name => name;
}