namespace E_Learning.Models
{
    public class ReviewStatViewModel
    {
       
            public int CourseId { get; set; }
            public string CourseTitle { get; set; }
            public double AvgRating { get; set; }
            public int TotalReviews { get; set; }
            public int? SelectedRating { get; set; }

            public List<RatingGroup> RatingGroups { get; set; }
            public List<CourseReview> Reviews { get; set; }
        }

        public class RatingGroup
        {
            public int Rating { get; set; }
            public int Count { get; set; }
        }

    
}
