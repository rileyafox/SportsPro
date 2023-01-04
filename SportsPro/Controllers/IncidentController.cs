using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
    {
        private SportsProContext context { get; set; }

        public IncidentController(SportsProContext ctx)
        {
            context = ctx;
        }


        [Route("[controller]s")]
        public IActionResult List(string filter)
        {

            if (filter == null)
                filter = "all";


            IQueryable<Incident> query = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .OrderBy(i => i.DateOpened);

            if(filter == "unassigned")
            {
                query = query.Where(i => i.TechnicianID == null);
            }

            if(filter == "open")
            {
                query = query.Where(i => i.DateClosed == null);
            }

            var incidents = query.ToList();

            IncidentListViewModel model = new IncidentListViewModel();

            model.Filter = filter;
            model.Incidents = incidents;

            return View("List", model);
        }

        public void StoreListsInViewBag()
        {
            ViewBag.Customers = context.Customers
                .OrderBy(c => c.FirstName)
                .ToList();

            ViewBag.Products = context.Products
                .OrderBy(c => c.Name)
                .ToList();

            ViewBag.Technicians = context.Technicians
                .OrderBy(c => c.Name)
                .ToList();
        }

        [HttpGet]
        public IActionResult Add()
        {
            //ViewBag.Action = "Add";

            //StoreListsInViewBag();

            //Instantiate new IncidentViewModel and set the list of
            //Customers, Technicians, and Products along with setting 
            // the Action property to "Add"

            IncidentViewModel model = new IncidentViewModel();
            model.Incident = new Incident();
            model.Action = "Add";

            model.Customers = context.Customers
                .OrderBy(c => c.FirstName)
                .ToList();

            model.Products = context.Products
                .OrderBy(p => p.Name)
                .ToList();

            model.Technicians = context.Technicians
                .OrderBy(t => t.Name)
                .ToList();




            return View("AddEdit", model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //ViewBag.Action = "Edit";

            //StoreListsInViewBag();

            //Instantiate a new IncidentViewModel that sets the Incident property
            //to the current Incident object for the id parameter, the Action to 
            //"Edit" and set the list of Customers, Technicians, and Products

            IncidentViewModel model = new IncidentViewModel();
            var incident = context.Incidents.Find(id);
            model.Incident = incident;


            model.Action = "Edit";
            model.Customers = context.Customers
                .OrderBy(c => c.FirstName)
                .ToList();
            model.Products = context.Products
                .OrderBy(p => p.Name)
                .ToList();
            model.Technicians = context.Technicians
                .OrderBy(t => t.Name)
                .ToList();

            //var product = context.Incidents.Find(id);

            return View("AddEdit", model);
        }

        [HttpPost]
        public IActionResult Save(IncidentViewModel incidentView)
        {
            string successMessage;
            if (ModelState.IsValid)
            {
                if (incidentView.Action == "Add")
                {
                    context.Incidents.Add(incidentView.Incident);
                    successMessage = incidentView.Incident.Title + " was added.";
                }
                else
                {
                    context.Incidents.Update(incidentView.Incident);
                    successMessage = incidentView.Incident.Title + " was updated.";
                }
                context.SaveChanges();
                TempData["message"] = successMessage;
                return RedirectToAction("List");
            }
            else
            {
                //StoreListsInViewBag();
                IncidentViewModel model = new IncidentViewModel();
                var incident = context.Incidents.Find(incidentView.Incident.IncidentID);

                model.Incident = incident;

                if (incidentView.Action == "Add")
                {
                    model.Action = "Add";
                    //ViewBag.Action = "Add";
                }
                else
                {
                    model.Action = "Edit";
                    //ViewBag.Action = "Edit";
                }

                model.Customers = context.Customers
                    .OrderBy(c => c.FirstName)
                    .ToList();
                model.Products = context.Products
                    .OrderBy(p => p.Name)
                    .ToList();
                model.Technicians = context.Technicians
                    .OrderBy(t => t.Name)
                    .ToList();



                return View("AddEdit", model);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = context.Incidents.Find(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            context.Incidents.Remove(incident);
            context.SaveChanges();
            return RedirectToAction("List");
        }
    }
}