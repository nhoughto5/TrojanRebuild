using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.ModelBinding;
namespace TrojanWebRebuild.Application.Categorization
{
    public partial class AttributeDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public IQueryable<TrojanWebRebuild.Models.Attribute> GetAttribute([QueryString("AttributeID")] int? AttributeId)
        {
            var _db = new TrojanWebRebuild.Models.TrojanContext();
            IQueryable<TrojanWebRebuild.Models.Attribute> query = _db.Attributes;
            if (AttributeId.HasValue && AttributeId > 0)
            {
                query = query.Where(p => p.AttributeId == AttributeId);
            }
            else
            {
                query = null;
            }
            return query;
        }
    }
}