using System.Runtime.Serialization;

namespace Ork.Data;

/// <summary>
/// Specifies the supported types of database engines for data providers.
/// </summary>
/// <remarks>Use this enumeration to indicate the database engine type when configuring or interacting with data
/// providers. The values correspond to common relational database systems. The <see cref="Unknown"/> value can be used
/// when the engine type is not specified or cannot be determined.</remarks>
public enum DataProviderType
{
    /// <summary>
    /// Represents an unknown engine type.
    /// </summary>
    [EnumMember(Value = "")]
    Unknown = 0,

    /// <summary>
    /// Represents the MSSQL database engine type.
    /// </summary>
    [EnumMember(Value = "sqlserver")]
    SqlServer = 1,

    /// <summary>
    /// Represents the MySQL database engine type.
    /// </summary>
    [EnumMember(Value = "mysql")]
    MySql = 2,

    /// <summary>
    /// Represents the PostgreSQL database engine type.
    /// </summary>
    [EnumMember(Value = "postgresql")]
    PostgreSQL = 3,
}
