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

            // Transformer les entités en ViewModel
            var offresEmploiViewModel = offresEmploi.Select(o => new DashboardConseil.ViewModel.OffreEmploiViewModel
            {
                Id = o.Id,
                Titre = o.Titre,
                Description = o.Description,
                DatePublication = o.DatePublication,
                Lieu = o.Lieu,
                ImageUrl = o.ImageUrl
            }).ToList();

            // Retourner les ViewModels à la vue
            return View("~/Views/OffresEmploi/IndexOffreEmploi.cshtml", offresEmploiViewModel);
        }

        // ----------------------------
        // Formulaire de création d'une offre
        // ----------------------------
        public IActionResult CreateOffreEmploi()
        {
            var offreEmploi = new OffreEmploiViewModel();
            return View("~/Views/OffresEmploi/CreateOffreEmploi.cshtml", offreEmploi); // Vue création
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOffreEmploi(OffreEmploiViewModel model)
        {
            
                var offre = new OffreEmploi
                {
                    Titre = model.Titre,
                    Description = model.Description,
                    Entreprise = model.Entreprise,
                    DatePublication = model.DatePublication,
                    Lieu = model.Lieu
                };

                // Gérer le téléchargement de l'image
                if (model.FichierImage != null && model.FichierImage.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/images/offres-emploi", model.FichierImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FichierImage.CopyToAsync(stream);
                    }
                    offre.ImageUrl = model.FichierImage.FileName; // Enregistrer le chemin ou nom du fichier
                }

                // Ajouter l'offre à la base de données
                _context.OffresEmploi.Add(offre);
                await _context.SaveChangesAsync();

                // Rediriger vers la liste des offres
                return RedirectToAction("IndexOffreEmploi");

        }



        // ----------------------------
        // Formulaire de modification d'une offre
        // ----------------------------
        public async Task<IActionResult> EditOffreEmploi(int id)
        {
            var offre = await _context.OffresEmploi.FindAsync(id);
            if (offre == null)
            {
                return NotFound();
            }

            var model = new OffreEmploiViewModel
            {
                Id = offre.Id,
                Titre = offre.Titre,
                Description = offre.Description,
                Entreprise = offre.Entreprise,
                DatePublication = offre.DatePublication,
                Lieu = offre.Lieu,
                ImageUrl = offre.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOffreEmploi(OffreEmploiViewModel model)
        {
            
                var offre = await _context.OffresEmploi.FindAsync(model.Id);
                if (offre == null)
                {
                    return NotFound();
                }

                offre.Titre = model.Titre;
                offre.Description = model.Description;
                offre.Entreprise = model.Entreprise;
                offre.DatePublication = model.DatePublication;
                offre.Lieu = model.Lieu;

                // Gérer l'image si elle a été changée
                if (model.FichierImage != null && model.FichierImage.Length > 0)
                {
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/offres-emploi");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var filePath = Path.Combine(folderPath, model.FichierImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.FichierImage.CopyToAsync(stream);
                    }

                    offre.ImageUrl = model.FichierImage.FileName; // Mettre à jour le chemin de l'image
                }

                _context.OffresEmploi.Update(offre);
                await _context.SaveChangesAsync();

                return RedirectToAction("IndexOffreEmploi");
            }


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
