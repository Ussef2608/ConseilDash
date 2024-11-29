using DashboardConseil.Data;
using DashboardConseil.Models;
using DashboardConseil.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DashboardConseil.Controllers
{
    [Authorize]
    public class OffresEmploiController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OffresEmploiController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // ----------------------------
        // Liste des offres d'emploi
        // ----------------------------
        public async Task<IActionResult> IndexOffreEmploi()
        {
            var offresEmploi = await _context.OffresEmploi.ToListAsync();
            return View("~/Views/OffreEmploi/IndexOffreEmploi.cshtml", offresEmploi); // Vue index
        }

        // ----------------------------
        // Formulaire de création d'une offre
        // ----------------------------
        public IActionResult CreateOffreEmploi()
        {
            var offreEmploi = new OffreEmploiViewModel();
            return View("~/Views/OffreEmploi/CreateOffreEmploi.cshtml", offreEmploi); // Vue création
        }

        [HttpPost]
        public async Task<IActionResult> CreateOffreEmploi(OffreEmploiViewModel offreEmploiViewModel)
        {
            // Chemin relatif pour enregistrer les images
            string imagePath = null;

            // Vérifier si une image a été téléchargée
            if (offreEmploiViewModel.FichierImage != null)
            {
                // Répertoire où les images seront sauvegardées
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/clients");

                // Créez le répertoire si nécessaire
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                // Générer un nom unique pour l'image
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(offreEmploiViewModel.FichierImage.FileName);

                // Combinez le chemin du répertoire avec le nom du fichier
                string fullPath = Path.Combine(uploadDir, uniqueFileName);

                // Sauvegarder le fichier sur le serveur
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await offreEmploiViewModel.FichierImage.CopyToAsync(fileStream);
                }

                // Stocker le chemin relatif de l'image (pour l'enregistrement en base de données)
                imagePath = Path.Combine("images/clients", uniqueFileName);
            }

            // Créer une entité OffreEmploi à partir du ViewModel
            var offreEmploi = new OffreEmploi
            {
                Titre = offreEmploiViewModel.Titre,
                Description = offreEmploiViewModel.Description,
                DatePublication = offreEmploiViewModel.DatePublication,
                Lieu = offreEmploiViewModel.Lieu,
                ImageUrl = imagePath // Enregistrer le chemin de l'image
            };

            // Ajouter l'offre dans la base de données
            _context.OffresEmploi.Add(offreEmploi);
            await _context.SaveChangesAsync();

            // Rediriger vers la liste des offres
            return RedirectToAction(nameof(IndexOffreEmploi));
        }


        // ----------------------------
        // Formulaire de modification d'une offre
        // ----------------------------
        public async Task<IActionResult> EditOffreEmploi(int id)
        {
            var offreEmploi = await _context.OffresEmploi.FindAsync(id);
            if (offreEmploi == null)
            {
                return NotFound(); // Si l'offre n'existe pas
            }

            var offreEmploiViewModel = new OffreEmploiViewModel
            {
                Id = offreEmploi.Id,
                Titre = offreEmploi.Titre,
                Description = offreEmploi.Description,
                DatePublication = offreEmploi.DatePublication,
                Lieu = offreEmploi.Lieu,
                ImageUrl = offreEmploi.ImageUrl // Afficher l'image actuelle
            };

            return View("~/Views/OffreEmploi/EditOffreEmploi.cshtml", offreEmploiViewModel); // Vue modification
        }

        [HttpPost]
        //public async Task<IActionResult> EditOffreEmploi(OffreEmploiViewModel offreEmploiViewModel)
        //{
        //    var offreEmploi = await _context.OffresEmploi.FindAsync(offreEmploiViewModel.Id);
        //    if (offreEmploi == null)
        //    {
        //        return NotFound(new { Message = "Offre d'emploi non trouvée." });
        //    }

        //    // Variables pour les chemins d'images
        //    string oldImagePath = offreEmploi.ImageUrl;
        //    string newImagePath = oldImagePath; // Par défaut, la nouvelle image reste la même

        //    // Mise à jour des champs texte
        //    offreEmploi.Titre = offreEmploiViewModel.Titre;
        //    offreEmploi.Description = offreEmploiViewModel.Description;
        //    offreEmploi.DatePublication = offreEmploiViewModel.DatePublication;
        //    offreEmploi.Lieu = offreEmploiViewModel.Lieu;

        //    // Gestion de l'image
        //    if (offreEmploiViewModel.ImageUrl != null && offreEmploiViewModel.ImageUrl.Length > 0)
        //    {
        //        // Validation de l'extension de fichier
        //        var fileExtension = Path.GetExtension(offreEmploiViewModel.ImageUrl.FileName).ToLower();
        //        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

        //        if (!allowedExtensions.Contains(fileExtension))
        //        {
        //            ModelState.AddModelError("ImageUrl", "Seules les images au format JPG et PNG sont autorisées.");
        //            return BadRequest(ModelState);
        //        }

        //        try
        //        {
        //            // Créez un répertoire si nécessaire
        //            string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/offres-emploi");
        //            if (!Directory.Exists(uploadDir))
        //            {
        //                Directory.CreateDirectory(uploadDir);
        //            }

        //            // Générer un nom unique pour le fichier
        //            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
        //            var fullPath = Path.Combine(uploadDir, uniqueFileName);

        //            // Sauvegarder le fichier sur le serveur
        //            using (var stream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                await offreEmploiViewModel.ImageUrl.CopyToAsync(stream);
        //            }

        //            // Supprimer l'ancienne image si elle existe
        //            if (!string.IsNullOrEmpty(oldImagePath))
        //            {
        //                var oldFullPath = Path.Combine(uploadDir, oldImagePath);
        //                if (System.IO.File.Exists(oldFullPath))
        //                {
        //                    System.IO.File.Delete(oldFullPath);
        //                }
        //            }

        //            // Mettre à jour les chemins
        //            newImagePath = uniqueFileName;
        //            offreEmploi.ImageUrl = uniqueFileName;
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, new { Message = "Erreur lors du traitement de l'image.", Error = ex.Message });
        //        }
        //    }

        //    try
        //    {
        //        // Mise à jour de l'offre d'emploi dans la base de données
        //        _context.OffresEmploi.Update(offreEmploi);
        //        await _context.SaveChangesAsync();

        //        // Retourner les chemins d'images pour affichage côté client
        //        return Json(new
        //        {
        //            Message = "Offre d'emploi mise à jour avec succès.",
        //            OldImagePath = $"/images/offres-emploi/{oldImagePath}",
        //            NewImagePath = $"/images/offres-emploi/{newImagePath}"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = "Erreur lors de la mise à jour de l'offre d'emploi.", Error = ex.Message });
        //    }
        //}


        // ----------------------------
        // Suppression d'une offre
        // ----------------------------
        public async Task<IActionResult> DeleteOffreEmploi(int id)
        {
            var offreEmploi = await _context.OffresEmploi.FindAsync(id);
            if (offreEmploi != null)
            {
                // Supprimer l'image associée si elle existe
                if (!string.IsNullOrEmpty(offreEmploi.ImageUrl))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, offreEmploi.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.OffresEmploi.Remove(offreEmploi);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(IndexOffreEmploi)); // Redirection vers l'index
        }
    }

}
