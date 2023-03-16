using System;
using System.Collections.Generic;

namespace projecttour.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Image { get; set; }

    public int? CategoryBlogId { get; set; }

    public virtual CategoryBlog? CategoryBlog { get; set; }
}
