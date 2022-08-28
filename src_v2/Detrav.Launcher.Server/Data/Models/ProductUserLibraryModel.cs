namespace Detrav.Launcher.Server.Data.Models
{
    public class ProductUserLibraryModel
    {
        public virtual ProductUserModel? User { get; set; }
        public int UserId { get; set; }
        public virtual ProductModel? Product { get; set; }
        public int ProductId { get; set; }

        public bool IsOwner { get; set; }
        public bool IsRequest { get; set; }
        public bool IsWishlist { get; set; }
    }
}
