using System.ComponentModel.DataAnnotations;

namespace E_Learning.Models
{
    public class Alert : IEntity
    {
        public int Id { get; set; }
        

        [MaxLength(500)]
        public required string Message { get; set; }


        public DateTime CreatedAt { get; set; }

        
        public bool IsResolved { get; set; }


     

       
      

    }
}
