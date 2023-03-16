using System;
using System.Collections.Generic;

namespace projecttour.Models;

public partial class CategoryBlog
{
    public int CategoryBlogId { get; set; }

    public string? CategoryBlogName { get; set; }

    public virtual ICollection<Blog> Blogs { get; } = new List<Blog>();
}
