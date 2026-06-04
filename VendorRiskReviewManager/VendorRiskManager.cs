using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace VendorRiskReviewManager
{
    public class VendorRiskManager
    {
        private List<Vendor> vendors = new List<Vendor>();
        private List<VendorReview> reviews = new List<VendorReview>();

        private int nextVendorId = 1001;
        private int nextReviewId = 5001;

        private string vendorFilePath = "vendors.json";
        private string reviewFilePath = "vendorReviews.json";

        public VendorRiskManager()
        {
            LoadDataFromFiles();
        }

        public void AddVendor()
        {
            Console.WriteLine("\n======================================");
            Console.WriteLine("              ADD VENDOR");
            Console.WriteLine("======================================");
            Console.WriteLine("Add an outside company, platform, tool,");
            Console.WriteLine("or service provider your organization uses.");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("- Cloud backup provider");
            Console.WriteLine("- Payroll software vendor");
            Console.WriteLine("- Email security platform");
            Console.WriteLine("- Managed IT provider");
            Console.WriteLine();
            Console.WriteLine("Enter 0 at any prompt to cancel without saving.");
            Console.WriteLine();

            string vendorName = GetTextOrCancel("Vendor name: ");
            if (vendorName == "0")
            {
                Console.WriteLine("Add vendor cancelled.");
                return;
            }

            string serviceProvided = GetTextOrCancel("Service provided, such as Cloud Backup or Payroll Software: ");
            if (serviceProvided == "0")
            {
                Console.WriteLine("Add vendor cancelled.");
                return;
            }

            string contactEmail = GetTextOrCancel("Contact email: ");
            if (contactEmail == "0")
            {
                Console.WriteLine("Add vendor cancelled.");
                return;
            }

            Vendor vendor = new Vendor(nextVendorId, vendorName, serviceProvided, contactEmail);

            vendors.Add(vendor);
            SaveDataToFiles();

            Console.WriteLine($"\nVendor added successfully. Vendor ID: {nextVendorId}");

            nextVendorId++;
        }

        public void ViewVendors()
        {
            Console.WriteLine("\n======================================");
            Console.WriteLine("              VENDORS");
            Console.WriteLine("======================================");

            if (vendors.Count == 0)
            {
                Console.WriteLine("No vendors have been added yet.");
                return;
            }

            foreach (Vendor vendor in vendors)
            {
                Console.WriteLine($"\nVendor ID: {vendor.VendorId}");
                Console.WriteLine($"Name: {vendor.VendorName}");
                Console.WriteLine($"Service: {vendor.ServiceProvided}");
                Console.WriteLine($"Contact: {vendor.ContactEmail}");
            }
        }

        public void CreateVendorReview()
        {
            Console.WriteLine("\n======================================");
            Console.WriteLine("        CREATE VENDOR RISK REVIEW");
            Console.WriteLine("======================================");
            Console.WriteLine("Use this to document a risk review for");
            Console.WriteLine("a vendor your organization depends on.");
            Console.WriteLine();
            Console.WriteLine("Enter 0 at any prompt to cancel without saving.");
            Console.WriteLine();

            if (vendors.Count == 0)
            {
                Console.WriteLine("You must add a vendor before creating a review.");
                return;
            }

            DisplayVendorSummary();

            int vendorId = GetIdOrCancel("\nEnter Vendor ID for this review, or 0 to cancel: ");
            if (vendorId == 0)
            {
                Console.WriteLine("Create review cancelled.");
                return;
            }

            Vendor vendor = vendors.Find(item => item.VendorId == vendorId);

            if (vendor == null)
            {
                Console.WriteLine("No vendor with that ID was found.");
                return;
            }

            string riskLevel = GetRiskLevelFromUser();

            if (riskLevel == "Cancel")
            {
                Console.WriteLine("Create review cancelled.");
                return;
            }

            DateTime nextReviewDate = GetDateOrCancel("Next review date, such as 12/15/2026, or 0 to cancel: ");

            if (nextReviewDate == DateTime.MinValue)
            {
                Console.WriteLine("Create review cancelled.");
                return;
            }

            VendorReview review = new VendorReview(nextReviewId, vendorId, riskLevel, nextReviewDate);

            reviews.Add(review);
            SaveDataToFiles();

            Console.WriteLine("\nVendor risk review created successfully.");
            Console.WriteLine($"Review ID: {nextReviewId}");
            Console.WriteLine($"Vendor: {vendor.VendorName}");
            Console.WriteLine($"Risk Level: {riskLevel}");

            nextReviewId++;
        }

        public void ViewAllReviews()
        {
            Console.WriteLine("\n======================================");
            Console.WriteLine("          VENDOR RISK REVIEWS");
            Console.WriteLine("======================================");

            if (reviews.Count == 0)
            {
                Console.WriteLine("No vendor reviews have been created yet.");
                return;
            }

            foreach (VendorReview review in reviews)
            {
                DisplayReview(review);
            }
        }

        public void ViewReviewsDueSoon()
        {
            Console.WriteLine("\n======================================");
            Console.WriteLine("          REVIEWS DUE SOON");
            Console.WriteLine("======================================");
            Console.WriteLine("Showing open reviews due within 60 days.");
            Console.WriteLine();

            bool found = false;

            foreach (VendorReview review in reviews)
            {
                int daysUntil = review.GetDaysUntilNextReview();

                if (review.ReviewStatus != "Closed" && daysUntil >= 0 && daysUntil <= 60)
                {
                    DisplayReview(review);
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No reviews are due within 60 days.");
            }
        }

        public void UpdateReviewStatus()
        {
            Console.WriteLine("\n======================================");
            Console.WriteLine("        UPDATE REVIEW STATUS");
            Console.WriteLine("======================================");
            Console.WriteLine("Enter 0 at any prompt to cancel without saving.");
            Console.WriteLine();

            if (reviews.Count == 0)
            {
                Console.WriteLine("No reviews are available.");
                return;
            }

            DisplayReviewSummary();

            int reviewId = GetIdOrCancel("\nEnter Review ID to update, or 0 to cancel: ");
            if (reviewId == 0)
            {
                Console.WriteLine("Status update cancelled.");
                return;
            }

            VendorReview review = reviews.Find(item => item.ReviewId == reviewId);

            if (review == null)
            {
                Console.WriteLine("No review with that ID was found.");
                return;
            }

            string status = GetReviewStatusFromUser();

            if (status == "Cancel")
            {
                Console.WriteLine("Status update cancelled.");
                return;
            }

            review.ReviewStatus = status;
            SaveDataToFiles();

            Console.WriteLine($"Review {review.ReviewId} updated to {review.ReviewStatus}.");
        }

        public void AddReviewNote()
        {
            Console.WriteLine("\n======================================");
            Console.WriteLine("             ADD REVIEW NOTE");
            Console.WriteLine("======================================");
            Console.WriteLine("Use notes to document follow-up actions,");
            Console.WriteLine("requested evidence, concerns, or decisions.");
            Console.WriteLine();
            Console.WriteLine("Enter 0 at any prompt to cancel without saving.");
            Console.WriteLine();

            if (reviews.Count == 0)
            {
                Console.WriteLine("No reviews are available.");
                return;
            }

            DisplayReviewSummary();

            int reviewId = GetIdOrCancel("\nEnter Review ID for this note, or 0 to cancel: ");
            if (reviewId == 0)
            {
                Console.WriteLine("Add note cancelled.");
                return;
            }

            VendorReview review = reviews.Find(item => item.ReviewId == reviewId);

            if (review == null)
            {
                Console.WriteLine("No review with that ID was found.");
                return;
            }

            if (review.Notes == null)
            {
                review.Notes = new List<ReviewNote>();
            }

            string noteText = GetTextOrCancel("Note text, or 0 to cancel: ");
            if (noteText == "0")
            {
                Console.WriteLine("Add note cancelled.");
                return;
            }

            review.AddNote(new ReviewNote(noteText));
            SaveDataToFiles();

            Console.WriteLine("Review note added successfully.");
        }

        public void ViewReviewNotes()
        {
            Console.WriteLine("\n======================================");
            Console.WriteLine("             REVIEW NOTES");
            Console.WriteLine("======================================");

            if (reviews.Count == 0)
            {
                Console.WriteLine("No reviews are available.");
                return;
            }

            DisplayReviewSummary();

            int reviewId = GetIdOrCancel("\nEnter Review ID to view notes, or 0 to cancel: ");
            if (reviewId == 0)
            {
                Console.WriteLine("View notes cancelled.");
                return;
            }

            VendorReview review = reviews.Find(item => item.ReviewId == reviewId);

            if (review == null)
            {
                Console.WriteLine("No review with that ID was found.");
                return;
            }

            if (review.Notes == null || review.Notes.Count == 0)
            {
                Console.WriteLine("No notes have been added for this review.");
                return;
            }

            foreach (ReviewNote note in review.Notes)
            {
                Console.WriteLine($"\n[{note.DateAdded}]");
                Console.WriteLine(note.NoteText);
            }
        }

        public void ViewRiskDashboard()
        {
            Console.WriteLine("\n======================================");
            Console.WriteLine("          VENDOR RISK DASHBOARD");
            Console.WriteLine("======================================");

            int low = 0;
            int medium = 0;
            int high = 0;
            int critical = 0;
            int open = 0;
            int inReview = 0;
            int closed = 0;

            foreach (VendorReview review in reviews)
            {
                if (review.RiskLevel == "Low") low++;
                else if (review.RiskLevel == "Medium") medium++;
                else if (review.RiskLevel == "High") high++;
                else if (review.RiskLevel == "Critical") critical++;

                if (review.ReviewStatus == "Open") open++;
                else if (review.ReviewStatus == "In Review") inReview++;
                else if (review.ReviewStatus == "Closed") closed++;
            }

            Console.WriteLine($"Vendors in System: {vendors.Count}");
            Console.WriteLine($"Total Reviews: {reviews.Count}");
            Console.WriteLine();
            Console.WriteLine("--- Risk Level Counts ---");
            Console.WriteLine($"Low: {low}");
            Console.WriteLine($"Medium: {medium}");
            Console.WriteLine($"High: {high}");
            Console.WriteLine($"Critical: {critical}");
            Console.WriteLine();
            Console.WriteLine("--- Review Status Counts ---");
            Console.WriteLine($"Open: {open}");
            Console.WriteLine($"In Review: {inReview}");
            Console.WriteLine($"Closed: {closed}");
        }

        private string GetRiskLevelFromUser()
        {
            while (true)
            {
                Console.WriteLine("\nChoose vendor risk level:");
                Console.WriteLine("1. Low - Limited business or security impact");
                Console.WriteLine("2. Medium - Important service with moderate risk");
                Console.WriteLine("3. High - Critical service or sensitive data involved");
                Console.WriteLine("4. Critical - Major business dependency or severe exposure");
                Console.WriteLine("0. Cancel and return to main menu");
                Console.Write("Choose option 0 through 4: ");

                string choice = Console.ReadLine();

                if (choice == "1") return "Low";
                if (choice == "2") return "Medium";
                if (choice == "3") return "High";
                if (choice == "4") return "Critical";
                if (choice == "0") return "Cancel";

                Console.WriteLine("Invalid option. Please choose 0 through 4.");
            }
        }

        private string GetReviewStatusFromUser()
        {
            while (true)
            {
                Console.WriteLine("\nChoose review status:");
                Console.WriteLine("1. Open");
                Console.WriteLine("2. In Review");
                Console.WriteLine("3. Waiting on Vendor");
                Console.WriteLine("4. Approved");
                Console.WriteLine("5. Closed");
                Console.WriteLine("0. Cancel and return to main menu");
                Console.Write("Choose option 0 through 5: ");

                string choice = Console.ReadLine();

                if (choice == "1") return "Open";
                if (choice == "2") return "In Review";
                if (choice == "3") return "Waiting on Vendor";
                if (choice == "4") return "Approved";
                if (choice == "5") return "Closed";
                if (choice == "0") return "Cancel";

                Console.WriteLine("Invalid option. Please choose 0 through 5.");
            }
        }

        private string GetTextOrCancel(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine().Trim();
        }

        private int GetIdOrCancel(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine().Trim();

                bool isValidNumber = int.TryParse(input, out int id);

                if (isValidNumber)
                {
                    return id;
                }

                Console.WriteLine("Invalid input. Enter a number, or 0 to cancel.");
            }
        }

        private DateTime GetDateOrCancel(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine().Trim();

                if (input == "0")
                {
                    return DateTime.MinValue;
                }

                bool isValidDate = DateTime.TryParse(input, out DateTime date);

                if (isValidDate)
                {
                    return date;
                }

                Console.WriteLine("Invalid date. Enter a date like 12/15/2026, or 0 to cancel.");
            }
        }

        private void DisplayVendorSummary()
        {
            Console.WriteLine("\n--- Current Vendors ---");

            foreach (Vendor vendor in vendors)
            {
                Console.WriteLine($"ID: {vendor.VendorId} | {vendor.VendorName} | Service: {vendor.ServiceProvided}");
            }
        }

        private void DisplayReviewSummary()
        {
            Console.WriteLine("\n--- Current Reviews ---");

            foreach (VendorReview review in reviews)
            {
                Console.WriteLine($"Review ID: {review.ReviewId} | Vendor ID: {review.VendorId} | Risk: {review.RiskLevel} | Status: {review.ReviewStatus}");
            }
        }

        private void DisplayReview(VendorReview review)
        {
            Vendor vendor = vendors.Find(item => item.VendorId == review.VendorId);

            string vendorName = vendor == null ? "Unknown Vendor" : vendor.VendorName;

            int noteCount = review.Notes == null ? 0 : review.Notes.Count;

            int daysUntilReview = review.GetDaysUntilNextReview();

            string reviewTiming = daysUntilReview >= 0
                ? $"{daysUntilReview} day(s) until next review"
                : $"{Math.Abs(daysUntilReview)} day(s) past review date";

            Console.WriteLine("\n------------------------------");
            Console.WriteLine($"Review ID: {review.ReviewId}");
            Console.WriteLine($"Vendor: {vendorName}");
            Console.WriteLine($"Risk Level: {review.RiskLevel}");
            Console.WriteLine($"Status: {review.ReviewStatus}");
            Console.WriteLine($"Review Date: {review.ReviewDate}");
            Console.WriteLine($"Next Review Date: {review.NextReviewDate.ToShortDateString()}");
            Console.WriteLine($"Review Timing: {reviewTiming}");
            Console.WriteLine($"Notes: {noteCount}");
        }

        private void SaveDataToFiles()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string vendorJson = JsonSerializer.Serialize(vendors, options);
            string reviewJson = JsonSerializer.Serialize(reviews, options);

            File.WriteAllText(vendorFilePath, vendorJson);
            File.WriteAllText(reviewFilePath, reviewJson);
        }

        private void LoadDataFromFiles()
        {
            if (File.Exists(vendorFilePath))
            {
                string vendorJson = File.ReadAllText(vendorFilePath);

                if (!string.IsNullOrWhiteSpace(vendorJson))
                {
                    vendors = JsonSerializer.Deserialize<List<Vendor>>(vendorJson);

                    if (vendors == null)
                    {
                        vendors = new List<Vendor>();
                    }
                }
            }

            if (File.Exists(reviewFilePath))
            {
                string reviewJson = File.ReadAllText(reviewFilePath);

                if (!string.IsNullOrWhiteSpace(reviewJson))
                {
                    reviews = JsonSerializer.Deserialize<List<VendorReview>>(reviewJson);

                    if (reviews == null)
                    {
                        reviews = new List<VendorReview>();
                    }
                }
            }

            foreach (Vendor vendor in vendors)
            {
                if (vendor.VendorId >= nextVendorId)
                {
                    nextVendorId = vendor.VendorId + 1;
                }
            }

            foreach (VendorReview review in reviews)
            {
                if (review.ReviewId >= nextReviewId)
                {
                    nextReviewId = review.ReviewId + 1;
                }

                if (review.Notes == null)
                {
                    review.Notes = new List<ReviewNote>();
                }
            }
        }
    }
}