namespace thTODO.Api.Model
{
    public class ToDo : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int Status { get; set; }
    }
}
