using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.ModelBinding;
using TrojanWebRebuild.Models;
namespace TrojanWebRebuild
{
    public partial class AttributeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public IQueryable<TrojanWebRebuild.Models.Attribute> GetAttributes([QueryString("id")] int? categoryId)
        {
            var _db = new TrojanWebRebuild.Models.TrojanContext();
            IQueryable<TrojanWebRebuild.Models.Attribute> query = _db.Attributes;
            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }
            return query;
        }
    }
}