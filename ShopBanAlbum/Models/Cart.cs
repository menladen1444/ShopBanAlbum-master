using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanAlbum.Models
{
    public class CartItem
    {
        public Album _shopping_product { get; set; }
        public int _shopping_quantity { get; set; }
    }
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }
        public void Add(Album _pro, int _quantity = 1)
        {
            var item = items.FirstOrDefault(s => s._shopping_product.AlbumID == _pro.AlbumID);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    _shopping_product = _pro,
                    _shopping_quantity = _quantity
                });
            }
            else
            {
                item._shopping_quantity += _quantity;
            }
        }
        public void Update_Quantity_Shopping(int id, int _quantity)
        {
            var item = items.Find(s => s._shopping_product.AlbumID == id);
            if (item != null)
            {
                item._shopping_quantity = _quantity;
            }
        }
        public double Total_Money()
        {
            if (items.Count() != 0)
            {
                var total = items.Sum(s => s._shopping_product.GiaBan * s._shopping_quantity);
                return (double)total;
            }
            return 0;

        }
        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s => s._shopping_product.AlbumID == id);
        }
        public int Total_Quantity_in_Cart()
        {
            return items.Sum(s => s._shopping_quantity);
        }
        public void ClearCart()
        {
            items.Clear();
        }

    }
}