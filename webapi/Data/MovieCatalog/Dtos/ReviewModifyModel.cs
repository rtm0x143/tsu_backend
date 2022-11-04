using System.ComponentModel.DataAnnotations;

namespace MovieCatalogBackend.Data.MovieCatalog.Dtos;

public class ReviewModifyModel
{
    [Required] public string reviewText { get; set; }

    [Required]
    [Range(0, 10, ErrorMessage = "rating can only take values in range [0, 10]")]
    public int? rating { get; set; }

    [Required] public bool isAnonymous { get; set; }

    public Review ToReview(in Guid creatorId, in Guid targetMovieId, in Guid reviewId)
    {
        var review = ToReview(creatorId, targetMovieId);
        review.Id = reviewId;
        return review;
    }

    public Review ToReview(in Guid creatorId, in Guid targetMovieId) => new()
    {
        CreatorId = creatorId,
        TargetMovieId = targetMovieId,
        IsAnonymous = isAnonymous,
        ReviewText = reviewText,
        Rating = rating ?? throw new ArgumentNullException(nameof(rating), "rating field is required")
    };
}