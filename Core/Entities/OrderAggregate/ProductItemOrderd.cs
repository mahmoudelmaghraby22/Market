namespace Core.Entities.OrderAggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(int productItedId, string productName, string pictureUrl)
        {
            ProductItemdId = productItedId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductItemdId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}