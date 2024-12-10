using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace DBAgent.Models;

public partial class Field
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Block { get; set; } = null!;
    public string Page { get; set; } = null!;
    public bool Mutability { get; set; }
}
