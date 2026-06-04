using System;
using System.Collections.Generic;

namespace VendorRiskReviewManager
{
    public class VendorReview
    {
        public int ReviewId { get; set; }
        public int VendorId { get; set; }
        public string RiskLevel { get; set; }
        public string ReviewStatus { get; set; }
        public DateTime ReviewDate { get; set; }
        public DateTime NextReviewDate { get; set; }
        public List<ReviewNote> Notes { get; set; }

        public VendorReview()
        {
            Notes = new List<ReviewNote>();
        }

        public VendorReview(int reviewId, int vendorId, string riskLevel, DateTime nextReviewDate)
        {
            ReviewId = reviewId;
            VendorId = vendorId;
            RiskLevel = riskLevel;
            ReviewStatus = "Open";
            ReviewDate = DateTime.Now;
            NextReviewDate = nextReviewDate;
            Notes = new List<ReviewNote>();
        }

        public int GetDaysUntilNextReview()
        {
            return (NextReviewDate.Date - DateTime.Today).Days;
        }

        public void AddNote(ReviewNote note)
        {
            Notes.Add(note);
        }
    }
}