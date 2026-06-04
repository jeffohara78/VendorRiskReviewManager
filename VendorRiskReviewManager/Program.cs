/* Jeff O'Hara
 * 6/4/2026
 * 
 * Helps organizations track third-party vendors, document vendor risk assessments, monitor review status and due dates, 
 * and maintain review notes and supporting information. It provides dashboard reporting, risk-level tracking, recurring review management, 
 * JSON persistence, and user-friendly workflows for managing vendor cybersecurity and business risk.
 */

using System;



namespace VendorRiskReviewManager
{
    class Program
    {
        static void Main(string[] args)
        {
            VendorRiskManager manager = new VendorRiskManager();

            bool running = true;

            while (running)
            {
                Console.WriteLine("\n==========================================");
                Console.WriteLine("        VENDOR RISK REVIEW MANAGER");
                Console.WriteLine("==========================================");
                Console.WriteLine("Track third-party vendors, risk reviews,");
                Console.WriteLine("review status, due dates, notes, and risk metrics.");
                Console.WriteLine();
                Console.WriteLine("1. Add vendor");
                Console.WriteLine("2. View vendors");
                Console.WriteLine("3. Create vendor risk review");
                Console.WriteLine("4. View all reviews");
                Console.WriteLine("5. View reviews due soon");
                Console.WriteLine("6. Update review status");
                Console.WriteLine("7. Add review note");
                Console.WriteLine("8. View review notes");
                Console.WriteLine("9. View risk dashboard");
                Console.WriteLine("10. Exit");
                Console.Write("\nChoose an option 1 through 10: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    manager.AddVendor();
                }
                else if (choice == "2")
                {
                    manager.ViewVendors();
                }
                else if (choice == "3")
                {
                    manager.CreateVendorReview();
                }
                else if (choice == "4")
                {
                    manager.ViewAllReviews();
                }
                else if (choice == "5")
                {
                    manager.ViewReviewsDueSoon();
                }
                else if (choice == "6")
                {
                    manager.UpdateReviewStatus();
                }
                else if (choice == "7")
                {
                    manager.AddReviewNote();
                }
                else if (choice == "8")
                {
                    manager.ViewReviewNotes();
                }
                else if (choice == "9")
                {
                    manager.ViewRiskDashboard();
                }
                else if (choice == "10")
                {
                    running = false;
                    Console.WriteLine("Exiting Vendor Risk Review Manager.");
                }
                else
                {
                    Console.WriteLine("Invalid option. Please choose 1 through 10.");
                }
            }
        }
    }
}