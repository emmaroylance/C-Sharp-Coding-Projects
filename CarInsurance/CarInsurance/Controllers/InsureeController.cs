using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        public InsuranceEntities db = new InsuranceEntities();


        // GET: Insuree
        public ActionResult Index()
        { 
        return View(db.Insurees.ToList());

        }

        public void getQuote(Insuree person)
        {
    
            using (InsuranceEntities db = new InsuranceEntities())
            {
                    person.Quote = 50;
                    int underAge = 25;
                    int veryUnderAge = 100;
                    int overAge = 100;
                    int oldCar = 25;
                    int newCar = 25;
                    int porscheMake = 25;
                    int carreraModel = 25;
                    int speedingTickets = 10;
                    decimal DUIYesOrNo = 0.25m;
                    decimal fullCoverage = 0.5m;
                    var currentDate = DateTime.Now;
                    var currentYearMinusTwentyFive = currentDate.AddYears(-25);
                    var currentYearMinusEighteen = currentDate.AddYears(-18);
                    var currentYearMinusOneHundred = currentDate.AddYears(-100);


                if (person.DateOfBirth > currentYearMinusEighteen)
                {
                person.Quote += veryUnderAge;
                }

                else if (person.DateOfBirth > currentYearMinusTwentyFive)
                {
                    person.Quote += underAge;
                }

                if (person.DateOfBirth < currentYearMinusOneHundred)
                {
                    person.Quote += overAge;
                }

                if (person.CarYear < 2000)
                {
                    person.Quote += oldCar;
                }

                if (person.CarYear > 2015)
                {
                    person.Quote += newCar;
                }

                if (person.CarMake.ToLower() == "porsche")
                {
                    person.Quote += porscheMake;
                    if (person.CarModel.ToLower() == "911 carrera")
                    {
                        person.Quote += carreraModel;
                    }
                }

                if (person.SpeedingTickets > 0)
                {
                    person.Quote += speedingTickets * person.SpeedingTickets;
                }

                if (person.DUI == true)
                {
                    person.Quote += person.Quote * DUIYesOrNo;
                }

                if (person.CoverageType == true)
                {
                    person.Quote += person.Quote * fullCoverage;
                }





                    //DUI and fullcoverage

            }

        }

        public ActionResult Admin()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }


            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(insuree.FirstName) || string.IsNullOrEmpty(insuree.LastName)
                                                            || string.IsNullOrEmpty(insuree.EmailAddress)
                                                            || insuree.DateOfBirth==DateTime.MinValue
                                                            || Convert.ToString(insuree.CarYear) == String.Empty
                                                            || string.IsNullOrEmpty(insuree.CarMake)
                                                            || string.IsNullOrEmpty(insuree.CarModel)
                                                            || Convert.ToString(insuree.SpeedingTickets) == String.Empty
                )
                {
                    return View("~/Views/Insuree/Error.cshtml");
                }
                else {
                    getQuote(insuree);
                    db.Insurees.Add(insuree);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }


            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                getQuote(insuree);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
