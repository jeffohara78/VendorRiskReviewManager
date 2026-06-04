namespace VendorRiskReviewManager
{
    public class Vendor
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string ServiceProvided { get; set; }
        public string ContactEmail { get; set; }

        public Vendor()
        {
        }

        public Vendor(int vendorId, string vendorName, string serviceProvided, string contactEmail)
        {
            VendorId = vendorId;
            VendorName = vendorName;
            ServiceProvided = serviceProvided;
            ContactEmail = contactEmail;
        }
    }
}