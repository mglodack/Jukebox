//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Artist
    {
        public Artist()
        {
            this.Songs = new HashSet<Song>();
        }
    
        public int Id { get; set; }
        public string sName { get; set; }
        public int SongId { get; set; }
    
        public virtual ICollection<Song> Songs { get; set; }
    }
}
