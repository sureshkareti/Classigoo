//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Classigoo
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Messages
    {
        public int MsgId { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public int AdId { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public bool Archive { get; set; }
        public string[] MovetoArchive { get; set; }
        public bool IsRed { get; set; }
    }
}
