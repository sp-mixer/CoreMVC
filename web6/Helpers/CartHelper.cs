using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using web6.Models;

namespace web6.Helpers {
    public static class CartHelper {
        private const string CartSessionKey = "Cart";

        // カートを取得
        public static List<CartItem> GetCart(ISession session) {
            var json = session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(json) ?? new List<CartItem>();
        }

        // カートを保存
        public static void SaveCart(ISession session, List<CartItem> cart) {
            var json = JsonConvert.SerializeObject(cart);
            session.SetString(CartSessionKey, json);
        }
    }
}
