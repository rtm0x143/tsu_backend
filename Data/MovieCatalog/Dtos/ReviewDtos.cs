using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public class ReviewShortModel
{
    [Required] public Guid id { get; set; }
    public int rating { get; set; }

    public static explicit operator ReviewShortModel(Review review) => new() { id = review.Id, rating = review.Rating };
}

public class ReviewModel : ReviewShortModel
{
    public string? reviewText { get; set; }
    public bool isAnonymous { get; set; }
    public DateTime? createDateTime { get; set; }
    public UserShortModel author { get; set; }

    public static explicit operator ReviewModel(Review review) => new()
    {
        id = review.Id, rating = review.Rating, author = (UserShortModel)review.Creator,
        isAnonymous = review.IsAnonymous,
        reviewText = review.ReviewText, createDateTime = review.CreateDateTime
    };

    public Review ToReview(in Guid movieId) => new()
    {
        Id = id, TargetMovieId = movieId, CreatorId = author.userId, Rating = rating,
        ReviewText = reviewText ?? string.Empty, IsAnonymous = isAnonymous,
        CreateDateTime = createDateTime ?? DateTime.UtcNow
    };
}