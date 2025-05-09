using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using JobTracking.DataAccess.Data.Base;

namespace JobTracking.DataAccess.Data.Models;

public class Preslava : IEntity
{
    [Key]
    public int Id { get; set; }
    public bool IsActive { get; set; }                                                                                                                                                            
    public DateTime CreatedOn { get; set; }
    
    [NotNull] 
    public /*required*/ string CreatedBy { get; set; }/* = null!;*/
    public DateTime? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; }

    [Required]
    public int SthID { get; set; }
    
    public virtual Sth Sth { get; set; }
}