namespace DEVTRACKR.API.Entities
{
    public class PackageUpdate
    {
        public PackageUpdate(string status, int packageId){
            PackageId = packageId;
            Status = status;
            UpdateDate = DateTime.Now;
        }

        public int Id { get; private set; }
        
        public int PackageId { get; set; }
        
        public string Status { get; set; }
        
        public DateTime UpdateDate { get; set; }
        
        
    }
}