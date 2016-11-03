using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrojanWebRebuild.Models;

namespace TrojanWebRebuild.Logic
{
    public class VirusDescriptionActions : IDisposable
    {
        public string VirusDescriptionID { get; set; }

        private TrojanContext _db = new TrojanContext();

        public const string DescriptionSessionKey = "VirusId";
        public void AddToVirus(int id)
        {
            // Retrieve the Attribute from the database.           
            VirusDescriptionID = GetVirusId();

            var virItem = _db.Virus_Item.SingleOrDefault(
                c => c.VirusId == VirusDescriptionID
                && c.AttributeId == id);
            if (virItem == null)
            {
                //If the selected Attribute is not already included in the description
                //Add it and activate it's On_Off Status                 
                virItem = new Virus_Item
                {
                    ItemId = Guid.NewGuid().ToString(),
                    AttributeId = id,
                    VirusId = VirusDescriptionID,
                    Attribute = _db.Attributes.SingleOrDefault(
                     p => p.AttributeId == id),
                    On_Off = true,
                    DateCreated = DateTime.Now,
                    userAdded = true
                };

                _db.Virus_Item.Add(virItem);
            }
            else
            {
                //If the seleted Attribute is already included in the description
                //Do nothing
                //virItem.On_Off = false;
            }
            _db.SaveChanges();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }

        public string GetVirusId()
        {
            if (HttpContext.Current.Session[DescriptionSessionKey] == null)
            {
                //If User is signed in use their username as id
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[DescriptionSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class.     
                    Guid tempDescriptionId = Guid.NewGuid();
                    HttpContext.Current.Session[DescriptionSessionKey] = tempDescriptionId.ToString();
                }
                // Generate a new random GUID using System.Guid class.     
                //Guid tempDescriptionId = Guid.NewGuid();
                //HttpContext.Current.Session[DescriptionSessionKey] = tempDescriptionId.ToString();
            }
            return HttpContext.Current.Session[DescriptionSessionKey].ToString();
        }
        public void setNewVirusId()
        {
            Guid tempDescriptionId = Guid.NewGuid();
            HttpContext.Current.Session[DescriptionSessionKey] = tempDescriptionId.ToString();
        }
        public List<Virus_Item> GetDescriptionItems()
        {
            VirusDescriptionID = GetVirusId();

            return _db.Virus_Item.Where(
                c => c.VirusId == VirusDescriptionID).ToList();
        }
        public bool Get_OnOff(string cartID, int AttributeId)
        {
            using (var _db = new TrojanWebRebuild.Models.TrojanContext())
            {
                try
                {
                    var myItem = (from c in _db.Virus_Item where c.VirusId == cartID && c.Attribute.AttributeId == AttributeId select c).FirstOrDefault();
                    return myItem.On_Off;
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to query DB - " + exp.Message.ToString(), exp);
                }
            }
        }
        public int GetTotal()
        {
            VirusDescriptionID = GetVirusId();
            // Multiply Attribute price by quantity of that Attribute to get        
            // the current price for each of those Attributes in the cart.  
            // Sum all Attribute price totals to get the cart total.   
            int? total = 0;
            total = (int?)(from virusItems in _db.Virus_Item where virusItems.VirusId == VirusDescriptionID select (int?)(virusItems.On_Off == true ? 1 : 1)).Sum();
            return total ?? 0;
        }
        public int getTotalF_in()
        {
            VirusDescriptionID = GetVirusId();
            // Multiply Attribute price by quantity of that Attribute to get        
            // the current price for each of those Attributes in the cart.  
            // Sum all Attribute price totals to get the cart total.   
            int? total = 0;
            total = (int?)(from virusItems in _db.Virus_Item where virusItems.VirusId == VirusDescriptionID select (int?)(virusItems.On_Off == true ? virusItems.Attribute.F_in : 0)).Sum();
            return total ?? 0;
        }
        public int getTotalF_out()
        {
            VirusDescriptionID = GetVirusId();
            // Multiply Attribute price by quantity of that Attribute to get        
            // the current price for each of those Attributes in the cart.  
            // Sum all Attribute price totals to get the cart total.   
            int? total = 0;
            total = (int?)(from virusItems in _db.Virus_Item where virusItems.VirusId == VirusDescriptionID select (int?)(virusItems.On_Off == true ? virusItems.Attribute.F_out : 0)).Sum();
            return total ?? 0;
        }
        public int GetCount()
        {
            VirusDescriptionID = GetVirusId();

            // Get the count of each item in the cart and sum them up
            int? count = 0;
            count = (int?)(from virusItems in _db.Virus_Item where ((virusItems.VirusId == VirusDescriptionID) && (virusItems.userAdded == true)) select (int?)(virusItems.AttributeId)).Count();

            // Return 0 if all entries are null         
            return count ?? 0;
        }
        public int getAllCount()
        {
            VirusDescriptionID = GetVirusId();

            // Get the count of each item in the cart and sum them up
            int? count = 0;
            count = (int?)(from virusItems in _db.Virus_Item where ((virusItems.VirusId == VirusDescriptionID)) select (int?)(virusItems.AttributeId)).Count();

            // Return 0 if all entries are null         
            return count ?? 0;
        }
        public int GetOnCount()
        {
            VirusDescriptionID = GetVirusId();

            // Get the count of each item in the cart and sum them up
            int? count = 0;
            count = (int?)(from virusItems in _db.Virus_Item where ((virusItems.VirusId == VirusDescriptionID) && (virusItems.On_Off == true) && (virusItems.userAdded == true)) select (int?)(virusItems.AttributeId)).Count();

            // Return 0 if all entries are null         
            return count ?? 0;
        }
        public VirusDescriptionActions GetCart(HttpContext context)
        {
            using (var virus = new VirusDescriptionActions())
            {
                virus.VirusDescriptionID = virus.GetVirusId();
                return virus;
            }
        }

