namespace Shop.Core.Dto
{
    public class FileToKindergartenDatabaseDto
    {
        public Guid Id { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }
        public Guid? KindergartenId { get; set; }
    }
}
