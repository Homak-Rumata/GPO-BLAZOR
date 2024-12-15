using System;
using System.Collections.Generic;

namespace DBAgent.Models;

public partial class Template
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string TemplateBody { get; set; } = null!;
}
