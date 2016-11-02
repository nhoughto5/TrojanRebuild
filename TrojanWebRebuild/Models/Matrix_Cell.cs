using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TrojanWebRebuild.Models
{
    public class Matrix_Cell
    {
        [Key]
        public int CellId { get; set; }
        public int RowId { get; set; }
        public int ColumnId { get; set; }
        public Nullable<bool> value { get; set; }
        public string submatrix { get; set; }
        public int MatrixMatrix_Id { get; set; }
        public string MatrixName { get; set; }
    }
}