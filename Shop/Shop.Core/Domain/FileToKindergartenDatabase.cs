namespace Shop.Core.Domain
{
    public class FileToKindergartenDatabase
    {
        public Guid Id { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }
        public Guid? KindergartenId { get; set; }
    }
}
