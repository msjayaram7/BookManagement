using System;
using System.Collections.Generic;

namespace BookManagementAPI.Entity;

public class BookDetail
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string? Author { get; set; }

    public string? Genre { get; set; }

    public string? PublishedYear { get; set; }

    public decimal? Price { get; set; }

    public string? DiscountPercentage { get; set; }

    public decimal? FinalPrice { get; set; }

    public string? Ratings { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedTimeStamp { get; set; }

    public DateTime UpdatedTimeStamp { get; set; }
}
