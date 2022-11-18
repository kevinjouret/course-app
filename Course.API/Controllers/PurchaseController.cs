using CourseAPI.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;
using CourseAPI.Models;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly CourseDbContext _context;

        public PurchaseController(CourseDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retourne les purchases realisés par tous les utilisateurs triés par date et par nom.
        /// Accessible seulement aux administrateurs
        /// </summary>
        /// <returns>Liste d'purchases</returns>

        [Authorize(Roles = "Admin")]
        // GET: api/Purchase/GetPurchases
        [HttpGet("[Action]")]
        public async Task<IActionResult> GetPurchases()
        {
            var purchases = from a in _context.Purchase
                         join c in _context.Course on a.CourseId equals c.CourseId
                         join u in _context.User on a.UserId equals u.UserId
                         orderby a.OrderDate, u.Lastname
                         select new { a.PurchaseId, u.UserId, u.Lastname, u.Firstname, a.OrderDate, a.Price, CourseName = c.Name };

            if (purchases.Count() == 0)
            {
                return NotFound();
            }
            return Ok(await purchases.ToListAsync());
        }



        /// <summary>
        /// Retourne un purchase Accessible seulement aux administrateurs
        /// </summary>
        /// <param name="purchaseId">identifiant d'un purchase</param>
        /// <returns>Un purchase</returns>

        [Authorize(Roles = "Admin")]
        // GET: api/Purchase/GetAchat/5
        [HttpGet("[Action]/{purchaseId}")]
        public async Task<IActionResult> GetPurchase(int purchaseId)
        {
            var purchases = from a in _context.Purchase
                         join c in _context.Course on a.CourseId equals c.CourseId
                         where (a.PurchaseId == purchaseId)
                         orderby a.OrderDate
                         select new { a.PurchaseId, a.OrderDate, a.Price, NonCours = c.Name };

            if (purchases.Count() == 0)
            {
                return NotFound();
            }
            return Ok(await purchases.ToListAsync());
        }



        /// <summary>
        /// Retourne pour un utilisateur donné une liste d'purchases triés par date.
        /// Accessible seulement à un administrateur
        /// </summary>
        /// <param name="userId">identifiant d'un utilisateur</param>
        /// <returns>Liste d'purchases</returns>

        [Authorize(Roles = "Admin")]
        // GET: api/Purchase/GetAchatsParUtilisateur/5
        [HttpGet("[Action]/{userId}")]
        public async Task<IActionResult> GetPurchasesByUser(int userId)
        {
            var purchases = from a in _context.Purchase
                         join c in _context.Course on a.CourseId equals c.CourseId
                         where a.UserId == userId
                         orderby a.OrderDate
                         select new { a.PurchaseId, a.UserId, a.OrderDate, a.Price, CourseName = c.Name };

            if (purchases.Count() == 0)
            {
                return NotFound();
            }

            return Ok(await purchases.ToListAsync());
        }



        /// <summary>
        /// Retourne pour un utilisateur authentifié ses purchases.
        /// Accessible à toute personne authentifiée
        /// </summary>
        /// <returns>Liste d'purchases</returns>

        [Authorize]
        // GET: api/Purchase/GetAchatsAuth
        [HttpGet("[Action]")]
        public async Task<IActionResult> GetPurchasesAuth()
        {
            string currentId = "0";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity.Claims.Count() != 0)
            {
                IEnumerable<Claim> claims = identity.Claims;
                currentId = identity.FindFirst(p => p.Type == "UserId").Value;
            }
            else
                return StatusCode(StatusCodes.Status401Unauthorized);

            if (Convert.ToInt32(currentId) == 0)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            var purchases = from a in _context.Purchase
                         join c in _context.Course on a.CourseId equals c.CourseId
                         where a.UserId == Convert.ToInt32(currentId)
                         orderby a.OrderDate
                         select new { a.PurchaseId, a.UserId, a.OrderDate, a.Price, CourseName = c.Name };

            if (purchases.Count() == 0)
            {
                return NotFound();
            }
            else
                return Ok(await purchases.ToListAsync());
        }



        /// <summary>
        /// Retourne pour un utilisateur authentifié un purchase.
        /// Accessible à toute personne authentifiée
        /// </summary>
        ///<param name="purchaseId">identifiant d'un purchase</param>
        /// <returns>Un purchase</returns>

        [Authorize]
        // GET: api/Purchase/GetAchatAuth/5
        [HttpGet("[Action]/{purchaseId}")]
        public async Task<IActionResult> GetPurchaseAuth(int purchaseId)
        {
            string currentId = "0";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity.Claims.Count() != 0)
            {
                IEnumerable<Claim> claims = identity.Claims;
                currentId = identity.FindFirst(p => p.Type == "UserId").Value;
            }
            else
                return StatusCode(StatusCodes.Status401Unauthorized);

            if (Convert.ToInt32(currentId) == 0)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            var purchases = from a in _context.Purchase
                         join c in _context.Course on a.CourseId equals c.CourseId
                         where (a.PurchaseId == purchaseId && a.UserId == Convert.ToInt32(currentId))
                         orderby a.OrderDate
                         select new { a.PurchaseId, a.OrderDate, a.Price, CourseName = c.Name };

            if (purchases.Count() == 0)
            {
                return NotFound();
            }
            return Ok(await purchases.ToListAsync());
        }



        /// <summary>
        /// Ajoute un purchase pour la personne authentifiée
        /// Accessible à toute personne authentifiée
        /// </summary>
        /// <param name="purchase">Objet représentant un purchase</param>
        /// <returns>Retourne l'purchase réalisé.</returns>

        [Authorize]
        // post: api/Purchase/PostPurchaseAuth/5
        [HttpPost("[Action]")]
        public async Task<ActionResult<Purchase>> PostPurchaseAuth(Purchase purchase)
        {
            string currentId = "0";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity.Claims.Count() != 0)
            {
                IEnumerable<Claim> claims = identity.Claims;
                currentId = identity.FindFirst(p => p.Type == "UserId").Value;
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            if (Convert.ToInt32(currentId) != purchase.UserId)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            _context.Purchase.Add(purchase);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetAchat", new { id = purchase.PurchaseId }, purchase);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        /// <summary>
        /// Modifie un purchase de la personne authentifiée
        /// Accessible à toute personne authentifiée
        /// </summary>
        /// <param name="purchase">Objet représentant un purchase</param>
        /// <param name="purchaseId">représente l'purchase modifié</param>
        /// <returns>HTTP 204 No Content indique que la requête a réussi </returns>

        [Authorize]
        // PUT: api/Purchase/PutPurchaseAuth/5
        [HttpPut("[Action]/{purchaseId}")]
        public async Task<IActionResult> PutAchatAuth(int purchaseId, Purchase purchase)
        {
            string currentId = "0";

            if (purchaseId != purchase.PurchaseId)
            {
                return BadRequest();
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity.Claims.Count() != 0)
            {
                IEnumerable<Claim> claims = identity.Claims;
                currentId = identity.FindFirst(p => p.Type == "UserId").Value;
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            if (Convert.ToInt32(currentId) != purchase.UserId)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                if (!PurchaseExists(purchaseId))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
                }
            }
        }



        /// <summary>
        /// Supprime un purchase
        /// Accessible aux administrateurs
        /// </summary>
        /// <param name="purchaseId">Représente l'purchase a supprimer</param>
        /// <returns>HTTP 204 No Content indique que la requête a réussi</returns>
        /// 

        [Authorize(Roles = "Admin")]
        // DELETE: api/Purchase/Delete/5
        [HttpDelete("[Action]/{id}")]
        public async Task<IActionResult> DeletePurchase(int purchaseId)
        {
            var purchase = await _context.Purchase.FindAsync(purchaseId);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchase.Remove(purchase);

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        private bool PurchaseExists(int purchaseId)
        {
            return _context.Purchase.Any(e => e.PurchaseId == purchaseId);
        }

    }
}
