namespace SqlServerWrapper.Wrapper.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FieldAttribute : Attribute
{
    /// <summary>
    /// Name of column in data base table
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Set field name in data base table.
    /// </summary>
    /// <param name="name">Field name in data base table</param>
    public FieldAttribute(string name) => Name = name;

    /// <summary>
    /// Set field name in data base table and add description about why you did this.
    /// </summary>
    /// <param name="name">Field name in data base table</param>
    /// <param name="description">your description</param>
    public FieldAttribute(string name, string description) => (Name) = (name);

    /// <summary>
    /// Set field name in data base table and add description about why you did this and alse who did this. 
    /// </summary>
    /// <param name="name">Field name in data base table</param>
    /// /// <param name="autor">Developer name who added this attribure</param>
    /// <param name="description">your description</param>
    public FieldAttribute(string name, string autor, string description) => (Name) = (name);
}
