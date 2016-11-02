using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TrojanWebRebuild.Models;
using System.Web.ModelBinding;
using System.Web.Routing;
using TrojanWebRebuild.Logic;
namespace TrojanWebRebuild.Application.Categorization
{
    public static class ListViewExtensions
    {
        public static List<DataKey> GetSelectedDataKeys(this ListView control, string checkBoxId)
        {
            return control.Items.Where(x => IsChecked(x, checkBoxId))
               .Select(x => control.DataKeys[x.DisplayIndex])
               .ToList();
        }
        private static bool IsChecked(ListViewDataItem item, string checkBoxId)
        {
            var control = item.FindControl(checkBoxId) as CheckBox;
            if (control == null)
            {
                return false;
            }
            return control.Checked;
        }
    } 
    public partial class AttributeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public IQueryable<TrojanWebRebuild.Models.Attribute> GetAttributes([QueryString("id")] int? categoryId, [RouteData] string categoryName)
        {
            var _db = new TrojanWebRebuild.Models.TrojanContext();
            IQueryable<TrojanWebRebuild.Models.Attribute> query = _db.Attributes;

            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            //if (!String.IsNullOrEmpty(categoryName))
            //{
            //    query = query.Where(p =>
            //                        String.Compare(p.Category.CategoryName,
            //                        categoryName) == 0);
            //}
            return query;
        }

        protected void selectAttrs_Btn_Click(object sender, EventArgs e)
        {
            
            var selectedKeys = attributeList.GetSelectedDataKeys("selectedChkBx");
            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                foreach (var X in selectedKeys)
                {
                    usersVirus.AddToVirus(int.Parse(X.Value.ToString()));
                }
            }
            Response.Redirect("VirusDescription.aspx");
        }
    }
}