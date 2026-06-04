using System;

namespace VendorRiskReviewManager
{
    public class ReviewNote
    {
        public string NoteText { get; set; }
        public DateTime DateAdded { get; set; }

        public ReviewNote()
        {
        }

        public ReviewNote(string noteText)
        {
            NoteText = noteText;
            DateAdded = DateTime.Now;
        }
    }
}