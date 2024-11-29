using System.ComponentModel.DataAnnotations;

namespace DashboardConseil.ViewModel
{
    public class OffreEmploiViewModel
    {
        public int Id { get; set; } // ID de l'offre d'emploi

        [Required(ErrorMessage = "Le titre du poste est requis.")]
        [StringLength(100, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères.")]
        [Display(Name = "Titre du poste")]
        public string Titre { get; set; } // Titre du poste

        [Required(ErrorMessage = "La description de l'offre est requise.")]
        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères.")]
        [Display(Name = "Description de l'offre")]
        public string Description { get; set; } // Description de l'offre

        [Required(ErrorMessage = "Le nom de l'entreprise est requis.")]
        [StringLength(100, ErrorMessage = "Le nom de l'entreprise ne peut pas dépasser 100 caractères.")]
        [Display(Name = "Entreprise")]
        public string Entreprise { get; set; } // Nom de l'entreprise

        [Required(ErrorMessage = "La date de publication est requise.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de publication")]
        public DateTime DatePublication { get; set; } // Date de publication

        [Required(ErrorMessage = "Le lieu de l'offre est requis.")]
        [StringLength(100, ErrorMessage = "Le lieu ne peut pas dépasser 100 caractères.")]
        [Display(Name = "Lieu")]
        public string Lieu { get; set; } // Lieu de l'offre

        //[Range(0, double.MaxValue, ErrorMessage = "Le salaire ne peut pas être négatif.")]
        //[Display(Name = "Salaire proposé (optionnel)")]
        //public decimal? Salaire { get; set; } // Salaire proposé (optionnel)

        //[StringLength(200, ErrorMessage = "L'URL ne peut pas dépasser 200 caractères.")]
        //[Display(Name = "URL de l'offre (optionnel)")]
        //public string Url { get; set; } // URL de l'offre (optionnel)

        [StringLength(250, ErrorMessage = "L'URL ou chemin du fichier ne peut pas dépasser 250 caractères.")]
        [Display(Name = "Image (URL ou chemin du fichier)")]
        public string ImageUrl { get; set; } // URL ou chemin du fichier image associé à l'offre

        // Si vous avez besoin de gérer un fichier image à télécharger, vous pouvez ajouter un champ de type IFormFile
        [DataType(DataType.Upload)]
        public IFormFile FichierImage { get; set; } // Fichier image pour l'offre d'emploi
    }

}
