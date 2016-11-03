using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TrojanWebRebuild.Models;
using TrojanWebRebuild.Logic;
using System.Collections.Specialized;
using System.Collections;
using System.Web.ModelBinding;
using Newtonsoft.Json;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.Entity.Core;
using System.Data.SqlClient;

namespace TrojanWebRebuild
{
    public partial class VirusDescription : System.Web.UI.Page
    {
        bool isBuilt;
        TrojanContext db = new TrojanContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
                {

                    int totalNumberofAttributes = 0;
                    int totalF_in = 0;
                    int totalF_out = 0;
                    totalNumberofAttributes = usersVirus.getAllCount();
                    totalF_in = usersVirus.getTotalF_in();
                    totalF_out = usersVirus.getTotalF_out();
                    //usersVirus.setNewVirusId();
                    if (totalNumberofAttributes > 0)
                    {
                        // Display Total.
                        hideResults();
                        VirusDescriptionTitle.Visible = true;
                        NoSelected.Visible = false;
                        UpdateBtn.Visible = true;
                        BuildCombo.Visible = true;
                        BuildRow.Visible = true;
                        BuildCol.Visible = true;
                        ClearBtn.Visible = true;
                        notes.Visible = false;
                        canNot.Visible = false;
                        ErrorMessage.Visible = false;
                        buttonTable.Visible = true;
                        abstractionEmpty.Visible = false;
                        //lblTotal.Text = String.Format("{0:d}", totalNumberofAttributes);
                        VirusDescriptionTitle.InnerText = "Current Virus Total";
                        //if (totalF_in > 0)
                        //{
                        //    lblTotalF_in.Text = String.Format("{0:d}", totalF_in);
                        //}
                        //else
                        //{
                        //    lblTotalF_in.Text = "0";
                        //}
                        //if (totalF_out > 0)
                        //{
                        //    lblTotalF_out.Text = String.Format("{0:d}", totalF_out);
                        //}
                        //else
                        //{
                        //    lblTotalF_out.Text = "0";
                        //}
                    }
                    else
                    {
                        noneSelected();
                    }
                }
            }
        }
        private bool getBuiltStatus()
        {
            return isBuilt;
        }
        private void setBuilt(bool V)
        {
            isBuilt = V;
        }
        public List<Virus_Item> GetVirusDescription()
        {
            VirusDescriptionActions actions = new VirusDescriptionActions();
            return actions.GetDescriptionItems();
        }

        public List<Virus_Item> UpdateCartItems()
        {
            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                String virusId = usersVirus.GetVirusId();
                int total = usersVirus.GetCount();
                if (total > 0)
                {
                    VirusDescriptionActions.VirusDescriptionUpdates[] cartUpdates = new VirusDescriptionActions.VirusDescriptionUpdates[DescriptionList.Rows.Count];
                    for (int i = 0; i < DescriptionList.Rows.Count; i++)
                    {
                        IOrderedDictionary rowValues = new OrderedDictionary();
                        rowValues = GetValues(DescriptionList.Rows[i]);
                        cartUpdates[i].AttributeId = Convert.ToInt32(rowValues["AttributeId"]);

                        CheckBox cbRemove = new CheckBox();
                        cbRemove = (CheckBox)DescriptionList.Rows[i].FindControl("Remove");
                        cartUpdates[i].RemoveItem = cbRemove.Checked;

                        CheckBox cbOnOff = new CheckBox();
                        cbOnOff = (CheckBox)DescriptionList.Rows[i].FindControl("On_Off_CheckBox");
                        if (cbOnOff.Checked == true) //Check to see if On/off is checked
                        {
                            if (usersVirus.Get_OnOff(virusId, cartUpdates[i].AttributeId) == true) //If checked and currently on, turn off
                            {
                                cartUpdates[i].OnOff = false;
                            }
                            else //If checked and currently off, turn on
                            {
                                cartUpdates[i].OnOff = true;
                            }
                            //cartUpdates[i].OnOff = cbOnOff.Checked;
                        }
                        else //if not checked, query DB for previous state
                        {
                            cartUpdates[i].OnOff = usersVirus.Get_OnOff(virusId, cartUpdates[i].AttributeId);
                        }


                    }
                    usersVirus.UpdateVirusDescriptionDatabase(virusId, cartUpdates);
                    DescriptionList.DataBind();
                    total = usersVirus.GetCount();
                    if (total == 0)
                    {
                        //noneSelected();
                        return usersVirus.GetDescriptionItems();
                    }
                    else
                    {
                        //lblTotal.Text = String.Format("{0:d}", usersVirus.GetCount());
                        //lblTotalF_in.Text = String.Format("{0:d}", usersVirus.getTotalF_in());
                        //lblTotalF_out.Text = String.Format("{0:d}", usersVirus.getTotalF_out());
                        return usersVirus.GetDescriptionItems();
                    }
                }
                else
                {
                    noneSelected();
                    return usersVirus.GetDescriptionItems();
                }
            }
        }

        public static IOrderedDictionary GetValues(GridViewRow row)
        {
            IOrderedDictionary values = new OrderedDictionary();
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.Visible)
                {
                    // Extract values from the cell.
                    cell.ContainingField.ExtractValuesFromCell(values, cell, row.RowState, true);
                }
            }
            return values;
        }

        //query the db for a single attribute
        private TrojanWebRebuild.Models.Attribute getAttribute(int ID)
        {
            return db.Attributes.Where(b => b.AttributeId == ID).FirstOrDefault();
        }
        private TrojanWebRebuild.Models.Matrix_Cell getCell(int row, int col)
        {
            return db.Matrix_Cell.Where(b => (b.RowId == row) && (b.ColumnId == col)).FirstOrDefault();
        }

        private TrojanWebRebuild.Models.Category getCategory(int ID)
        {
            return db.Categories.Where(b => b.CategoryId == ID).FirstOrDefault();
        }
        private TrojanWebRebuild.Models.Category getCategoryFromAttr(int attr_ID)
        {
            int ID = getAttribute(attr_ID).CategoryId;
            return db.Categories.Where(b => b.CategoryId == ID).FirstOrDefault();
        }
        private void CategoryCheck(string virusId)
        {
            List<Virus_Item> V_Items = new List<Virus_Item>();
            try
            {
                var y = from b in db.Virus_Item where (b.VirusId == virusId) select b;
                V_Items = y.ToList().OrderBy(p => p.AttributeId).ToList();
            }
            catch (Exception exp)
            {
                throw new Exception("ERROR: Fail on Category Check", exp);
            }
            foreach (Virus_Item X in V_Items)
            {
                if (X.Category == null)
                {
                    X.Category = getCategoryFromAttr(X.AttributeId);
                }
            }
            db.SaveChanges();
        }

        //Scan the row and return all column ID's with value 1
        private List<Matrix_Cell> scanRowTrue(int rowNum, string subM)
        {
            //Scan row in specific subMatrix
            if (subM != null)
            {
                var X = from b in db.Matrix_Cell where (b.RowId == rowNum) && (b.submatrix == subM) && (b.value == true) select b;
                return X.ToList();
            }
            //Scan entire row
            else
            {
                var X = from b in db.Matrix_Cell where (b.RowId == rowNum) && (b.value == true) select b;
                return X.ToList();
            }

        }

        //Return all of the matrix cells in a particulat column that are in a particular submatrix
        private List<Matrix_Cell> scanColumnTrue(int Id, string subM)
        {
            if (subM != null)
            {
                var X = from b in db.Matrix_Cell where (b.ColumnId == Id) && (b.submatrix == subM) && (b.value == true) select b;
                return X.ToList();
            }
            else
            {
                var X = from b in db.Matrix_Cell where (b.ColumnId == Id) && (b.value == true) select b;
                return X.ToList();
            }

        }

        //Scan the row and return all column ID's
        private List<Matrix_Cell> getRow(int rowNum, string subM)
        {
            //Scan row in specific subMatrix
            if (subM != null)
            {
                var X = from b in db.Matrix_Cell where (b.RowId == rowNum) && (b.submatrix == subM) select b;
                return X.ToList();
            }
            //Scan entire row
            else
            {
                var X = from b in db.Matrix_Cell where (b.RowId == rowNum) select b;
                return X.ToList();
            }
        }
        private List<Virus_Item> getVirus(string virusId)
        {
            var X = from b in db.Virus_Item where (b.VirusId == virusId) select b;
            return X.ToList().OrderBy(x => x.AttributeId).ToList();
        }
        private void populateDropDown()
        {
            string virusId;
            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                virusId = usersVirus.GetVirusId();
            }
            List<Virus_Item> currentVirus = getVirus(virusId);
            List<int> ids = new List<int>();
            foreach (Virus_Item V in currentVirus)
            {
                ids.Add(V.AttributeId);
            }
            ids.Sort();
            attrDropDownList.DataSource = ids;
            attrDropDownList.DataBind();
        }

        protected void removeAttrBtn_Click(object sender, EventArgs e)
        {
            string virusId;
            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                virusId = usersVirus.GetVirusId();
                List<Virus_Item> virus = getVirus(virusId);
                int remove = Int32.Parse(attrDropDownList.SelectedItem.Text);
                virus = virus.Where(x => x.AttributeId != remove).ToList();
                switch (getAttribute(remove).CategoryName)
                {
                    case "Chip Life Cycle":
                        removeInsertion(virus, getAttribute(remove), virusId);
                        break;
                    case "Abstraction":
                        removeAbstraction(virus, getAttribute(remove), virusId);
                        break;
                    case "Properties":
                        removePropertie(virus, virusId);
                        break;
                    case "Locations":
                        removeLocation(virus, virusId);
                        break;
                    default:
                        selectionNotPossible();
                        break;
                }
            }
        }
        private void removeInsertion(List<Virus_Item> virus, TrojanWebRebuild.Models.Attribute removedAttr, string virusId)
        {
            List<int> insertion = new List<int>();
            var abstraction = new HashSet<int>();
            List<int> properties = new List<int>();
            List<int> location = new List<int>();
            List<Connection> Connections = new List<Connection>();

            foreach (Virus_Item V in virus)
            {
                if (V.Category.CategoryName == "Chip Life Cycle")
                {
                    insertion.Add(V.AttributeId);
                }
                else if (V.Category.CategoryName == "Abstraction")
                {
                    abstraction.Add(V.AttributeId);
                }
                else
                {

                }
                //else if (V.Category.CategoryName == "Properties")
                //{
                //    properties.Add(V.AttributeId);
                //}
                //else
                //{
                //    location.Add(V.AttributeId);
                //}
            }
            //Check if any of the categories is empty
            if (insertion.Count == 0)
            {
                selectionNotPossible();
                return;
            }
            List<Matrix_Cell> tempRow = new List<Matrix_Cell>();
            foreach (int X in insertion)
            {
                tempRow = scanRowTrue(X, null);
                foreach (Matrix_Cell M in tempRow)
                {
                    Connections.Add(new Connection(X, M.ColumnId, directConnectionForwards(M, X), virusId));
                }
            }

            properties = testRowTrue(abstraction.ToList(), "R23");
            location = testRowTrue(properties, "R34");
            if (location.Count == 0 || properties.Count == 0)
            {
                selectionNotPossible();
                return;
            }
            List<int> Nodes = insertion.Concat(location).ToList().Concat(properties).ToList().Concat(abstraction.ToList()).ToList();

            for (int i = 0; i < 11; ++i)
            {
                if (Nodes.Contains(i) && Nodes.Contains(i + 1))
                {
                    if (!connectionCheck(Connections, i, i + 1))
                    {
                        Connections.Add(new Connection(i, i + 1, false, virusId));
                    }
                }
            }
            Nodes = nodeFilter(Nodes, abstraction.Max(), Connections);
            Connections = connectionFilter(Nodes, Connections);
            Nodes.Sort();
            saveRebuildToDB(Nodes, Connections, virusId);
            Visualize(Nodes, Connections, virusId);
        }
        private void removeAbstraction(List<Virus_Item> virus, TrojanWebRebuild.Models.Attribute removedAttr, string virusId)
        {
            var insertion = new HashSet<int>();
            var abstraction = new HashSet<int>();
            List<int> properties = new List<int>();
            List<int> location = new List<int>();
            List<Connection> Connections = new List<Connection>();

            foreach (Virus_Item V in virus)
            {
                if (V.Category.CategoryName == "Chip Life Cycle")
                {
                    insertion.Add(V.AttributeId);
                }
                else if (V.Category.CategoryName == "Abstraction")
                {
                    abstraction.Add(V.AttributeId);
                }
                else
                {

                }
            }
            if (abstraction.Count == 0)
            {
                selectionNotPossible();
                return;
            }
            List<Matrix_Cell> tempCol = new List<Matrix_Cell>();
            foreach (int X in abstraction)
            {
                tempCol = scanColumnTrue(X, null);
                foreach (Matrix_Cell M in tempCol)
                {
                    if (abstraction.Contains(M.RowId) || M.RowId < 6)
                    {
                        Connections.Add(new Connection(M.RowId, X, directConnectionBackwards(M, X), virusId));
                    }
                }
            }
            properties = testRowTrue(abstraction.ToList(), "R23");
            location = testRowTrue(properties, "R34");
            if (location.Count == 0 || properties.Count == 0)
            {
                selectionNotPossible();
                return;
            }
            List<int> Nodes = insertion.Concat(location).ToList().Concat(properties).ToList().Concat(abstraction.ToList()).ToList();
            for (int i = 0; i < 11; ++i)
            {
                if (Nodes.Contains(i) && Nodes.Contains(i + 1))
                {
                    if (!connectionCheck(Connections, i, i + 1))
                    {
                        Connections.Add(new Connection(i, i + 1, false, virusId));
                    }
                }
            }
            Nodes = nodeFilter(Nodes, abstraction.Max(), Connections);
            Connections = connectionFilter(Nodes, Connections);
            Nodes.Sort();
            saveRebuildToDB(Nodes, Connections, virusId);
            Visualize(Nodes, Connections, virusId);
        }
        private void removePropertie(List<Virus_Item> virus, string virusId)
        {
            List<int> properties = new List<int>();
            foreach (Virus_Item V in virus)
            {
                if (V.Category.CategoryName == "Properties")
                {
                    properties.Add(V.AttributeId);
                }
            }
            propertiesOnly(intToAttr(properties), virusId);
        }
        private void removeLocation(List<Virus_Item> virus, string virusId)
        {
            List<int> locations = new List<int>();
            foreach (Virus_Item V in virus)
            {
                if (V.Category.CategoryName == "Location")
                {
                    locations.Add(V.AttributeId);
                }
            }
            locationOnly(intToAttr(locations), virusId);
        }
        //Return all of the matrix cells in a particulat column that are in a particular submatrix
        private List<Matrix_Cell> getColumn(int Id, string subM)
        {
            if (subM != null)
            {
                var X = from b in db.Matrix_Cell where (b.ColumnId == Id) && (b.submatrix == subM) select b;
                return X.ToList();
            }
            else
            {
                var X = from b in db.Matrix_Cell where (b.ColumnId == Id) select b;
                return X.ToList();
            }

        }
        private bool connectionCheck(List<Connection> Connections, int source, int target)
        {
            foreach (Connection C in Connections)
            {
                if (C.source == source && C.target == target)
                {
                    return true;
                }
            }
            return false;
        }
        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            abstractionGrid.Visible = abstractionResults.Visible = false;
            directNone.Visible = direct.Visible = directGrid.Visible = false;
            indirectNone.Visible = indirectGrid.Visible = indirect.Visible = false;
            ColumnGrid.Visible = ColumnResults.Visible = false;
            RowGrid.Visible = RowResults.Visible = false;

            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                usersVirus.CleanVirus();
                string id = usersVirus.GetVirusId();
                UpdateCartItems();
                int total = usersVirus.GetCount();
                if (usersVirus.GetCount() > 0)
                {
                    VirusDescriptionTitle.InnerText = "Current Virus Total";
                }
                else
                {
                    noneSelected();
                    setBuilt(false);
                }
            }
        }

        private void noneSelected()
        {
            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                setBuilt(false);
                usersVirus.EmptyVirus();
                DescriptionList.DataSource = null;
                DescriptionList.DataBind();
                hideResults();
                NoSelected.Visible = true;
                buttonTable.Visible = false;
                UpdateBtn.Visible = false;
                BuildCombo.Visible = false;
                BuildRow.Visible = false;
                BuildCol.Visible = false;
                ClearBtn.Visible = false;
                VirusDescriptionTitle.Visible = false;
                ErrorMessage.Visible = true;
            }
        }

        private void selectionNotPossible()
        {
            noneSelected();
            NoSelected.Visible = false;
            startOverDiv.Visible = true;
            VirusDescriptionTitle.Visible = false;
            buttonTable.Visible = false;
            UpdateBtn.Visible = false;
            BuildCombo.Visible = false;
            BuildRow.Visible = false;
            BuildCol.Visible = false;
            ClearBtn.Visible = false;
            //VisualizeBtn.Visible = false;
            startOverBtn.Visible = true;
        }

        protected void ClearBtn_Click(object sender, EventArgs e)
        {
            using (VirusDescriptionActions W = new VirusDescriptionActions())
            {
                W.setNewVirusId();
            }
            noneSelected();
        }
        private void severity(List<int> attributes)
        {
            Severity sevRating = new Severity(attributes);
            List<SeverityRating> sevList = new List<SeverityRating>();
            SeverityRating rating = sevRating.getSevRating();
            trjnCelliR_lbl.InnerText = rating.iR;
            trjnCelliA_lbl.InnerText = rating.iA;
            trjnCelliE_lbl.InnerText = rating.iE;
            trjnCelliL_lbl.InnerText = rating.iL;
            trjnCelliF_lbl.InnerText = rating.iF;
            trjnCelliC_lbl.InnerText = rating.iC;
            trjnCelliP_lbl.InnerText = rating.iP;
            trjnCelliO_lbl.InnerText = rating.iO;

            trjnCellcR_lbl.InnerText = rating.cR;
            trjnCellcA_lbl.InnerText = rating.cA;
            trjnCellcE_lbl.InnerText = rating.cE;
            trjnCellcL_lbl.InnerText = rating.cL;
            trjnCellcF_lbl.InnerText = rating.cF;
            trjnCellcC_lbl.InnerText = rating.cC;
            trjnCellcP_lbl.InnerText = rating.cP;
            trjnCellcO_lbl.InnerText = rating.cO;

            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                rating.VirusId = usersVirus.GetVirusId();
            }
            db.SeverityRating.Add(rating);
            db.SaveChanges();
        }

        protected void BuildComboBtn_Click(object sender, EventArgs e)
        {
            ColumnGrid.Visible = ColumnResults.Visible = false;
            LocationGrid.Visible = Locationlbl.Visible = false;
            RowGrid.Visible = RowResults.Visible = false;
            notes.Visible = false;

            List<TrojanWebRebuild.Models.Attribute> Attributes = new List<TrojanWebRebuild.Models.Attribute>();
            VirusDescriptionActions.VirusDescriptionUpdates[] currentBuild = new VirusDescriptionActions.VirusDescriptionUpdates[DescriptionList.Rows.Count];
            TrojanWebRebuild.Models.Attribute tempAttr = new Models.Attribute();
            var categorySet = new HashSet<string>();
            string virusId = null;
            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                usersVirus.CleanVirus();
                int total = usersVirus.GetOnCount();
                if (total > 0)
                {
                    virusId = usersVirus.GetVirusId();
                    IOrderedDictionary rowValues = new OrderedDictionary();
                    for (int i = 0; i < DescriptionList.Rows.Count; i++)
                    {
                        rowValues = GetValues(DescriptionList.Rows[i]);
                        currentBuild[i].AttributeId = Convert.ToInt32(rowValues["AttributeId"]);
                        tempAttr = getAttribute(currentBuild[i].AttributeId);
                        //If current attribute is turned on
                        if (usersVirus.Get_OnOff(virusId, currentBuild[i].AttributeId))
                        {
                            categorySet.Add(tempAttr.CategoryName);
                            Attributes.Add(tempAttr);
                        }
                    }
                }
                //Total <= 0
                else
                {
                    setBuilt(false);
                    notes.Visible = true;
                    canNot.Visible = true;
                    abstractionNone.Visible = abstractionNone.Visible = abstractionResults.Visible = abstractionGrid.Visible = false;
                    directNone.Visible = direct.Visible = directGrid.Visible = false;
                    indirectNone.Visible = indirectGrid.Visible = indirect.Visible = false;
                    RowResults.Visible = RowGrid.Visible = false;
                    ColumnResults.Visible = ColumnGrid.Visible = false;
                }

            }
            Attributes = Attribute_Sorting(Attributes);
            hideResults();
            if (categorySet.Contains("Chip Life Cycle") && categorySet.Contains("Abstraction") && categorySet.Contains("Properties") && categorySet.Contains("Location"))
            {
                explicitBuild(Attributes, virusId);
            }
            //#1 Used for IAPL: 0010
            else if (!categorySet.Contains("Chip Life Cycle") && !categorySet.Contains("Abstraction") && categorySet.Contains("Properties") && !categorySet.Contains("Location"))
            {
                propertiesOnly(Attributes, virusId);
            }
            //#2 Used for IAPL: 0100
            else if (!categorySet.Contains("Chip Life Cycle") && categorySet.Contains("Abstraction") && !categorySet.Contains("Properties") && !categorySet.Contains("Location"))
            {
                abstractionOnly(Attributes, virusId);
            }
            //#3 Used for IAPL: 0001
            else if (!categorySet.Contains("Chip Life Cycle") && !categorySet.Contains("Abstraction") && !categorySet.Contains("Properties") && categorySet.Contains("Location"))
            {
                locationOnly(Attributes, virusId);
            }
            //#4 Used for IAPL: 1000
            else if (categorySet.Contains("Chip Life Cycle") && !categorySet.Contains("Abstraction") && !categorySet.Contains("Properties") && !categorySet.Contains("Location"))
            {
                insertionOnly(Attributes, virusId);
            }
            //#5 Used for IAPL: 0101 1100 1101 => ( B . C'. D ) + ( A . B . C')
            else if ((categorySet.Contains("Abstraction") && !categorySet.Contains("Properties") && categorySet.Contains("Location")) || (categorySet.Contains("Chip Life Cycle") && categorySet.Contains("Abstraction") && !categorySet.Contains("Properties")))
            {
                //
                forwardPropagation(Attributes, virusId);
            }
            //# 7 Used for IAPL: 0011 1010 1011 => ( B'. C . D ) + ( A . B'. C )
            else if ((!categorySet.Contains("Abstraction") && categorySet.Contains("Properties") && categorySet.Contains("Location")) || (categorySet.Contains("Chip Life Cycle") && !categorySet.Contains("Abstraction") && categorySet.Contains("Properties")))
            {
                //
                backPropagationNoAbstraction(Attributes, virusId);
            }
            //#8 Used for IAPL: 0110 0111 1110 1111 => ( B . C )
            else if (categorySet.Contains("Abstraction") && categorySet.Contains("Properties"))
            {
                //
                backPropagationNoAbstraction(Attributes, virusId);
            }
            //#9 Used for IAPL: 1001 => ( A . B'. C'. D )
            else if (categorySet.Contains("Chip Life Cycle") && !categorySet.Contains("Abstraction") && !categorySet.Contains("Properties") && categorySet.Contains("Location"))
            {
                splitPropagation(Attributes, virusId);
            }
            //#X Used for IAPL: 0000
            else
            {
                selectionNotPossible();
            }
            populateDropDown();
        }
        protected void BuildRowBtn_Click(object sender, EventArgs e)
        {
            abstractionGrid.Visible = abstractionResults.Visible = false;
            directNone.Visible = direct.Visible = directGrid.Visible = false;
            indirectNone.Visible = indirectGrid.Visible = indirect.Visible = false;
            ConnectionsResults.Visible = false;
            ConnectionsGrid.Visible = false;
            ColumnGrid.Visible = ColumnResults.Visible = false;
            RowGrid.Visible = RowResults.Visible = true;
            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                setBuilt(true);
                int total = usersVirus.GetOnCount();
                if (total > 0)
                {
                    List<int> resultsInt = new List<int>();
                    List<int> userSubmitted = new List<int>();
                    List<TrojanWebRebuild.Models.Attribute> results = new List<TrojanWebRebuild.Models.Attribute>();
                    String virusId = usersVirus.GetVirusId();
                    VirusDescriptionActions.VirusDescriptionUpdates[] currentBuild = new VirusDescriptionActions.VirusDescriptionUpdates[DescriptionList.Rows.Count];
                    for (int i = 0; i < DescriptionList.Rows.Count; i++)
                    {
                        IOrderedDictionary rowValues = new OrderedDictionary();
                        rowValues = GetValues(DescriptionList.Rows[i]);
                        currentBuild[i].AttributeId = Convert.ToInt32(rowValues["AttributeId"]);
                        if (usersVirus.Get_OnOff(virusId, currentBuild[i].AttributeId))
                        {
                            userSubmitted.Add(currentBuild[i].AttributeId);
                        }
                    }
                    resultsInt = testRowTrue(userSubmitted, null);
                    foreach (int X in resultsInt)
                    {
                        results.Add(getAttribute(X));
                    }
                    RowGrid.DataSource = Attribute_Sorting(results);
                    RowGrid.DataBind();
                }
                else
                {
                    notes.Visible = true;
                    canNot.Visible = true;
                    abstractionNone.Visible = abstractionNone.Visible = abstractionResults.Visible = abstractionGrid.Visible = false;
                    directNone.Visible = direct.Visible = directGrid.Visible = false;
                    indirectNone.Visible = indirectGrid.Visible = indirect.Visible = false;
                    RowResults.Visible = RowGrid.Visible = false;
                    ColumnResults.Visible = ColumnGrid.Visible = false;
                }
            }
        }
        protected void BuildColumnBtn_Click(object sender, EventArgs e)
        {

            abstractionGrid.Visible = abstractionResults.Visible = false;
            directNone.Visible = direct.Visible = directGrid.Visible = false;
            indirectNone.Visible = indirectGrid.Visible = indirect.Visible = false;
            ConnectionsResults.Visible = false;
            ConnectionsGrid.Visible = false;
            ColumnGrid.Visible = ColumnResults.Visible = true;
            RowGrid.Visible = RowResults.Visible = false;
            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                setBuilt(true);
                int total = usersVirus.GetOnCount();
                if (total > 0)
                {
                    List<TrojanWebRebuild.Models.Attribute> results = new List<TrojanWebRebuild.Models.Attribute>();
                    List<int> userSubmitted = new List<int>();
                    List<int> resultsInt = new List<int>();
                    String virusId = usersVirus.GetVirusId();
                    VirusDescriptionActions.VirusDescriptionUpdates[] currentBuild = new VirusDescriptionActions.VirusDescriptionUpdates[DescriptionList.Rows.Count];
                    for (int i = 0; i < DescriptionList.Rows.Count; i++)
                    {
                        IOrderedDictionary rowValues = new OrderedDictionary();
                        rowValues = GetValues(DescriptionList.Rows[i]);
                        currentBuild[i].AttributeId = Convert.ToInt32(rowValues["AttributeId"]);

                        if (usersVirus.Get_OnOff(virusId, currentBuild[i].AttributeId))
                        {
                            userSubmitted.Add(currentBuild[i].AttributeId);
                        }
                    }
                    resultsInt = testColumnTrue(userSubmitted, null);
                    foreach (int X in resultsInt)
                    {
                        results.Add(getAttribute(X));
                    }
                    ColumnGrid.DataSource = Attribute_Sorting(results);
                    ColumnGrid.DataBind();
                }
                else
                {
                    notes.Visible = true;
                    canNot.Visible = true;
                    abstractionNone.Visible = abstractionNone.Visible = abstractionResults.Visible = abstractionGrid.Visible = false;
                    directNone.Visible = direct.Visible = directGrid.Visible = false;
                    indirectNone.Visible = indirectGrid.Visible = indirect.Visible = false;
                    RowResults.Visible = RowGrid.Visible = false;
                    ColumnResults.Visible = ColumnGrid.Visible = false;
                }
            }
        }
        protected List<Connection> Connection_Sorting(List<Connection> List)
        {
            List<Connection> SortedList = List.OrderBy(p => p.source).ThenBy(c => c.target).ToList();
            return SortedList;
        }
        private void explicitBuild(List<TrojanWebRebuild.Models.Attribute> attributesList, string virusId)
        {
            List<int> locations = new List<int>();
            List<int> insert = new List<int>();
            List<int> abstraction = new List<int>();
            List<int> properties = new List<int>();

            foreach (TrojanWebRebuild.Models.Attribute A in attributesList)
            {
                if (A.CategoryName == "Location")
                {
                    locations.Add(A.AttributeId);
                }
                else if (A.CategoryName == "Abstraction")
                {
                    abstraction.Add(A.AttributeId);
                }
                else if (A.CategoryName == "Properties")
                {
                    properties.Add(A.AttributeId);
                }
                else
                {
                    insert.Add(A.AttributeId);
                }
            }
            List<int> attr = new List<int>(), insertAbs = new List<int>();
            insertAbs = insert.Concat(abstraction).ToList();
            foreach(TrojanWebRebuild.Models.Attribute a in attributesList){
                attr.Add(a.AttributeId);
            }
            List<Connection> Connections = new List<Connection>();
            attr.Sort((a, b) => a.CompareTo(b));
            int maxAttr = abstraction.Max();
            foreach (int i in insertAbs)
            {
                for (int j = i; j <= maxAttr; ++j)
                {
                    if(doesForwardConnectionExist(i, j)){
                        Connections.Add(new Connection(i, j, directConnectionForwards(i, j), virusId));
                    }
                }
            }
            attr = nodeFilter(attr, abstraction.Max(), Connections);
            Connections = connectionFilter(attr, Connections);
            saveToDB(attr, Connections, virusId);
            Visualize(attr, Connections, virusId);
            severity(attr);
            //insert.Sort((a, b) => a.CompareTo(b)); //Ascending
            //locations.Sort((a, b) => a.CompareTo(b)); //Ascending
            //abstraction.Sort((a, b) => a.CompareTo(b)); //Ascending
            //properties.Sort((a, b) => a.CompareTo(b)); //Ascending
            
        }
        private void propertiesOnly(List<TrojanWebRebuild.Models.Attribute> PropertiesList, string virusId)
        {
            List<int> propertyIDs = attrToInt(PropertiesList);
            List<int> LocationIDs = testRowTrue(propertyIDs, "R34");
            List<int> abstractionResults = testColumnTrue(propertyIDs, "R23");
            if (abstractionResults.Count == 0) selectionNotPossible();
            else
            {
                List<Connection> Connections = new List<Connection>();
                List<Matrix_Cell> tempCol = new List<Matrix_Cell>();
                var nodeSet = new HashSet<int>();
                bool tempDirect = false;
                int maxAbstraction = abstractionResults.Max();

                //Adds nodes in Abstraction and Insertion Category
                for (int i = maxAbstraction; i > 0; --i)
                {
                    tempCol = scanColumnTrue(i, null);
                    nodeSet.Add(i);
                    foreach (Matrix_Cell X in tempCol)
                    {
                        tempDirect = directConnectionBackwards(X, i);
                        Connections.Add(new Connection(X.RowId, i, tempDirect, virusId));
                    }
                }
                List<int> Nodes = nodeSet.ToList().Concat(LocationIDs).ToList().Concat(propertyIDs).ToList();
                Nodes = nodeFilter(Nodes, abstractionResults.Max(), Connections);
                Connections = connectionFilter(Nodes, Connections);
                Nodes.Sort();
                saveToDB(Nodes, Connections, virusId);
                Visualize(Nodes, Connections, virusId);
                severity(Nodes);
            }

        }

        private void abstractionOnly(List<TrojanWebRebuild.Models.Attribute> userChosen, string virusId)
        {
            List<int> abstraction = attrToInt(userChosen);
            List<int> properties = testRowTrue(abstraction, "R23");
            List<int> locations = testRowTrue(properties, "R34");
            if ((abstraction.Count == 0) || (properties.Count == 0) || (locations.Count == 0))
            {
                selectionNotPossible();
                return;
            }
            else
            {
                List<Connection> Connections = new List<Connection>();
                List<Matrix_Cell> tempCol = new List<Matrix_Cell>();
                var nodeSet = new HashSet<int>();
                bool tempDirect = false;
                int maxAbstraction = abstraction.Max();

                //Adds nodes in Abstraction and Insertion Category
                for (int i = maxAbstraction; i > 0; --i)
                {
                    tempCol = scanColumnTrue(i, null);
                    nodeSet.Add(i);
                    foreach (Matrix_Cell X in tempCol)
                    {
                        tempDirect = directConnectionBackwards(X, i);
                        Connections.Add(new Connection(X.RowId, i, tempDirect, virusId));
                    }
                }
                List<int> Nodes = nodeSet.ToList().Concat(locations).ToList().Concat(properties).ToList();
                Nodes = nodeFilter(Nodes, abstraction.Max(), Connections);
                Connections = connectionFilter(Nodes, Connections);
                Nodes.Sort();
                saveToDB(Nodes, Connections, virusId);
                Visualize(Nodes, Connections, virusId);
                severity(Nodes);
            }
        }
        private void insertionOnly(List<TrojanWebRebuild.Models.Attribute> userChosen, string virusId)
        {
            List<int> insertion = attrToInt(userChosen);
            var currentAttr = insertion.Min();
            List<Matrix_Cell> tempRow = new List<Matrix_Cell>();
            var nodeSet = new HashSet<int>();
            var absSet = new HashSet<int>();
            List<Connection> Connections = new List<Connection>();

            //Scan through insertion attributes
            for (int i = currentAttr; i < 6; ++i)
            {
                tempRow = scanRowTrue(i, null);
                nodeSet.Add(i);
                foreach (Matrix_Cell M in tempRow)
                {

                    if (M.submatrix == "R12")
                    {
                        absSet.Add(M.ColumnId);
                    }
                    Connections.Add(new Connection(i, M.ColumnId, directConnectionForwards(M, i), virusId));
                }
            }
            int highestAbs = absSet.Max();

            //Scan through abstraction attributes
            for (int i = 6; i <= highestAbs; ++i)
            {
                nodeSet.Add(i); absSet.Add(i);
                tempRow = scanRowTrue(i, "R2");
                foreach (Matrix_Cell M in tempRow)
                {
                    Connections.Add(new Connection(i, M.ColumnId, directConnectionForwards(M, i), virusId));
                }
            }
            List<int> properties = testRowTrue(absSet.ToList(), "R23");
            List<int> locations = testRowTrue(properties, "R34");
            List<int> Nodes = nodeSet.ToList().Concat(locations).ToList().Concat(properties).ToList();
            Nodes = nodeFilter(Nodes, highestAbs, Connections);
            Connections = connectionFilter(Nodes, Connections);
            Nodes.Sort();
            saveToDB(Nodes, Connections, virusId);
            Visualize(Nodes, Connections, virusId);
            severity(Nodes);
        }
        private void locationOnly(List<TrojanWebRebuild.Models.Attribute> userChosen, string virusId)
        {
            List<int> locations = attrToInt(userChosen);
            List<int> properties = testColumnTrue(locations, "R34");
            List<int> abstraction = testColumnTrue(properties, "R23");
            if ((locations.Count == 0) || (properties.Count == 0) || (abstraction.Count == 0))
            {
                selectionNotPossible();
                return;
            }
            else
            {
                List<Connection> Connections = new List<Connection>();
                List<Matrix_Cell> tempCol = new List<Matrix_Cell>();
                var nodeSet = new HashSet<int>();
                bool tempDirect = false;
                int maxAbstraction = abstraction.Max();

                //Adds nodes in Abstraction and Insertion Category
                for (int i = maxAbstraction; i > 0; --i)
                {
                    tempCol = scanColumnTrue(i, null);
                    nodeSet.Add(i);
                    foreach (Matrix_Cell X in tempCol)
                    {
                        tempDirect = directConnectionBackwards(X, i);
                        Connections.Add(new Connection(X.RowId, i, tempDirect, virusId));
                    }
                }
                List<int> Nodes = nodeSet.ToList().Concat(locations).ToList().Concat(properties).ToList();
                Nodes = nodeFilter(Nodes, abstraction.Max(), Connections);
                Connections = connectionFilter(Nodes, Connections);
                Nodes.Sort();
                saveToDB(Nodes, Connections, virusId);
                Visualize(Nodes, Connections, virusId);
                severity(Nodes);
            }

        }
        private bool virusItemCheck(List<Virus_Item> items, int X)
        {
            foreach (Virus_Item V in items)
            {
                if (V.AttributeId == X)
                {
                    return true;
                }
            }
            return false;
        }
        private void saveRebuildToDB(List<int> NodeInt, List<Connection> Edges, string virusId)
        {
            List<int> currentList = new List<int>();
            var nodeSet = new HashSet<int>(NodeInt);
            List<Virus_Item> userItems = (from c in db.Virus_Item where ((c.VirusId == virusId) && (c.userAdded == true)) select c).ToList();
            List<Virus_Item> newSave = new List<Virus_Item>();
            Models.Attribute tempAttr = new Models.Attribute();

            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                usersVirus.EmptyVirus();
            }

            foreach (int X in NodeInt)
            {
                tempAttr = getAttribute(X);
                if (virusItemCheck(userItems, X))
                {
                    db.Virus_Item.Add(new Virus_Item(virusId, X, tempAttr, getCategoryFromAttr(X), true));
                }
                else
                {
                    db.Virus_Item.Add(new Virus_Item(virusId, X, tempAttr, getCategoryFromAttr(X), false));
                }
            }
            foreach (Connection Q in Edges)
            {
                db.Connections.Add(Q);
            }
            db.SaveChanges();
            populateDropDown();
        }

        private void saveToDB(List<int> NodeInt, List<Connection> Edges, string virusId)
        {
            List<int> currentList = new List<int>();
            var nodeSet = new HashSet<int>(NodeInt);
            List<Virus_Item> userItems = (from c in db.Virus_Item where (c.VirusId == virusId) select c).ToList();
            foreach (Virus_Item V in userItems)
            {
                nodeSet.Add(V.AttributeId);
                if (V.userAdded == true)
                {
                    currentList.Add(V.AttributeId);
                }

            }
            List<Models.Attribute> Nodes = intToAttr(nodeSet.ToList());
            using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
            {
                usersVirus.EmptyVirus();
            }
            foreach (Connection Q in Edges)
            {
                db.Connections.Add(Q);
            }
            foreach (Models.Attribute N in Nodes)
            {
                if (currentList.Contains(N.AttributeId))
                {
                    db.Virus_Item.Add(new Virus_Item(virusId, N.AttributeId, N, getCategoryFromAttr(N.AttributeId), true));
                }
                else
                {
                    db.Virus_Item.Add(new Virus_Item(virusId, N.AttributeId, N, getCategoryFromAttr(N.AttributeId), false));
                }
            }
            db.SaveChanges();
        }
        //Gets a RowId and checks if it is connected to the col
        private bool doesForwardConnectionExist(int rowId, int colId)
        {
            List<Matrix_Cell> conns = scanRowTrue(rowId, null);
            foreach(Matrix_Cell conn in conns){
                if(colId == conn.ColumnId){
                    return true;
                }
            }
            return false;
        }
        //Determines if a rowId is a direct connection
        //to the current Matrix_Cell when doing backwards propagation 
        private bool directConnectionBackwards(Matrix_Cell X, int i)
        {
            if (Math.Abs(i - X.RowId) == 1) return false;
            else return true;
        }
        //Determines if a colId is a direct connection
        //to the current Matrix_Cell when doing forwards propagation 
        private bool directConnectionForwards(Matrix_Cell X, int i)
        {
            if (Math.Abs(i - X.ColumnId) == 1) return false;
            else return true;
        }
        private bool directConnectionForwards(int X, int i)
        {
            if (Math.Abs(i - X) == 1) return false;
            else return true;
        }

        private void backPropagationWithPropertiesLocationAbstraction(List<TrojanWebRebuild.Models.Attribute> userChosen, string virusId)
        {
            List<int> properties = new List<int>();
            List<int> locations = new List<int>();
            List<int> abstraction = new List<int>();
            foreach (TrojanWebRebuild.Models.Attribute A in userChosen)
            {
                if (A.CategoryName == "Properties")
                {
                    properties.Add(A.AttributeId);
                }
                else if (A.CategoryName == "Abstraction")
                {
                    abstraction.Add(A.AttributeId);
                }
                else
                {
                    locations.Add(A.AttributeId);
                }
            }
            List<int> scannedLocations = testRowTrue(properties, "R34");
            List<int> scannedAbstraction = testColumnTrue(properties, "R23");
            if (abstraction.Count == 0)
            {
                abstraction = scannedAbstraction;
            }
            if (!scannedAttributesCheck(scannedLocations, locations) || !scannedAttributesCheck(scannedAbstraction, abstraction))
            {
                selectionNotPossible();
                return;
            }
            else
            {
                List<Connection> Connections = new List<Connection>();
                List<Matrix_Cell> tempCol = new List<Matrix_Cell>();
                var nodeSet = new HashSet<int>();
                bool tempDirect = false;
                int maxAbstraction = abstraction.Max();

                //Adds nodes in Abstraction and Insertion Category
                for (int i = maxAbstraction; i > 0; --i)
                {
                    tempCol = scanColumnTrue(i, null);
                    nodeSet.Add(i);
                    foreach (Matrix_Cell X in tempCol)
                    {
                        tempDirect = directConnectionBackwards(X, i);
                        Connections.Add(new Connection(X.RowId, i, tempDirect, virusId));
                    }
                }
                List<int> Nodes = nodeSet.ToList().Concat(scannedLocations).ToList().Concat(properties).ToList();
                Nodes.Sort();
                displayResults(Nodes, Connections);
                saveToDB(Nodes, Connections, virusId);
            }

        }

        private void backPropagationNoAbstraction(List<TrojanWebRebuild.Models.Attribute> userChosen, string virusId)
        {
            List<int> properties = new List<int>();
            List<int> locations = new List<int>();
            List<int> insert = new List<int>();
            List<int> abstraction = new List<int>();

            foreach (TrojanWebRebuild.Models.Attribute A in userChosen)
            {
                if (A.CategoryName == "Properties")
                {
                    properties.Add(A.AttributeId);
                }
                else if (A.CategoryName == "Location")
                {
                    locations.Add(A.AttributeId);
                }
                else if (A.CategoryName == "Abstraction")
                {
                    abstraction.Add(A.AttributeId);
                }
                else
                {
                    insert.Add(A.AttributeId);
                }
            }
            List<int> scannedAbstraction = testColumnTrue(properties, "R23");
            if (abstraction.Count == 0)
            {
                abstraction = scannedAbstraction;
            }
            if (!scannedAttributesCheck(scannedAbstraction, abstraction))
            {
                selectionNotPossible();
                return;
            }
            else
            {
                List<int> scannedLocation = testRowTrue(properties, "R34");
                if (locations.Count == 0)
                {
                    locations = scannedLocation;
                }
                if (!scannedAttributesCheck(scannedLocation, locations))
                {
                    selectionNotPossible();
                    return;
                }
                else
                {
                    //Add Nodes
                    var insertSet = new HashSet<int>();
                    var locationSet = new HashSet<int>(locations);
                    List<Connection> Connections = new List<Connection>();
                    List<Matrix_Cell> testCol = new List<Matrix_Cell>();

                    //Find Nodes
                    foreach (int X in abstraction)
                    {
                        testCol = scanColumnTrue(X, null);
                        foreach (Matrix_Cell M in testCol)
                        {
                            if (M.RowId <= 5)
                            {
                                insertSet.Add(M.RowId);
                            }
                        }
                    }
                    testCol.Clear();
                    foreach (int X in insert)
                    {
                        if (!insertSet.Contains(X))
                        {
                            selectionNotPossible();
                            return;
                        }
                        else
                        {
                            insertSet.Add(X);
                        }
                    }
                    int maxAbs = abstraction.Max();

                    for (int i = maxAbs; i >= 6; --i)
                    {
                        if (!abstraction.Contains(i))
                        {
                            abstraction.Add(i);
                        }
                    }
                    for (int i = 5; i >= 1; --i)
                    {
                        insertSet.Add(i);
                    }
                    List<int> scannedProperties = testRowTrue(abstraction, "R23");
                    scannedLocation = testRowTrue(properties, "R34");
                    if (!scannedAttributesCheck(scannedProperties, properties))
                    {
                        selectionNotPossible();
                        return;
                    }
                    if (!scannedAttributesCheck(scannedLocation, locations))
                    {
                        selectionNotPossible();
                        return;
                    }
                    //Make Connections
                    List<Matrix_Cell> testRow = new List<Matrix_Cell>();
                    foreach (int X in insertSet)
                    {
                        testRow = scanRowTrue(X, null);
                        foreach (Matrix_Cell M in testRow)
                        {
                            if (insertSet.Contains(M.ColumnId) || abstraction.Contains(M.ColumnId))
                            {
                                Connections.Add(new Connection(X, M.ColumnId, directConnectionForwards(M, X), virusId));
                            }
                        }
                    }
                    foreach (int X in abstraction)
                    {
                        testRow = scanRowTrue(X, "R2");
                        foreach (Matrix_Cell M in testRow)
                        {
                            if (abstraction.Contains(M.ColumnId))
                            {
                                Connections.Add(new Connection(X, M.ColumnId, directConnectionForwards(M, X), virusId));
                            }
                        }
                    }
                    if (Connections.Count == 0)
                    {
                        selectionNotPossible();
                        return;
                    }
                    List<int> Nodes = insertSet.ToList().Concat(abstraction).ToList();
                    Nodes = nodeFilter(Nodes, abstraction.Max(), Connections);
                    Nodes = Nodes.Concat(locations).ToList().Concat(properties).ToList();
                    Connections = connectionFilter(Nodes, Connections);
                    Nodes.Sort();
                    saveToDB(Nodes, Connections, virusId);
                    Visualize(Nodes, Connections, virusId);
                    severity(Nodes);
                    return;
                }
            }
        }
        private List<Connection> connectionFilter(List<int> nodes, List<Connection> Connections)
        {
            List<Connection> filteredConnections = new List<Connection>();
            foreach (Connection C in Connections)
            {
                if (nodes.Contains(C.source) && nodes.Contains(C.target))
                {
                    filteredConnections.Add(C);
                }
            }
            return filteredConnections;
        }
        private List<int> nodeFilter(List<int> nodes, int end, List<Connection> Connections)
        {
            List<int> tempNodes = new List<int>();
            foreach (int X in nodes)
            {
                if (X < 12)
                {
                    tempNodes.Add(X);
                }
            }
            List<int> removedNodes = bellmanForward(tempNodes, Connections);
            removedNodes = removedNodes.Concat(bellmanBackWards(tempNodes, Connections)).ToList();
            List<int> filtereNodes = new List<int>();
            foreach (int X in nodes)
            {
                if (!removedNodes.Contains(X))
                {
                    filtereNodes.Add(X);
                }
            }
            return filtereNodes;
        }
        private List<int> bellmanForward(List<int> nodes, List<Connection> Connections)
        {
            int source = nodes.Min();
            int target = nodes.Max();
            nodes.Sort();
            List<int> unreachables = new List<int>();
            Dictionary<int, int> distance = new Dictionary<int, int>();
            Dictionary<int, int> predecessor = new Dictionary<int, int>();
            foreach (int X in nodes)
            {
                if (X == source)
                {
                    distance[X] = 0;
                }
                else
                {
                    distance[X] = 999;
                }
                predecessor[X] = 999;
            }

            foreach (int X in nodes)
            {
                foreach (Connection C in Connections)
                {
                    if (distance[C.source] != 999)
                    {
                        if (nodes.Contains(C.source) && nodes.Contains(C.target))
                        {
                            if (distance[C.source] + 1 < distance[C.target])
                            {
                                distance[C.target] = distance[C.source] + 1;
                                predecessor[C.target] = C.source;
                            }
                        }

                    }
                }
            }
            foreach (int X in nodes)
            {
                if (distance[X] == 999)
                {
                    unreachables.Add(X);
                }
            }
            return unreachables;
        }
        private List<int> bellmanBackWards(List<int> nodes, List<Connection> Connections)
        {
            int source = nodes.Max();
            int target = nodes.Min();
            nodes.Sort();
            nodes.Reverse();
            List<int> unreachables = new List<int>();
            Dictionary<int, int> distance = new Dictionary<int, int>();
            Dictionary<int, int> predecessor = new Dictionary<int, int>();
            foreach (int X in nodes)
            {
                if (X == source)
                {
                    distance[X] = 0;
                }
                else
                {
                    distance[X] = 999;
                }
                predecessor[X] = 999;
            }

            foreach (int X in nodes)
            {
                foreach (Connection C in Connections)
                {
                    if (nodes.Contains(C.source) && nodes.Contains(C.target))
                    {
                        if (distance[C.target] != 999)
                        {
                            if (distance[C.target] + 1 < distance[C.source])
                            {
                                distance[C.source] = distance[C.target] + 1;
                                predecessor[C.source] = C.target;
                            }
                        }
                    }
                }
            }
            foreach (int X in nodes)
            {
                if (distance[X] == 999)
                {
                    unreachables.Add(X);
                }
            }
            return unreachables;
        }
        private bool touchInsertion(int X, List<Connection> Connections)
        {
            if (X < 6) return true;
            foreach (Connection C in Connections)
            {
                if (C.target == X && C.source < 6) return true;
            }
            return false;
        }
        private bool isSource(int X, List<Connection> Connections)
        {
            foreach (Connection C in Connections)
            {
                if (C.source == X)
                {
                    return true;
                }
            }
            return false;
        }
        private void forwardPropagationWithProperties(List<TrojanWebRebuild.Models.Attribute> userChosen, string virusId)
        {
            List<int> properties = new List<int>();
            List<int> insertAbs = new List<int>();
            foreach (TrojanWebRebuild.Models.Attribute A in userChosen)
            {
                if (A.CategoryName == "Properties")
                {
                    properties.Add(A.AttributeId);
                }
                else
                {
                    insertAbs.Add(A.AttributeId);
                }
            }
            int currentAttr = insertAbs.Min();
            int highestAttr = insertAbs.Max();
            var nodeSet = new HashSet<int>();
            List<int> abstraction = new List<int>();
            List<Matrix_Cell> tempRow = new List<Matrix_Cell>();
            bool tempDirect = false;
            List<Connection> Connections = new List<Connection>();

            while (currentAttr <= highestAttr)
            {
                nodeSet.Add(currentAttr);
                if (currentAttr >= 6)
                {
                    abstraction.Add(currentAttr);
                }
                tempRow = scanRowTrue(currentAttr, null);
                foreach (Matrix_Cell M in tempRow)
                {
                    tempDirect = directConnectionForwards(M, currentAttr);
                    //Only adds connections that are less than the highest 
                    //Attribute number chosen by the user
                    if (M.ColumnId <= highestAttr)
                    {
                        Connections.Add(new Connection(currentAttr, M.ColumnId, tempDirect, virusId));
                    }
                }
                ++currentAttr;
            }

            List<int> scannedProperties = testRowTrue(abstraction, "R23");
            if (!scannedAttributesCheck(scannedProperties, properties))
            {
                selectionNotPossible();
                return;
            }
            List<int> Locations = testRowTrue(scannedProperties, "R34");
            List<int> Nodes = nodeSet.ToList().Concat(Locations).ToList().Concat(scannedProperties).ToList();
            Nodes.Sort();
            displayResults(Nodes, Connections);
            saveToDB(Nodes, Connections, virusId);
        }

        //Checks to see if each of the ints in 'properties' is contained in
        //the list 'scannedProperties', if not. Returns false;
        private bool scannedAttributesCheck(List<int> scannedProperties, List<int> properties)
        {
            foreach (int X in properties)
            {
                if (!scannedProperties.Contains(X)) return false;
            }
            return true;
        }
        private void forwardPropagation(List<TrojanWebRebuild.Models.Attribute> userChosen, string virusId)
        {
            List<int> locations = new List<int>();
            List<int> insert = new List<int>();
            List<int> abstraction = new List<int>();

            foreach (TrojanWebRebuild.Models.Attribute A in userChosen)
            {
                if (A.CategoryName == "Location")
                {
                    locations.Add(A.AttributeId);
                }
                else if (A.CategoryName == "Abstraction")
                {
                    abstraction.Add(A.AttributeId);
                }
                else
                {
                    insert.Add(A.AttributeId);
                }
            }
            List<int> properties = testRowTrue(abstraction, "R23");
            List<int> scannedLocations = testRowTrue(properties, "R34");
            if (locations.Count == 0)
            {
                locations = scannedLocations;
            }
            if (!scannedAttributesCheck(scannedLocations, locations))
            {
                selectionNotPossible();
                return;
            }
            else
            {
                var insertSet = new HashSet<int>();
                List<Connection> Connections = new List<Connection>();
                List<Matrix_Cell> testCol = new List<Matrix_Cell>();

                //Find Nodes
                foreach (int X in abstraction)
                {
                    testCol = scanColumnTrue(X, null);
                    foreach (Matrix_Cell M in testCol)
                    {
                        if (M.RowId <= 5)
                        {
                            insertSet.Add(M.RowId);
                        }
                    }
                }
                testCol.Clear();
                foreach (int X in insert)
                {
                    if (!insertSet.Contains(X))
                    {
                        selectionNotPossible();
                        return;
                    }
                    else
                    {
                        insertSet.Add(X);
                    }
                }
                //Make Connections
                List<Matrix_Cell> testRow = new List<Matrix_Cell>();
                foreach (int X in insertSet)
                {
                    testRow = scanRowTrue(X, null);
                    foreach (Matrix_Cell M in testRow)
                    {
                        if (insertSet.Contains(M.ColumnId) || abstraction.Contains(M.ColumnId))
                        {
                            Connections.Add(new Connection(X, M.ColumnId, directConnectionForwards(M, X), virusId));
                        }
                    }
                }
                foreach (int X in abstraction)
                {
                    testRow = scanRowTrue(X, "R2");
                    foreach (Matrix_Cell M in testRow)
                    {
                        if (abstraction.Contains(M.ColumnId))
                        {
                            Connections.Add(new Connection(X, M.ColumnId, directConnectionForwards(M, X), virusId));
                        }
                    }
                }
                if (Connections.Count == 0)
                {
                    selectionNotPossible();
                    return;
                }
                List<int> Nodes = insertSet.ToList().Concat(locations).ToList().Concat(properties).ToList().Concat(abstraction).ToList();
                Nodes = nodeFilter(Nodes, abstraction.Max(), Connections);
                Connections = connectionFilter(Nodes, Connections);
                Nodes.Sort();
                saveToDB(Nodes, Connections, virusId);
                Visualize(Nodes, Connections, virusId);
                severity(Nodes);
                return;
            }
        }
        private void splitPropagation(List<TrojanWebRebuild.Models.Attribute> userChosen, string virusId)
        {
            List<int> locations = new List<int>();
            List<int> insert = new List<int>();

            foreach (TrojanWebRebuild.Models.Attribute A in userChosen)
            {
                if (A.CategoryName == "Location")
                {
                    locations.Add(A.AttributeId);
                }
                else
                {
                    insert.Add(A.AttributeId);
                }
            }
            List<int> properties = testColumnTrue(locations, "R34");
            List<int> abstraction = testColumnTrue(properties, "R23");
            if (abstraction.Count == 0 || properties.Count == 0)
            {
                selectionNotPossible();
                return;
            }
            var insertSet = new HashSet<int>();
            List<Connection> Connections = new List<Connection>();
            List<Matrix_Cell> testCol = new List<Matrix_Cell>();

            //Find Nodes
            foreach (int X in abstraction)
            {
                testCol = scanColumnTrue(X, null);
                foreach (Matrix_Cell M in testCol)
                {
                    if (M.RowId <= 5)
                    {
                        insertSet.Add(M.RowId);
                    }
                }
            }
            foreach (int X in insert)
            {
                if (!insertSet.Contains(X))
                {
                    selectionNotPossible();
                    return;
                }
                else
                {
                    insertSet.Add(X);
                }
            }
            testCol.Clear();
            //Make Connections
            List<Matrix_Cell> testRow = new List<Matrix_Cell>();
            foreach (int X in insertSet)
            {
                testRow = scanRowTrue(X, null);
                foreach (Matrix_Cell M in testRow)
                {
                    if (insertSet.Contains(M.ColumnId) || abstraction.Contains(M.ColumnId))
                    {
                        Connections.Add(new Connection(X, M.ColumnId, directConnectionForwards(M, X), virusId));
                    }
                }
            }
            foreach (int X in abstraction)
            {
                testRow = scanRowTrue(X, "R2");
                foreach (Matrix_Cell M in testRow)
                {
                    if (abstraction.Contains(M.ColumnId))
                    {
                        Connections.Add(new Connection(X, M.ColumnId, directConnectionForwards(M, X), virusId));
                    }
                }
            }
            if (Connections.Count == 0)
            {
                selectionNotPossible();
                return;
            }
            List<int> Nodes = insertSet.ToList().Concat(locations).ToList().Concat(properties).ToList().Concat(abstraction).ToList();
            Nodes = nodeFilter(Nodes, abstraction.Max(), Connections);
            Connections = connectionFilter(Nodes, Connections);
            Nodes.Sort();
            saveToDB(Nodes, Connections, virusId);
            Visualize(Nodes, Connections, virusId);
            severity(Nodes);
            return;
        }
        //Receives a list of column numbers and determines which rows
        //The columns have true value in common
        private List<int> testColumnTrue(List<int> list, string subMatrix)
        {
            List<int> resultsInt = new List<int>();
            List<Matrix_Cell> colTrue = new List<Matrix_Cell>();
            var removedSet = new HashSet<int>();
            foreach (int X in list)
            {
                colTrue = getColumn(X, subMatrix);
                foreach (Matrix_Cell A in colTrue)
                {
                    if (A.value == false)
                    {
                        removedSet.Add(A.RowId);
                        if (resultsInt.Contains(A.RowId))
                        {
                            resultsInt.Remove(A.RowId);
                        }
                    }
                    if ((A.value == true) && (!removedSet.Contains(A.RowId)) && (!resultsInt.Contains(A.RowId)))
                    {
                        resultsInt.Add(A.RowId);
                    }
                }
            }
            return resultsInt;
        }
        //Receives a list of column numbers and determines which rows
        //The columns have false vaue in common
        private List<int> testColumnFalse(List<int> list, string subMatrix)
        {
            List<int> resultsInt = new List<int>();
            List<Matrix_Cell> colTrue = new List<Matrix_Cell>();
            var removedSet = new HashSet<int>();
            foreach (int X in list)
            {
                colTrue = getColumn(X, subMatrix);
                foreach (Matrix_Cell A in colTrue)
                {
                    if (A.value == true)
                    {
                        removedSet.Add(A.RowId);
                        if (resultsInt.Contains(A.RowId))
                        {
                            resultsInt.Remove(A.RowId);
                        }
                    }
                    if ((A.value == false) && (!removedSet.Contains(A.RowId)) && (!resultsInt.Contains(A.RowId)))
                    {
                        resultsInt.Add(A.RowId);
                    }
                }
            }
            return resultsInt;
        }
        //Receives a list of row numbers and determines which columns
        //The rows have value true in common
        private List<int> testRowTrue(List<int> list, string subMatrix)
        {
            List<int> resultsInt = new List<int>();
            List<Matrix_Cell> rowTrue = new List<Matrix_Cell>();
            var removedSet = new HashSet<int>();
            foreach (int X in list)
            {
                rowTrue = getRow(X, subMatrix);
                foreach (Matrix_Cell A in rowTrue)
                {
                    if (A.value == false)
                    {
                        removedSet.Add(A.ColumnId);
                        if (resultsInt.Contains(A.ColumnId))
                        {
                            resultsInt.Remove(A.ColumnId);
                        }
                    }
                    if ((A.value == true) && (!removedSet.Contains(A.ColumnId)) && (!resultsInt.Contains(A.ColumnId)))
                    {
                        resultsInt.Add(A.ColumnId);
                    }
                }
            }
            return resultsInt;
        }
        //Receives a list of row numbers and determines which columns
        //The rows have value false in common
        private List<int> testRowFalse(List<int> list, string subMatrix)
        {
            List<int> resultsInt = new List<int>();
            List<Matrix_Cell> rowTrue = new List<Matrix_Cell>();
            var removedSet = new HashSet<int>();
            foreach (int X in list)
            {
                rowTrue = getRow(X, subMatrix);
                foreach (Matrix_Cell A in rowTrue)
                {
                    if (A.value == true)
                    {
                        removedSet.Add(A.ColumnId);
                        if (resultsInt.Contains(A.ColumnId))
                        {
                            resultsInt.Remove(A.ColumnId);
                        }
                    }
                    if ((A.value == false) && (!removedSet.Contains(A.ColumnId)) && (!resultsInt.Contains(A.ColumnId)))
                    {
                        resultsInt.Add(A.ColumnId);
                    }
                }
            }
            return resultsInt;
        }

        private void displayResults(List<int> NodeInt, List<Connection> Edges)
        {
            List<Models.Attribute> Nodes = intToAttr(NodeInt);
            if ((Nodes.Count == 0) || (Edges.Count == 0))
            {
                notes.Visible = true;
                startOverDiv.Visible = true;
            }
            else
            {
                nodesGrid.DataSource = Attribute_Sorting(Nodes);
                nodesGrid.DataBind();
                nodesDiv.Visible = true;
                nodesGrid.Visible = true;
                ConnectionsGrid.DataSource = Connection_Sorting(Edges);
                ConnectionsGrid.DataBind();
                ConnectionsResults.Visible = true;
                ConnectionsGrid.Visible = true;
            }
        }

        //Convert a list of attributes to a list of ints (Attribute ID)
        private List<int> attrToInt(List<TrojanWebRebuild.Models.Attribute> attributes)
        {
            List<int> Ints = new List<int>();
            foreach (TrojanWebRebuild.Models.Attribute A in attributes)
            {
                Ints.Add(A.AttributeId);
            }
            return Ints;
        }
        private List<TrojanWebRebuild.Models.Attribute> intToAttr(List<int> ints)
        {
            List<Models.Attribute> attrs = new List<Models.Attribute>();
            foreach (int X in ints)
            {
                attrs.Add(getAttribute(X));
            }
            return attrs;
        }
        protected List<TrojanWebRebuild.Models.Attribute> Attribute_Sorting(List<TrojanWebRebuild.Models.Attribute> List)
        {
            List<TrojanWebRebuild.Models.Attribute> SortedList = List.OrderBy(p => p.AttributeId).ToList();
            return SortedList;
        }
        protected bool Connection_Check(List<Connection> List, int source_, int dest, bool TF, string virusId)
        {
            foreach (Connection X in List)
            {
                if (X.source == source_ && X.target == dest && X.VirusId == virusId) return true;
            }
            return false;
        }
        protected bool Attribute_Check(List<TrojanWebRebuild.Models.Attribute> List, int Y)
        {
            foreach (TrojanWebRebuild.Models.Attribute X in List)
            {
                if (X.AttributeId == Y) return true;
            }
            return false;
        }

        //Checks to see if the list of nodes containts the Id Y
        protected bool NodeCheck(List<TrojanWebRebuild.Models.Node> Nodes, int Y)
        {
            foreach (TrojanWebRebuild.Models.Node X in Nodes)
            {
                if (X.nodeID == Y) return true;
            }
            return false;
        }

        //Takes the list of connections generated and returns a list of all the nodes
        private List<TrojanWebRebuild.Models.Node> edgesToNodes(List<Connection> Connections)
        {
            List<TrojanWebRebuild.Models.Node> Nodes = new List<TrojanWebRebuild.Models.Node>();
            TrojanWebRebuild.Models.Attribute tempAttribute;
            foreach (Connection F in Connections)
            {

                if (!NodeCheck(Nodes, F.source))
                {
                    tempAttribute = getAttribute(F.source);
                    if (F.source <= 5) Nodes.Add(new TrojanWebRebuild.Models.Node(F.source, tempAttribute.AttributeName, "Chip Life Cycle", tempAttribute.F_in, tempAttribute.F_out, tempAttribute.Description));
                    else if ((5 < F.source) && (F.source <= 11)) Nodes.Add(new TrojanWebRebuild.Models.Node(F.source, tempAttribute.AttributeName, "Abstraction", tempAttribute.F_in, tempAttribute.F_out, tempAttribute.Description));
                    else if ((11 < F.source) && (F.source <= 28)) Nodes.Add(new TrojanWebRebuild.Models.Node(F.source, tempAttribute.AttributeName, "Properties", tempAttribute.F_in, tempAttribute.F_out, tempAttribute.Description));
                    else
                    {
                        Nodes.Add(new TrojanWebRebuild.Models.Node(F.source, tempAttribute.AttributeName, "Location", tempAttribute.F_in, tempAttribute.F_out, tempAttribute.Description));
                    }
                }
                if (!NodeCheck(Nodes, F.target))
                {
                    tempAttribute = getAttribute(F.target);
                    if (F.target <= 5) Nodes.Add(new TrojanWebRebuild.Models.Node(F.target, tempAttribute.AttributeName, "Chip Life Cycle", tempAttribute.F_in, tempAttribute.F_out, tempAttribute.Description));
                    else if ((5 < F.target) && (F.target <= 11)) Nodes.Add(new TrojanWebRebuild.Models.Node(F.target, tempAttribute.AttributeName, "Abstraction", tempAttribute.F_in, tempAttribute.F_out, tempAttribute.Description));
                    else if ((11 < F.target) && (F.target <= 28)) Nodes.Add(new TrojanWebRebuild.Models.Node(F.target, tempAttribute.AttributeName, "Properties", tempAttribute.F_in, tempAttribute.F_out, tempAttribute.Description));
                    else
                    {
                        Nodes.Add(new TrojanWebRebuild.Models.Node(F.target, tempAttribute.AttributeName, "Location", tempAttribute.F_in, tempAttribute.F_out, tempAttribute.Description));
                    }
                }
            }
            return Nodes.OrderBy(p => p.nodeID).ToList();
        }
        private void hideResults()
        {
            NoSelected.Visible = false;
            canNot.Visible = false;
            abstractionNone.Visible = abstractionNone.Visible = abstractionResults.Visible = abstractionGrid.Visible = false;
            directNone.Visible = direct.Visible = directGrid.Visible = false;
            indirectNone.Visible = indirectGrid.Visible = indirect.Visible = false;
            RowResults.Visible = RowGrid.Visible = false;
            ColumnResults.Visible = ColumnGrid.Visible = false;
            ConnectionsResults.Visible = false;
            ConnectionsGrid.Visible = false;
            LocationGrid.Visible = Locationlbl.Visible = false;
            nodesDiv.Visible = nodesGrid.Visible = false;
            notes.Visible = abstractionEmpty.Visible = startOverDiv.Visible = notBuilt.Visible = false;
            jumboWrap.Visible = false;
            visualRepLbl.Visible = false;
        }
        private void showVisButtons()
        {
            startOverDiv.Visible = true;
            startOverBtn.Visible = true;
        }
        protected void Visualize(List<int> V_Items, List<Connection> Connections, string virusId)
        {
            jumboWrap.Visible = true;
            visualRepLbl.Visible = true;
            List<TrojanWebRebuild.Models.Node> Nodes = new List<TrojanWebRebuild.Models.Node>();
            Models.Attribute tempAttr = null;
            foreach (int X in V_Items)
            {
                tempAttr = getAttribute(X);
                Nodes.Add(new TrojanWebRebuild.Models.Node(tempAttr.AttributeId, tempAttr.AttributeName, getCategoryFromAttr(X).CategoryName, tempAttr.F_in, tempAttr.F_out, tempAttr.Description));
            }
            string json;
            json = JsonConvert.SerializeObject(virusId);
            ClientScript.RegisterArrayDeclaration("virusId", json);
            foreach (Connection Con in Connections)
            {
                json = JsonConvert.SerializeObject(Con);
                ClientScript.RegisterArrayDeclaration("edges", json);
            }
            foreach (TrojanWebRebuild.Models.Node N in Nodes)
            {
                json = JsonConvert.SerializeObject(N);
                ClientScript.RegisterArrayDeclaration("nod", json);
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "id", "visualize('#visrep', " + Connections.Count + "," + Nodes.Count + ")", true);
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            if (this.saveNameTxtBx.Text == "")
            {
                ErrorMessage.Visible = true;
                FailureText.Text = "Please Enter a nickname for your work to save it";
            }
            else
            {
                ErrorMessage.Visible = false;
                if (db.SeverityRating.Any(W => (W.nickName == saveNameTxtBx.Text) && (W.userName == HttpContext.Current.User.Identity.Name) && (W.coverage == false)))
                {
                    ErrorMessage.Visible = true;
                    FailureText.Text = "You already have a virus with that nickname";
                }
                else
                {

                    Virus virus = new Virus();
                    string virusId = null;
                    using (VirusDescriptionActions usersVirus = new VirusDescriptionActions())
                    {
                        virusId = usersVirus.GetVirusId();
                        virus.virusId = virusId;
                        virus.userName = HttpContext.Current.User.Identity.Name;
                        virus.virusNickName = this.saveNameTxtBx.Text;
                    }
                    SeverityRating SeverityRating = db.SeverityRating.Where(c => c.VirusId == virusId).FirstOrDefault();
                    SeverityRating.coverage = false;
                    SeverityRating.Saved = true;
                    SeverityRating.userName = HttpContext.Current.User.Identity.Name;
                    SeverityRating.nickName = saveNameTxtBx.Text;
                    db.SeverityRating.Add(SeverityRating);
                    db.Virus.Add(virus);
                    db.SaveChanges();
                    SavedMessage.Visible = true;
                    savedText.Text = "Virus Saved";
                }
            }
        }
    }
}