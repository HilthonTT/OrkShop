namespace Ork.Core.ComponentModel;

/// <summary>
/// Reader/Write locker type
/// </summary>
public enum ReaderWriteLockType
{
    Read = 0,
    Write = 1,
    UpgradeableRead = 2,
}