        public void UpdateVirusDescriptionDatabase(String cartId, VirusDescriptionUpdates[] CartItemUpdates)
        {
            using (var db = new TrojanWebRebuild.Models.TrojanContext())
            {
                try
                {
                    int CartItemCount = CartItemUpdates.Count();
                    List<Virus_Item> myVirus = GetDescriptionItems();
                    foreach (var virusItem in myVirus)
                    {
                        // Iterate through all rows within shopping cart list
                        for (int i = 0; i < CartItemCount; i++)
                        {
                            if (virusItem.Attribute.AttributeId == CartItemUpdates[i].AttributeId)
                            {
                                if (CartItemUpdates[i].RemoveItem == true)
                                {
                                    RemoveItem(cartId, virusItem.AttributeId);
                                }
                                else
                                {
                                    UpdateItem(cartId, virusItem.AttributeId, CartItemUpdates[i].OnOff);
                                }
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to update Description Database - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void RemoveItem(string removeCartID, int removeAttributeId)
        {
            using (var _db = new TrojanWebRebuild.Models.TrojanContext())
            {
                try
                {
                    var myItem = (from c in _db.Virus_Item where c.VirusId == removeCartID && c.Attribute.AttributeId == removeAttributeId select c).FirstOrDefault();
                    if (myItem != null)
                    {
                        // Remove Item.
                        _db.Virus_Item.Remove(myItem);
                        _db.SaveChanges();
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to Remove Description Item - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void UpdateItem(string updateCartID, int updateAttributeId, bool OnOff)
        {
            using (var _db = new TrojanWebRebuild.Models.TrojanContext())
            {
                try
                {
                    var myItem = (from c in _db.Virus_Item where c.VirusId == updateCartID && c.Attribute.AttributeId == updateAttributeId select c).FirstOrDefault();
                    if (myItem != null)
                    {
                        myItem.On_Off = OnOff;
                        _db.SaveChanges();
                    }
                }
                catch (Exception exp)
                {
                    throw new Exception("ERROR: Unable to update decription - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void EmptyVirus()
        {
            VirusDescriptionID = GetVirusId();
            var cartItems = _db.Virus_Item.Where(c => c.VirusId == VirusDescriptionID);
            foreach (var cartItem in cartItems)
            {
                _db.Virus_Item.Remove(cartItem);
            }


            var cons = _db.Connections.Where(c => c.VirusId == VirusDescriptionID);
            foreach (var cartItem in cons)
            {
                _db.Connections.Remove(cartItem);
            }

            // Save changes.             
            _db.SaveChanges();
        }
        public void CleanVirus()
        {
            VirusDescriptionID = GetVirusId();
            var cartItems = _db.Virus_Item.Where(c => (c.VirusId == VirusDescriptionID) && (c.userAdded == false));
            foreach (var cartItem in cartItems)
            {
                _db.Virus_Item.Remove(cartItem);
            }
            var cons = _db.Connections.Where(c => c.VirusId == VirusDescriptionID);
            foreach (var cartItem in cons)
            {
                _db.Connections.Remove(cartItem);
            }

            // Save changes.             
            _db.SaveChanges();
        }

        public struct VirusDescriptionUpdates
        {
            public int AttributeId;
            public bool OnOff;
            public bool RemoveItem;
        }
        public void MigrateCart(string cartId, string userName)
        {
            var shoppingCart = _db.Virus_Item.Where(c => c.VirusId == cartId);
            foreach (Virus_Item item in shoppingCart)
            {
                item.VirusId = userName;
            }
            HttpContext.Current.Session[DescriptionSessionKey] = userName;
            _db.SaveChanges();
        }
    }
}