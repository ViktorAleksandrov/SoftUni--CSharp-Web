using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eventures.Web.ViewModels.Events
{
    public class CreateEventBindingModel : IValidatableObject
    {
        [Required]
        [MinLength(10, ErrorMessage = "The {0} should be at least {1} symbols long.")]
        public string Name { get; set; }

        [Required]
        public string Place { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [Range(1, int.MaxValue)]
        public int TotalTickets { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal PricePerTicket { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Start < DateTime.Now)
            {
                yield return new ValidationResult("Start cannot be before current date and time", new[] { "Start" });
            }

            if (this.End < this.Start)
            {
                yield return new ValidationResult("End cannot be before Start", new[] { "End" });
            }
        }
    }
}
