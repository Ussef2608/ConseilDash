using System.ComponentModel.DataAnnotations;

namespace DashboardConseil.Models
{
    public class OffreEmploi
    {
        [Key]
        public int Id { get; set; } // ID unique auto-incrémenté

        [Required]
        [StringLength(100)]
        [Display(Name = "Titre du poste")]
        public string Titre { get; set; } // Titre du poste

        [Required]
        [StringLength(500)]
        [Display(Name = "Description de l'offre")]
        public string Description { get; set; } // Description de l'offre

        [Required]
        [StringLength(100)]
        [Display(Name = "Entreprise")]
        public string Entreprise { get; set; } // Nom de l'entreprise

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date de publication")]
        public DateTime DatePublication { get; set; } // Date de publication

        [Required]
        [StringLength(100)]
        [Display(Name = "Lieu")]
        public string Lieu { get; set; } // Localisation de l'offre

        //[Range(0, double.MaxValue)]
        //[Display(Name = "Salaire proposé (optionnel)")]
        //public decimal? Salaire { get; set; } // Salaire proposé (optionnel)

        //[StringLength(200)]
        //[Display(Name = "URL de l'offre (optionnel)")]
        //public string Url { get; set; } // Lien vers l'offre (optionnel)
        [StringLength(250)]
        [Display(Name = "Image (URL ou chemin du fichier)")]
        public string ImageUrl { get; set; } // Nouveau champ pour l'image
    }
}
